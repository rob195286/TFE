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
        // Ensemble de tous les vertices / noeuds qui consituent le graphe.
        private Dictionary<int, Vertex> _vertices = new Dictionary<int, Vertex>() { };
        // Ensemble de toutes les arêtes qui consituent le graphe.
        private Dictionary<int, List<Edge>> _edges = new Dictionary<int, List<Edge>>() { };

        public Graph(string filePath = @"A:\3)_Bibliotheque\Documents\Ecam\Anne5\TFE\Code\ways.csv")
        {
            CreateGraph(filePath);
        }

        private Vertex GetNode(int id, double lon, double lat)
        {
            if (!NodeExist(id))
            {
                AddNode(new Vertex(id, lon, lat));
            }
            return GetNode(id);
        }
        private void CreateGraph(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {   // Permet d'ajouter des champs null car certaines valeurs dans la base de données le sont.
                csv.Context.TypeConverterOptionsCache.GetOptions<double?>().NullValues.Add("NULL");
                foreach (CSVwayData way in csv.GetRecords<CSVwayData>())
                {   // Créer ou récupère les vertices et arêtes à partir des données récupérées de la ligne du fichier CSV.
                    Vertex sourceNode = GetNode(way.source, way.x1, way.y1);
                    Vertex targetNode = GetNode(way.target, way.x2, way.y2);
                    Edge edge = new Edge(way.length,
                                         way.cost_s ?? 99999, // way.cost_s on vérifie que le champ n'est pas null. S'il l'est, mieux vaut l'ignorer.
                                         way.maxspeed_forward,
                                         way.maxspeed_backward,
                                         way.osm_id,
                                         way.tag_id,
                                         way.one_way); 
                    // Sauvegarde l'arête dans le dictinnaire d'arêtes "_edges" pour pouvoir modifier leur coût.
                    _SaveEdge(edge);
                    //------------------------------- one way
                    sourceNode.AddOutgoingEdge(edge);
                    edge.sourceVertex = sourceNode;
                    edge.targetVertex = targetNode;
                    //------------------------------- two way
                    if (way.one_way == 2 || way.one_way == 0)
                    {
                        edge = new Edge(way.length,
                                        way.reverse_cost_s ?? 99999,
                                        way.maxspeed_forward,
                                        way.maxspeed_backward,
                                        way.osm_id,
                                        way.tag_id,
                                        way.one_way); 
                        targetNode.AddOutgoingEdge(edge);
                        edge.sourceVertex = targetNode;
                        edge.targetVertex = sourceNode;
                        _SaveEdge(edge);
                    }
                }
            }
        }
        /// <summary>
        ///     Fonction sauvegardant l'arête passée en paramètre en créant une nouvelle liste d'arêtes si 
        ///     son identifiant n'existe pas déjà dans le dictionnaire et en l'ajoutant à la liste si
        ///     l'arête existe déjà.
        /// </summary>
        /// <param name="edge"> Arête à ajouter dans le dictionnaire contenant toutes les arêtes. </param>
        private void _SaveEdge(Edge edge)
        {
            if (!_edges.ContainsKey(edge.osmID))
            {
                _edges.Add(edge.osmID, new List<Edge>() { edge });
            }
            else
            {
                _edges[edge.osmID].Add(edge);
            }
        }
        public bool NodeExist(int id)
        {
            return _vertices.ContainsKey(id);
        }
        public bool AddNode(Vertex vertex)
        {
            if (!NodeExist(vertex.id))// éviter d'ajouter 2x la même key et d'avoir une exception
            {
                _vertices.Add(vertex.id, vertex);
                return true;
            }
            return false;
        }
        public Vertex GetNode(int vertexID)
        {
            Vertex vertex = null; // todo : voir comment gére exception
            try
            {
                vertex = _vertices[vertexID];
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("aucun vertex trouvé");// throw;
            }
            return vertex;
        }
        /// <summary>
        ///     Fonction permettant de récupérer les arêtes liées à un identifiant OSM.
        /// </summary>
        /// <param name="osmID"> Identifiant OSM des arêtes qu'on voudrait récupérer. </param>
        /// <returns> Renvois la liste d'arêtes lié à l'identifiant OSM passé en paramètre. </returns>
        public List<Edge> GetEdges(int osmID)
        {
            return _edges[osmID];
        }
        public IEnumerable<Edge> GetNextEdges(int vertexID, int visitId)
        {
            foreach (Edge edge in GetNode(vertexID).outgoingEdges)
            {
                if (edge.targetVertex.lastVisit != visitId && edge.cost >= 0)
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
        public List<Vertex> GetNextNodes(int vertexID)
        {
            List<Vertex> nodes = new List<Vertex>();
            foreach (Edge edge in GetNode(vertexID).outgoingEdges)
            {
                nodes.Add(edge.targetVertex);
            }
            return nodes;
        }
        /// <summary>
        ///     Cette fonction permet de changer le coût des arêtes par rapport à un identifiant.
        ///     L'ensemble des arêtes ayant cette identifiant seront modifiées d'un facteur égale
        ///     à celui du nombre indiqué comme deuxième paramètre.
        /// </summary>
        /// <param name="osmId"> Identifiant des arêtes dont on veut modifier le coût. </param>
        /// <param name="multiplier"> Nombre multiplicateur qui modifiera le coût des arêtes trouvées. </param>
        public void ChangeEdgeCost(int osmId, double multiplier)
        {
            _edges[osmId].ForEach(delegate (Edge edge) {
                edge.cost *= multiplier;
            });
        }
    }

    //--------------------------------------------------------------------------------------------------------

    public class Vertex
    {
        // Identifiant OSM du vertex (dans ce cas il s'agit de la valeur "source" tirée de la base de données).
        public int id { get; private set; }
        // Liste des arêtes du vertex qui mène vers un des autres vertex.
        public List<Edge> outgoingEdges { get; private set; } = new List<Edge>();
        // Latitude du vertex.
        public double latitude { get; private set; }
        // Longitude du vertex.
        public double longitude { get; private set; }
        // Valeur de l'identifiant de la dernière fois que le vertex a été visité pour la recherche forward.
        public int lastVisit = 0;

        public Vertex(int pid, double plon, double plat)
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
            return "vertex info :" +
                    "\n-----------" +
                    "\n - id : " + id +
                    "\n - latitude : " + latitude +
                    "\n - longitude : " + longitude +
                    "\n";
        }
    }

    public class Edge
    {
        // Identifiant OSM de l'arête (il s'agit de la valeur "osm_id" dans la base de données).
        public int osmID { get; private set; }
        // Vertex d'où l'arête part (puisque le graphe est dirigé) pour mener au vertex suivant dans la recherche forward.
        public Vertex sourceVertex { get; set; } = new Vertex(-1, 0, 0);
        // Vertex d'où l'arête part (puisque le graphe est dirigé) pour mener au vertex précédent dans la recherche backward.
        public Vertex targetVertex { get; set; } = new Vertex(-1, 0, 0);
        // Longueur de l'arête.
        public double length { get; set; }
        // Coût pour traverser l'arête (celle qui sera utilisée par l'algorithme Dijkstra).
        public double cost { get; set; }
        // Vitesse maximum pour le parcourt dans le sens forward.
        public double maxSpeedForward { get; private set; }
        // Vitesse maximum pour le parcourt dans le sens backward.
        public double maxSpeedBackward { get; private set; }
        // Type de route (auto-route, tram...).
        public int tagID { get; private set; }
        // Indication sur le sens de parcourt de la route, unidirectionnelle ou bidirectionnelle.
        public int oneWay { get; private set; }

        public Edge(double plength,
                    double pcost,
                    int pmaxSpeedForward,
                    int pmaxSpeedBackward,
                    int posmID,
                    int ptagID,
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
                    "\n-----------" +
                    "\n - length : " + length +
                    "\n - vertex source id : " + sourceVertex.id +
                    "\n - vertex target id : " + targetVertex.id +
                    "\n - totalCost : " + cost +
                    "\n - maxSpeedForward : " + maxSpeedForward +
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

