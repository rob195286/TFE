using CsvHelper;
using CsvHelper.Configuration.Attributes;
using Priority_Queue;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace TFE
{
    public class Graph
    {
        private Dictionary<int, GraphNode> _nodes;

        public Graph(string filePath = "ways.csv")
        {
            _nodes = new Dictionary<int, GraphNode>() { };
            CreateGraph(filePath);
        }

        private GraphNode GetNode(int id, double lon, double lat)
        {
            if (NodeExist(id))
            {
                return GetNode(id);
            }
            else
            {
                return new GraphNode(id, lon, lat);
            }
        }
        private void CreateGraph(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {   // permet d'ajouter des champs null.
                csv.Context.TypeConverterOptionsCache.GetOptions<double?>().NullValues.Add("NULL");
                foreach (CSVwayData way in csv.GetRecords<CSVwayData>())
                {
                    GraphNode sourceNode = GetNode(way.source, way.x1, way.y1);
                    GraphNode targetNode = GetNode(way.target, way.x2, way.y2);
                    Edge edge = new Edge(way.length_m, way.name, way.cost, way.cost_s, way.maxspeed_forward);
                    //------------------------------- one way
                    sourceNode.AddOutgoingEdge(edge);
                    edge.sourceNode = sourceNode;
                    edge.targetNode = targetNode;
                    //------------------------------- two way
                    if (way.one_way == 2 || way.one_way == 0)
                    {
                        edge = new Edge(way.length_m, way.name, way.reverse_cost, way.reverse_cost_s, way.maxspeed_backward);
                        targetNode.AddOutgoingEdge(edge);
                        edge.sourceNode = targetNode;
                        edge.targetNode = sourceNode;
                    }
                    AddNode(sourceNode);
                    AddNode(targetNode);
                }
            }
        }
        public bool NodeExist(int id)
        {
            return _nodes.ContainsKey(id);
        }
        public bool AddNode(GraphNode node)
        {
            if (!NodeExist(node.id))// éviter d'ajouter 2x la même key et d'avoir une exception
            {
                _nodes.Add(node.id, node);
                return true;
            }
            return false;
        }
        public GraphNode GetNode(int nodeID)
        {
            GraphNode node = null; // todo : voir comment gére exception
            try
            {
                node = _nodes[nodeID];
            }
            catch (System.Collections.Generic.KeyNotFoundException)
            {
                Console.WriteLine("aucun graphNode trouvé");// throw;
            }
            return node;
        }
        public IEnumerable<Edge> GetNextEdges(int nodeID, int visitId, int finalNodeID)
        {
            foreach (Edge edge in GetNode(nodeID).outgoingEdges)
            {
                if (edge.targetNode.VisitID != visitId && edge.cost >= 0)
                {
                    yield return edge;
                }
            }
        }
        /// <summary>
        /// Fonction utilisée pour les test unitaires, récupère les noeuds suivant au noeud inséré.
        /// </summary>
        /// <param name="nodeID"> Id du noeud à partir duquel on veut trouver ses voisins. </param>
        /// <returns> Retourne une liste de noeuds contenant l'nsemble des noeuds voisins. </returns>
        public List<GraphNode> GetNextNodes(int nodeID)
        {
            List<GraphNode> nodes = new List<GraphNode>();
            foreach (Edge edge in GetNode(nodeID).outgoingEdges)
            {
                nodes.Add(edge.targetNode);
            }
            return nodes;
        }
    }

    //--------------------------------------------------------------------------------------------------------

    public class GraphNode
    {
        public int id { get; private set; }
        public List<Edge> outgoingEdges { get; private set; }
        public double latitude { get; private set; }
        public double longitude { get; private set; }
        public int VisitID;

        public GraphNode(int pid, double plon, double plat)
        {
            id = pid;
            outgoingEdges = new List<Edge>();
            latitude = plat;
            longitude = plon;
        }

        public void AddOutgoingEdge(Edge edge)
        {
            outgoingEdges.Add(edge);
        }
        public override string ToString()
        {
            return "graphNode info :" +
                    "\n-----------" +
                    "\n - id : " + id +
                    "\n - latitude : " + latitude +
                    "\n - longitude : " + longitude +
                    "\n";
        }
    }

    public class Edge
    {
        public double? length_m { get; private set; }
        public string roadName { get; private set; }
        public GraphNode sourceNode { get; set; }
        public GraphNode targetNode { get; set; }
        public double cost { get; private set; }
        public double? costS { get; private set; }
        public int maxSpeedForward { get; private set; }

        public Edge(double? plength_m, string proadName,
                    double pcost, double? pcoastS,
                    int pmaxSpeedForward)
        {
            length_m = plength_m;
            roadName = proadName;
            sourceNode = new GraphNode(-1, 0, 0);
            targetNode = new GraphNode(-1, 0, 0);
            cost = pcost;
            costS = pcoastS;
            maxSpeedForward = pmaxSpeedForward;
        }

        public override string ToString()
        {
            return "edge info :" +
                    "\n-----------" +
                    "\n - length_m : " + length_m +
                    "\n - roadName : " + roadName +
                    "\n - graphNode source id : " + sourceNode.id +
                    "\n - graphNode target id : " + targetNode.id +
                    "\n - costS : " + cost +
                    "\n - costS : " + costS +
                    "\n - maxSpeedForward : " + maxSpeedForward +
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
        // [Index(16)]
        // public int tag_id { get; set; }
    }
}

