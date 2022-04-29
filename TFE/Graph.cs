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
        private Dictionary<int, Vertex> _vertices;
        private Dictionary<int, Edge> _edges;

        public Graph(string filePath = @"A:\3)_Bibliotheque\Documents\Ecam\Anne5\TFE\Code\ways.csv")
        {
            _vertices = new Dictionary<int, Vertex>() { };
            _edges = new Dictionary<int, Edge>() { };
            _CreateGraph(filePath);
        }

        private Vertex _GetVertex(int id, double lon, double lat)
        {
            if (VertexExist(id))
            {
                return _GetVertex(id);
            }
            else
            {
                AddVertex(new Vertex(id, lon, lat));
                return _GetVertex(id);
            }
        }
        private void _CreateGraph(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {   // permet d'ajouter des champs null.
                csv.Context.TypeConverterOptionsCache.GetOptions<double?>().NullValues.Add("NULL");
                foreach (CSVwayData way in csv.GetRecords<CSVwayData>())
                {
                    Vertex sourceVertex = _GetVertex(way.source, way.x1, way.y1);
                    Vertex targetVertex = _GetVertex(way.target, way.x2, way.y2);
                    Edge edge = new Edge(way.length_m, way.name, way.cost, way.cost_s ?? 999999, way.maxspeed_forward, way.osm_id); // way.cost_s on vérifie que le champ n'est pas null. S'il l'est, mieux vaut l'ignorer.
                    _SaveEdge(edge);
                    //------------------------------- one way
                    sourceVertex.AddOutgoingEdge(edge);
                    targetVertex.AddReverseOutgoingEdge(edge); // Ajout pour le reverse graph
                    edge.sourceVertex = sourceVertex;
                    edge.targetVertex = targetVertex;
                    //------------------------------- two way
                    if (way.one_way == 2 || way.one_way == 0)
                    {
                        edge = new Edge(way.length_m, way.name, way.reverse_cost, way.reverse_cost_s ?? 999999, way.maxspeed_backward, way.osm_id);
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
            /*
            if (!_edges.ContainsKey(edge.osmId))
                _edges.Add(edge.osmId, edge);
            */
            // todo : à remettre une fois fini
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
        public Vertex _GetVertex(int vertexID)
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
            foreach (Edge edge in _GetVertex(vertexID).outgoingEdges)
            {
                if (edge.targetVertex.lastVisitForward != visitId && edge.costS >= 0)
                {
                    yield return edge;
                }
            }
        }   
        public IEnumerable<Edge> GetPreviousEdges(int vertexID, int visitId)
        {
            foreach (Edge edge in _GetVertex(vertexID).reverseOutgoingEdges)
            {
                if (edge.sourceVertex.lastVisitBackward != visitId && edge.costS >= 0)
                {
                    yield return edge;
                }
            }
        }     
        public void ChangeEdgeCost(int osmId, double newCost, double newCostS)
        {
            if (newCostS == null)
            {
                _edges[osmId].costS = newCost;
            }
            else if (newCost == null)
            {
                _edges[osmId].costS = newCostS;
            }
        }
    }

    //--------------------------------------------------------------------------------------------------------

    public class Vertex
    {
        // todo : remettre données
        public int id { get; private set; }
        public List<Edge> outgoingEdges { get; private set; }
        public List<Edge> reverseOutgoingEdges { get; private set; }
        //public double latitude { get; private set; }
        //public double longitude { get; private set; }
        public int lastVisitForward = 0;
        public int lastVisitBackward = 0;

        public Vertex(int pid, double plon, double plat)
        {
            id = pid;
            outgoingEdges = new List<Edge>();
            reverseOutgoingEdges = new List<Edge>();
            //latitude = plat;
            //longitude = plon;
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
                   // "\n - latitude : " + latitude +
                    //"\n - longitude : " + longitude +
                    "\n";
        }
    }

    public class Edge
    {
        // todo : enlever inutile
        //public double? length_m { get; private set; }
        public Vertex sourceVertex { get; set; }
        public Vertex targetVertex { get; set; }
        //public double cost { get; set; }
        public double costS { get; set; }
        //public int maxSpeedForward { get; private set; }
        public int osmId { get; private set; }

        public Edge(double? plength_m, string proadName,
                    double pcost, double pcoastS,
                    int pmaxSpeedForward, int posmId)
        {
            //length_m = plength_m;
            //roadName = proadName;
            sourceVertex = new Vertex(-1, 0, 0);
            targetVertex = new Vertex(-1, 0, 0);
            //cost = pcost;
            costS = pcoastS;
            //maxSpeedForward = pmaxSpeedForward;
            osmId = posmId;
        }

        public override string ToString()
        {
            return "edge info :" +
                    "\n" +
                   // "\n - length_m : " + length_m +
                    //"\n - roadName : " + roadName +
                    "\n - vertex source id : " + sourceVertex.id +
                    "\n - vertex target id : " + targetVertex.id +
                   // "\n - totalCostS : " + cost +
                    "\n - totalCostS : " + costS +
                    //"\n - maxSpeedForward : " + maxSpeedForward +
                    "\n - osm Id : " + osmId +
                    "\n";
        }
    }

    public class CSVwayData
    {
        [Index(0)]
        public int gid { get; set; }
        [Index(1)]
        public double? length_m { get; set; }
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
    }
}