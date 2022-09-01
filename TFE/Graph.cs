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
        public Dictionary<int, Vertex> _vertices = new Dictionary<int, Vertex>() { };
        // Ensemble de toutes les arêtes qui consituent le graphe.
        public Dictionary<long, List<Edge>> _edges = new Dictionary<long, List<Edge>>() { };
        // Variable comptant le nombre d'arêtes ayant un sens.
        public int nombre_ways_oneway = 0;
        // Variable comptant le nombre d'arêtes ayant deux sens.
        public int nombre_ways_twoway = 0;

        /// <summary>
        ///     Créer le graphe à partir d'un ficheir csv.
        /// </summary>
        /// <param name="filePath"> Chemin du csv contenant l'ensemble des données pour créer le graphe.</param>
        public Graph(string filePath = @"A:\3)_Bibliotheque\Documents\Ecam\Anne5\TFE\Code\ways.csv")
        {
            CreateGraph(filePath);
        }

        /// <summary>
        ///     Ajoute un vertex au graphe, soit dans la liste des vertexes (_vertices).
        /// </summary>
        /// <param name="vertex"> Vertex à ajouter.</param>
        /// <returns> True s'il est ajouté, false sinon.</returns>
        private bool _AddVertex(Vertex vertex)
        {
            if (!VertexExist(vertex.id))// todo : éviter d'ajouter 2x la même key et d'avoir une exception
            {
                _vertices.Add(vertex.id, vertex);
                return true;
            }
            return false;
        }
        /// <summary>
        ///     Récupère un vertex du dictionnaire de vertex s'il existe déjà ou bien en créer un nouveau et 
        ///         l'ajoute au dictionnaire.
        /// </summary>
        /// <param name="csvRow"> La ligne du csv contenant les informations des arêtes et des vertices.</param>
        /// <param name="isSourceVertex"> 
        ///     Indique si le vertex sera un vertex source ou target dans le cas où on a une arête 
        ///         bidirectionnelle.              
        /// </param>
        /// <returns> Retourne le vertex existant ou celui créé.</returns>
        private Vertex GetVertex(CSVwayData csvRow, bool isSourceVertex)
        {
            int id = isSourceVertex ? csvRow.source : csvRow.target;
            double lon = isSourceVertex ? csvRow.x1 : csvRow.x2;
            double lat = isSourceVertex ? csvRow.y1 : csvRow.y2;
            if (!VertexExist(id))
            {
                _AddVertex(new Vertex(id, lon, lat));
            }
            return GetVertex(id);
        }
        /// <summary>
        ///     Fonction sauvegardant l'arête passée en paramètre en créant une nouvelle liste d'arêtes si 
        ///     son identifiant n'existe pas déjà dans le dictionnaire et en l'ajoutant à la liste si
        ///     l'arête existe déjà.
        /// </summary>
        /// <param name="edge"> Arête à ajouter dans le dictionnaire contenant toutes les arêtes. </param>
        public void _AddEdge(Edge edge)
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
        /// <summary>
        ///     Même opération que pour le vertex, on récupère l'arête si elle existe, 
        ///         sinon on retourne celle créée et ajoutée au dictionnaire d'arêtes.
        /// </summary>
        /// <param name="csvRow"> La ligne du csv contenant les informations des arêtes et des vertices.</param>
        /// <param name="vSource"> Vertex source d'où part l'arête.</param>
        /// <param name="vTarget"> Vertex target où mène l'arête.</param>
        /// <param name="isForwardEdge"> 
        ///     Indique si l'arête est une arête forward ou backward 
        ///     dans le cas où elle est bidirectionnelle.
        /// </param>
        /// <returns> Retourne l'arête existante ou celle créée</returns>
        private Edge _GetEdge(CSVwayData csvRow, Vertex vSource, Vertex vTarget, bool isForwardEdge)
        {
            Edge edge = new Edge(csvRow.length,
                                isForwardEdge ? csvRow.cost : csvRow.reverse_cost,
                                csvRow.maxspeed_forward,
                                csvRow.maxspeed_backward,
                                csvRow.osm_id,
                                csvRow.tag_id,
                                csvRow.one_way);
            edge.sourceVertex = vSource;
            edge.targetVertex = vTarget;
            _AddEdge(edge);
            return edge;
        }
        /// <summary>
        ///     Fonction qui se charge de créer le graphe en ajoutant les vertices et les arêtes 
        ///         dans leur dictionnaire respectif.
        ///     Cette fonction prendra les lignes du CSV une par une pour pouvoir créer les vertices et arêtes.
        /// </summary>
        /// <param name="filePath"> Chemin d'accès à partir d'où le fichier CSV contenant les informations seront prise. </param>
        private void CreateGraph(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {   // Permet d'ajouter des champs null car certaines valeurs dans la base de données le sont.
                csv.Context.TypeConverterOptionsCache.GetOptions<double?>().NullValues.Add("NULL");
                foreach (CSVwayData way in csv.GetRecords<CSVwayData>())
                {   // Créer ou récupère les vertices et arêtes à partir des données de la ligne du fichier CSV.
                    Vertex sourceNode = GetVertex(way, true);
                    Vertex targetNode = GetVertex(way, false);
                    //------------------------------- one way
                    sourceNode.AddOutgoingEdge(_GetEdge(way, sourceNode, targetNode, true));
                    //------------------------------- two way
                    nombre_ways_oneway++;
                    if (way.one_way != 1)
                    {
                        nombre_ways_twoway++;
                        targetNode.AddOutgoingEdge(_GetEdge(way, targetNode, sourceNode, false));
                    }
                }
            }
        }
        /// <summary>
        ///     Fonction permettant de vérifier si un vertex existe ou pas dans le graphe.
        /// </summary>
        /// <param name="id"> Identifiant du vertex dont on aimerais savoir s'il existe. </param>
        /// <returns> Renvois true s'il existe, false sinon. </returns>
        public bool VertexExist(int id)
        {
            return _vertices.ContainsKey(id);
        }
        /// <summary>
        ///     Fonction permettant de vérifier si une arête existe ou pas dans le graphe.
        /// </summary>
        /// <param name="edge"> Edge dont on veut vérifier l'existance.</param>
        /// <returns> Renvois true si elle existe, false sinon.</returns>
        public bool EdgeExist(Edge edge)
        {
            if (_edges.ContainsKey(edge.osmID))
            {
                return _edges[edge.osmID].Exists(e => e.Equals(edge));
            }
            else { return false; }
        }
        /// <summary>
        ///     Fonction permettant d'obtenir un vertex contenu dans le graphe de l'extérieur.
        /// </summary>
        /// <param name="vertexID"> Identifiant du vertex à trouver. </param>
        /// <returns> Retourne le vertex correspondant à l'identifiant s'il existe, sinon renvois une exception. </returns>
        public Vertex GetVertex(int vertexID)
        {
            Vertex vertex = null;
            try
            {
                vertex = _vertices[vertexID];
                return _vertices[vertexID];
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("aucun vertex trouvé");
            }
            return vertex;
        }
        /// <summary>
        ///     Fonction permettant de récupérer les arêtes liées à un identifiant OSM.
        /// </summary>
        /// <param name="osmID"> Identifiant OSM des arêtes qu'on voudrait récupérer. </param>
        /// <returns> Renvois la liste d'arêtes lié à l'identifiant OSM passé en paramètre. </returns>
        public List<Edge> GetEdges(long osmID)
        {
            return _edges[osmID];
        }
        /// <summary>
        ///     Fonction permettant de récupérer les arêtes du vertex correspondant à un idfentifiant donné
        ///         pour le premier sens (forward).
        ///     Elle permet aussi de vérifier si un noeud a déjà été visité pour ne pas retourner
        ///         des noeuds inutiles.
        /// </summary>
        /// <param name="vertexID"> Identifiant du vertex à partir duquel on veut récupérer les arêtes. </param>
        /// <param name="visitId"> Identifiant correspondant au numéro d'appel de l'algorithme. </param>
        /// <returns> Renvois les arêtes les une après les autres si elles n'ont pas déjà été visitées. </returns>
        public IEnumerable<Edge> GetNextEdges(int vertexID, int visitId)
        {
            foreach (Edge edge in GetVertex(vertexID).outgoingEdges)
            {
                if (edge.cost >= 0)// && edge.targetVertex.lastVisit != visitId)
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
        public List<Vertex> GetNextVertices(int vertexID)
        {
            List<Vertex> nodes = new List<Vertex>();
            foreach (Edge edge in GetVertex(vertexID).outgoingEdges)
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
        public void ChangeEdgeCost(long osmId, double multiplier)
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

        /// <summary>
        ///     Ajoute une arête au noeud qui mène vers le noued suivant.
        /// </summary>
        /// <param name="edge"> Arête à ajouter.</param>
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
        public long osmID { get; private set; }
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
                    long posmID,
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

        public override int GetHashCode()
        {
            return HashCode.Combine(osmID, sourceVertex.id, targetVertex.id, cost, length);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Edge);
        }

        public bool Equals(Edge other)
        {
            return other != null &&
                   EqualityComparer<int>.Default.Equals(sourceVertex.id, other.sourceVertex.id) &&
                   EqualityComparer<int>.Default.Equals(targetVertex.id, other.targetVertex.id) &&
                   cost == other.cost;
        }
    }
    /// <summary>
    ///     Classe se chargeant de récupérer les valeurs contenues dans un fichier CSV pour pouvoir les utiliser lors de 
    ///         la création du graphe dans lafonction "_createGraph".
    /// </summary>
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
        public long osm_id { get; set; }
        [Index(17)]
        public int tag_id { get; set; }
    }
}

