using CsvHelper;
using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace TFE
{
    public class Graph
    {
        private Dictionary<int, Vertex> _vertices = new Dictionary<int, Vertex>() { };
        private Dictionary<int, List<Edge>> _edges = new Dictionary<int, List<Edge>>() { };

        public Graph(string filePath = @"A:\3)_Bibliotheque\Documents\Ecam\Anne5\TFE\Code\ways.csv")
        {
            _CreateGraph(filePath);
        }

        private Vertex GetVertex(int id, double lon, double lat)
        {
            if (VertexExist(id))
            {
                return GetVertex(id);
            }
            else
            {
                AddVertex(new Vertex(id, lon, lat));
                return GetVertex(id);
            }
        }
        private void _CreateGraph(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {   
                csv.Context.TypeConverterOptionsCache.GetOptions<double?>().NullValues.Add("NULL"); // permet d'ajouter des champs null.
                foreach (CSVwayData way in csv.GetRecords<CSVwayData>())
                {
                    Vertex sourceVertex = GetVertex(way.source, way.x1, way.y1);
                    Vertex targetVertex = GetVertex(way.target, way.x2, way.y2);
                    Edge edge = new Edge(way.length,
                                         way.cost,
                                         way.maxspeed_forward,
                                         way.maxspeed_backward,
                                         way.osm_id,
                                         way.tag_id,
                                         way.one_way); // way.cost_s on vérifie que le champ n'est pas null. S'il l'est, mieux vaut l'ignorer.
                    _SaveEdge(edge);
                    //------------------------------- one way
                    sourceVertex.AddOutgoingEdge(edge);
                    targetVertex.AddReverseOutgoingEdge(edge); // Ajout pour le reverse graph
                    edge.sourceVertex = sourceVertex;
                    edge.targetVertex = targetVertex;
                    //------------------------------- two way
                    if (way.one_way == 2 || way.one_way == 0)
                    {
                        edge = new Edge(way.length,
                                        way.reverse_cost,
                                        way.maxspeed_forward,
                                        way.maxspeed_backward,
                                        way.osm_id,
                                        way.tag_id,
                                        way.one_way);
                        targetVertex.AddOutgoingEdge(edge);
                        sourceVertex.AddReverseOutgoingEdge(edge); // Ajout pour le reverse graph
                        edge.sourceVertex = targetVertex;
                        edge.targetVertex = sourceVertex;
                    }
                }
            }
        }
        private void _SaveEdge(Edge edge)
        {
            if (_edges.ContainsKey(edge.osmID))
            {
                _edges[edge.osmID].Add(edge);
            }
            else
            {
                _edges.Add(edge.osmID, new List<Edge>() { edge });
            }
        }
        public bool VertexExist(int id)
        {
            return _vertices.ContainsKey(id);
        }
        public bool AddVertex(Vertex vertex)
        {
            if (!VertexExist(vertex.id))// éviter d'ajouter 2x la même key et d'avoir une exception
            {
                _vertices.Add(vertex.id, vertex);
                return true;
            }
            return false;
        }
        public Vertex GetVertex(int vertexID)
        {
            Vertex node = null; // todo : voir comment gére exception
            try
            {
                node = _vertices[vertexID];
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("aucun vertex trouvé");// throw;
            }
            return node;
        }
        public IEnumerable<Edge> GetNextEdges(int vertexID, int visitId)
        {
            foreach (Edge edge in GetVertex(vertexID).outgoingEdges)
            {
                if (edge.targetVertex.lastVisitForward != visitId && edge.cost >= 0)
                {
                    yield return edge;
                }
            }
        }   
        public IEnumerable<Edge> GetNextReverseEdges(int vertexID, int visitId)
        {
            foreach (Edge edge in GetVertex(vertexID).reverseOutgoingEdges)
            {
                if (edge.sourceVertex.lastVisitBackward != visitId && edge.cost >= 0)
                {
                    yield return edge;
                }
            }
        }     
        public List<Edge> GetEdge(int osmID)
        {
            return _edges[osmID];
        }
        public void ChangeEdgeCost(int osmId, double newCost)
        {
            _edges[osmId].ForEach(delegate(Edge edge) {
                edge.cost *= newCost;
            });
        }
    }

    //--------------------------------------------------------------------------------------------------------

    public class Vertex
    {
        public int id { get; private set; }
        public List<Edge> outgoingEdges { get; private set; } = new List<Edge>();
        public List<Edge> reverseOutgoingEdges { get; private set; } = new List<Edge>();
        public double latitude { get; private set; }
        public double longitude { get; private set; }
        public int lastVisitForward = 0;
        public int lastVisitBackward = 0;

        public Vertex(int pid, double plon, double plat)
        {
            id = pid;
            latitude = plat;
            longitude = plon;
        }

        public void AddOutgoingEdge(Edge edge)
        {
            outgoingEdges.Add(edge);
        }
        public void AddReverseOutgoingEdge(Edge edge)
        {
            reverseOutgoingEdges.Add(edge);
        }
        public override string ToString()
        {
            return "Vertex info :" +
                    "\n" +
                    "\n - id : " + id +
                    "\n - visited F : " + lastVisitForward +
                    "\n - visited B : " + lastVisitBackward +
                    "\n - latitude : " + latitude +
                    "\n - longitude : " + longitude +
                    "\n";
        }
    }

    public class Edge
    {
        public int osmID { get; private set; }
        public Vertex sourceVertex { get; set; } = new Vertex(-1, 0, 0);
        public Vertex targetVertex { get; set; } = new Vertex(-1, 0, 0);
        public double length { get; set; }
        public double cost { get; set; }
        public double maxSpeedForward { get; private set; }
        public double maxSpeedBackward { get; private set; }
        public int tagID { get; private set; }
        public int oneWay { get; private set; }

        public Edge(double plength, 
                    double pcost,
                    int pmaxSpeedForward, 
                    int pmaxSpeedBackward, 
                    int posmID,
                    int ptagID, // Type de route.
                    int poneWay
            )
        {
            length = plength;
            cost = pcost;
            maxSpeedForward = pmaxSpeedForward;
            maxSpeedBackward = pmaxSpeedBackward;
            osmID = posmID;
            tagID = ptagID;
            oneWay = poneWay;
        }

        public override string ToString()
        {
            return "edge info :" +
                    "\n" +
                    "\n - vertex source id : " + sourceVertex.id +
                    "\n - vertex target id : " + targetVertex.id +
                    "\n - cost : " + cost +
                    "\n - osm Id : " + osmID +
                    "\n";
        }
    }

    public class CSVwayData
    {
        [Index(0)]
        public int gid { get; set; }
        [Index(1)]
        public double length { get; set; }
        [Index(2)]
        public string name { get; set; }
        [Index(3)]
        public int source { get; set; }
        [Index(4)]
        public int target { get; set; }
        [Index(5)]
        public double cost { get; set; }
        [Index(6)]
        public double reverse_cost { get; set; }
        [Index(7)]
        public double? cost_s { get; set; }
        [Index(8)]
        public double? reverse_cost_s { get; set; }
        [Index(9)]
        public int one_way { get; set; }
        [Index(10)]
        public double x1 { get; set; }
        [Index(11)]
        public double y1 { get; set; }
        [Index(12)]
        public double x2 { get; set; }
        [Index(13)]
        public double y2 { get; set; }
        [Index(14)]
        public int maxspeed_forward { get; set; }
        [Index(15)]
        public int maxspeed_backward { get; set; }
        [Index(16)]
        public int osm_id { get; set; }
        [Index(17)]
        public int tag_id { get; set; }
    }
}