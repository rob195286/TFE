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
            _CreateGraph(filePath);
        }
        /// <summary>
        ///     Cette fonction permet d'obtenir un vertex, soit en le créant, soit en le récupérant du dictionnaire 
        ///     contenant l'ensemble des vertices à partir de la méthode "GetVertex(int)".
        /// </summary>
        /// <param name="id"> Identifiant du vertex qui doit être créé ou récupréré s'il existe déjà. </param>
        /// <param name="lon"> Longitude du vertex à créer, soit l'une de ses deux coordonnées de géolocalisation. </param>
        /// <param name="lat"> Latitude du vertex à créer, soit la deuxième coordonnées de géolocalisation. </param>
        /// <returns> Renvois le vertex créé ou celui trouvé dans le dictionnaire de vertex "_vertices". </returns>
        private Vertex GetVertex(int id, double lon, double lat)
        {
            if (VertexExist(id))
            {
                return GetVertex(id);
            }
            else
            {   // Ajoute le vertex créé avant de le renvoyer.
                AddVertex(new Vertex(id, lon, lat));
                return GetVertex(id);
            }
        }
        /// <summary>
        ///     Fonction qui se charge de créer le graphe en ajoutant les vertices et les arêtes qui le constituera
        ///     dans leur dictionnaire respectif. Le graphe bidirectionnelle est lui aussi créé ici et 
        ///     pour son implémentation, les arêtes créées seront ajoutées à chaque vertex dans une liste
        ///     d'arêtes "reverse". 
        ///     Pour parcourir le graphe dans le sens inverse, il suffira d'accéder aux arêtes "reverse" qui
        ///     mène vers le noeud précédents.
        ///     Cette fonction prendra les lignes du CSV une par une pour pouvoir créer les vertices et arêtes.
        /// </summary>
        /// <param name="filePath"> Chemin d'accès à partir d'où le fichier CSV contenant les informations sera pris. </param>
        private void _CreateGraph(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {   // Permet d'ajouter des champs null car certaines valeurs dans la base de données le sont.
                csv.Context.TypeConverterOptionsCache.GetOptions<double?>().NullValues.Add("NULL"); 
                foreach (CSVwayData way in csv.GetRecords<CSVwayData>())
                {   // Créer ou récupère les vertices et arêtes à partir des données récupérées de la ligne du fichier CSV.
                    Vertex sourceVertex = GetVertex(way.source, way.x1, way.y1);
                    Vertex targetVertex = GetVertex(way.target, way.x2, way.y2);
                    Edge edge = new Edge(way.length,
                                         way.cost,
                                         way.maxspeed_forward,
                                         way.maxspeed_backward,
                                         way.osm_id,
                                         way.tag_id,
                                         way.one_way); // way.cost_s on vérifie que le champ n'est pas null. S'il l'est, mieux vaut l'ignorer.
                    // Sauvegarde l'arête dans le dictinnaire d'arêtes "_edges" pour pouvoir modifier leur coût.
                    _SaveEdge(edge);
                    //------------------------------- Partie gérant les arêtes correspondant à une route n'ayant qu'un seul sens (one way). 
                    // Ajout de l'arête créée en tant qu'arête sortante dans le vertex source (graphe normal).
                    sourceVertex.AddOutgoingEdge(edge);
                    // Ajout de l'arête créée en tant qu'arête sortante dans le vertex target (graphe inverse).
                    targetVertex.AddReverseOutgoingEdge(edge);
                    // Liaison des arêtes au noeuds qu'elle uni.
                    edge.sourceVertex = sourceVertex;
                    edge.targetVertex = targetVertex;
                    //------------------------------- Partie gérant les arêtes correspondant à une route ayant deux sens de circulation. 
                    if (way.one_way == 2 || way.one_way == 0)
                    {   // Même principe que pour la création de l'arête dans le premier sens mais en lui affectant les valeurs du sens inverse (reverse_cost...).
                        edge = new Edge(way.length,
                                        way.reverse_cost,
                                        way.maxspeed_forward,
                                        way.maxspeed_backward,
                                        way.osm_id,
                                        way.tag_id,
                                        way.one_way);
                        // Même Principe que pour la partie ne gérant qu'un seul sens.
                        targetVertex.AddOutgoingEdge(edge);
                        sourceVertex.AddReverseOutgoingEdge(edge);
                        edge.sourceVertex = targetVertex;
                        edge.targetVertex = sourceVertex;
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
            if (_edges.ContainsKey(edge.osmID))
            {
                _edges[edge.osmID].Add(edge);
            }
            else
            {
                _edges.Add(edge.osmID, new List<Edge>() { edge });
            }
        }
        /// <summary>
        ///     Fonction permettant de vérifier si un vertex existe ou pas dans le graphe.
        /// </summary>
        /// <param name="id"> Identifiant du vertex dont on aimerais savoir s'il existe. </param>
        /// <returns> renvois true s'il existe, false sinon. </returns>
        public bool VertexExist(int id)
        {
            return _vertices.ContainsKey(id);
        }
        /// <summary>
        ///     Fonction permettant d'ajouter un vertex dans le dictionnaire des vertices du graphe.
        /// </summary>
        /// <param name="vertex"> Vertex à ajouter au dictionnaire. </param>
        /// <returns> Renvois true si le vertex a été ajouté, false sinon. </returns>
        public bool AddVertex(Vertex vertex)
        {   // Permet d'éviter d'ajouter 2 fois la même key et d'avoir une exception.
            if (!VertexExist(vertex.id))
            {
                _vertices.Add(vertex.id, vertex);
                return true;
            }
            return false;
        }
        /// <summary>
        ///     Fonction permettant d'obtenir un vertex contenu dans le graphe.
        /// </summary>
        /// <param name="vertexID"> Identifiant du vertex à trouver. </param>
        /// <returns> Retourne le vertex correspondant à l'identifiant s'il existe, sinon renvois une exception. </returns>
        public Vertex GetVertex(int vertexID)
        {
            try
            {
                return _vertices[vertexID];
            }
            catch (KeyNotFoundException)
            {
                throw new InvalidOperationException(Messages.VertexDontExist);
            }
        }
        /// <summary>
        ///     Fonction permettant de récupérer les arêtes du vertex correspondant à un idfentifiant donné
        ///     pour le premier sens (forward).
        ///     Elle permet aussi de vérifier si un noeud a déjà été visité pour ne pas retourner
        ///     des noeuds inutiles.
        /// </summary>
        /// <param name="vertexID"> Identifiant du vertex à partir duquel on veut récupérer les arêtes. </param>
        /// <param name="visitId"> Identifiant correspondant au numéro d'appel de l'algorithme. </param>
        /// <returns> Renvois les arêtes les une après les autres si elles n'ont pas déjà été visitées. </returns>
        public IEnumerable<Edge> GetNextEdges(int vertexID, int visitId)
        {
            foreach (Edge edge in GetVertex(vertexID).outgoingEdges)
            {   // Vérifie que l'arêtes n'a pas déjà été visitée  et vérifie que sont coût n'est pas inférieur car
                //  pour l'algorithme de Dijkstra il faut uniquement des valeurs > 0.
                if (edge.targetVertex.lastVisitForward != visitId && edge.cost >= 0)
                {
                    yield return edge;
                }
            }
        }
        /// <summary>
        ///     Fonction identique à "GetNextEdges" mais pour les arêtes consitutant la recherche inverse (backard).
        /// </summary>
        /// <param name="vertexID"> Identifiant du vertex à partir duquel on veut récupérer les arêtes. </param>
        /// <param name="visitId"> Identifiant correspondant au numéro d'appel de l'algorithme.</param>
        /// <returns> Renvois les arêtes de la recherche inverse (backward) les une après les autres si elles n'ont pas déjà été visitées.</returns>
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
        /// <summary>
        ///     Fonction permettant de récupérer les arêtes liées à un identifiant OSM.
        /// </summary>
        /// <param name="osmID"> Identifiant OSM des arêtes qu'on voudrait récupérer. </param>
        /// <returns> Renvois la liste d'arêtes lié à l'identifiant OSM passé en paramètre. </returns>
        public List<Edge> GetEdges(int osmID)
        {
            return _edges[osmID];
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
            _edges[osmId].ForEach(delegate(Edge edge) {
                edge.cost *= multiplier;
            });
        }
    }

    //--------------------------------------------------------------------------------------------------------

    public class Vertex
    {
        // Identifiant OSM du vertex (dans ce cas il s'agit de la valeur "source" tirée de la base de données).
        public int id { get; private set; }
        // Liste des arêtes du vertex qui vont vers un des autres vertces suivants (recherche forward).
        public List<Edge> outgoingEdges { get; private set; } = new List<Edge>();
        // Liste des arêtes du vertex qui vont vers un des autres vertces précédents (recherche backward).
        public List<Edge> reverseOutgoingEdges { get; private set; } = new List<Edge>();
        // Latitude du vertex.
        public double latitude { get; private set; }
        // Longitude du vertex.
        public double longitude { get; private set; }
        // Valeur de l'identifiant de la dernière fois que le vertex a été visité pour la recherche forward.
        public int lastVisitForward = 0;
        // Valeur de l'identifiant de la dernière fois que le vertex a été visité pour la recherche backward.
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
                    "\n" +
                    "\n - vertex source id : " + sourceVertex.id +
                    "\n - vertex target id : " + targetVertex.id +
                    "\n - cost : " + cost +
                    "\n - osm Id : " + osmID +
                    "\n";
        }
    }
    /// <summary>
    ///     Classe se chargeant de récupérer les valeurs contenues dans un fichier CSV pour pouvoir les utiliser lors de la création du graphe dans lafonction "_createGraph".
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
        public int osm_id { get; set; }
        [Index(17)]
        public int tag_id { get; set; }
    }
}