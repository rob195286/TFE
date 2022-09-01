using System;
using System.Collections.Generic;

namespace TFE
{
    public class Dijkstra
    {
        private PriorityQueue<State, double> _queue;
        private Graph _graph;
        // Capacité maximum de la PQ avant qu'elle ne se redimensionne. Si sa valeur est par exemple de 200 et qu'elle est dépassée, alors elle se redimensionnera. 
        private int _priorityQueueMaxCapacity;
        // Valeur de l'appel actuelle de l'algorithme. S'il est appelé 3x, alors la valeur s'incrémentera au fur et amesure à chaque appel pour atteindre 3. 
        public int lastVisitID = 0;
        // Représente le nombre total de noeuds ajoutés à la PQ.
        public int totalNumberOfnodes;
        // Représente le nombre total de noeuds parcouru et pris de la PQ.
        public int tookNodeNumber;

        public Dijkstra(Graph graph, int ppriorityQueueMaxCapacity = 4000)
        {
            _graph = graph;
            _priorityQueueMaxCapacity = ppriorityQueueMaxCapacity;
            _queue = new PriorityQueue<State, double>(_priorityQueueMaxCapacity);
        }

        /// <summary>
        ///     Fonction permettant d'extraire la valeur racine de la PQ.
        /// </summary>
        /// <returns> Retourne l'état (objet "state") et le coût associé à la valeure minimale de la PQ.</returns>
        private CostWithNode _PopHeadPriorityQueue()
        {
            State s = _queue.Dequeue();
            return new CostWithNode(s.totalCost, s);
        }
        /// <summary>
        ///     Ajoute un état (ojbet "state") d'une des PQ à un coût donné.
        /// </summary>
        /// <param name="cost"> Coût auxquel l'état doit être ajouté. Il correspond au coût du chemin pour arriver jusuq'au noeud qu'il contient. </param>
        /// <param name="state"> Etat qui doit être ajouté et qui sera le noeud de la PQ. </param>
        private void _AddPriotiyQueueNode(double cost, State state)
        {
            totalNumberOfnodes++;
            _queue.Enqueue(state, cost);
        }
        /// <summary>
        ///     Réinitialise les PQ pour qu'elles puissent être à nouveau utilisées lors du prochain appel de l'algorithme.
        /// </summary>
        private void _ClearQueue()
        {
            _queue.Clear();
        }
        /// <summary>
        ///     Fonction ayant pour objectif de vérifier si la PQ est vide. 
        ///     L'objectif est de savoir, via cette caractéristique, si l'algorithme a terminé d'explorer tous les vertices qu'il pouvait.
        /// </summary>
        /// <returns> Retourne un booléen indiquant si la PQ est vide ou non. La valeur est true si elle est vide, false sinon. </returns>
        private bool _QueueIsEmpty()
        {
            return true ? _queue.Count == 0 : false;
        }
        /// <summary>
        ///     Cette fonction à pour but de regrouper les conditions d'arrêt qui ne sont pas celle où l'algorithme trouve un plus court chemin.
        ///     Les moments où elles intervient sont celui ou un des noeuds d'entré n'existe pas dans le graphe ou pour vérifier si les PQ ne sont pas vide.
        /// </summary>
        /// <returns> Retourne une clé-valeur où le coût est null et le state un objet vide. </returns>
        private KeyValuePair<double, State> _NoPathFound()
        {
            return new KeyValuePair<double, State>();
        }
        /// <summary>
        ///     Fonction permettant de vérifier si un vertex a déjà été visité.
        /// </summary>
        /// <param name="vertex"> Vertex à vérifier.</param>
        /// <returns> Renvois true s'il a déjà été visité, false sinon.</returns>
        private bool _VertexHasBeenVisited(Vertex vertex)
        {
            return vertex.lastVisit == lastVisitID;
        }        
        /// <summary>
        ///     Fonction ayant pour objectif d'évaluer le coût d'un noeud à ajouter dans la PQ. 
        ///     Pour cela elle évalue les distances à vol d'oiseau entre différents noeuds.
        /// </summary>
        /// <param name="nextVertex"> Prend le prochain noeud du graphe, qui est celui où mène l'arête afin d'en récupérer les coordonnées géographiques.</param>
        /// <param name="finalVertex"> Prend le noeud cible à atteindre, le noeud final dont on veut connaître le plus court chemin.</param>
        /// <param name="costToNextVertex"> Coût de l'arête qui mène au prochain vertex.</param>
        /// <param name="withHeuristic"> Permet de prendre en compte l'heuristique ou non en fonction d'un Dijkstra classique ou avec heuristique.</param>
        /// <param name="currentState"></param>
        /// <returns>
        ///     Retourne l'évaluation sur le noeud actuel (sur lequel on est en train d'itérer et qui est normalement en tête de la PQ) pour en évaluer le coût par rapport au noeuds final. 
        ///     La valeur retournée, le coût, est la somme entre le coût ur arriver jusqu'à ce noeud plus un copût équivalent tiré d'une distance en Km à vol d'oiseau entre le noeud courant 
        ///         fournit et le noeud qu'on veut atteindre au départ.
        /// </returns>
        private double _CostEvaluation(Vertex nextVertex,
                                        Vertex finalVertex,
                                        double costToNextVertex,
                                        bool withHeuristic,
                                        State currentState)
        {
            // Somme du coût des noeuds précédements visités, soit du chemin total + le coût pour rejoindre le prochain noeud,
            //  ce qui est l'équivaletn de la fonction G.
            return currentState.costOnly + costToNextVertex +
                    // Ajout de l'évaluation de la distance entre le noeud courant et le noeud target, équivalent de la fonction H.
                    (withHeuristic ? GeometricFunctions.EuclideanDistanceCostFromTo(nextVertex, finalVertex) : 0);
        }
        /// <summary>
        ///     Fonction implémentant l'algorithme de Dijkstra simple.
        /// </summary>
        /// <param name="sourceVertexID"> Identifiant du vertex source, soit celui d'où l'on part et à partir duquel on veut trouver un chemin. L'id est la valeur "source" venant du format OSM. </param>
        /// <param name="endVertexID"> Identifiant du vertex de destination, soit celui vers lequel on veut trouver un chemin. L'id est aussi la valeur "source" venant du format OSM. </param>
        /// <returns> 
        ///     Retourne le dernier état (l'objet state) qui constitue le chemin et à partir duquel, en parcourant les variables "previousState" de chacun d'eux, il est possible de trouver l'ensmble des vertices. 
        ///     Le résultat est une clé valeur avec en premier lieu le coût total pour atteindre le dernier état (et donc atteindre le vertex de destination) et en second lieu est l'état y correspondant.
        ///     Si le chemin n'est pas trouvé alors une clé valeur de zéro et un objet vide sera renvoyé.
        /// </returns>
        public KeyValuePair<double, State> ComputeShortestPath(int sourceVertexID, int endVertexID)
        {
             if (!_graph.VertexExist(sourceVertexID) || !_graph.VertexExist(endVertexID))
                return _NoPathFound(); // arrête si le noeud de départ ou celui recherché n'existe pas.
            lastVisitID++;
            _ClearQueue();
            totalNumberOfnodes = 0;
            tookNodeNumber = 0;
            Vertex sourceVertex = _graph.GetVertex(sourceVertexID);
            Vertex endVertex = _graph.GetVertex(endVertexID);
            double cost;
            _AddPriotiyQueueNode(0, new State(0, sourceVertex, 0));

            while (!_QueueIsEmpty())
            {
                CostWithNode bestNode = _PopHeadPriorityQueue();
                tookNodeNumber++;
                if (bestNode.state.getVertexID == endVertexID) // Condition d'arrêt de l'algorithme.
                { return new KeyValuePair<double, State>(bestNode.cost, bestNode.state); }

                if (_VertexHasBeenVisited(bestNode.state.vertex)) // Vérification que le noeud n'ai pas déjà été visité. Si tel est le cas, on le passe.
                { continue; }
                bestNode.state.vertex.lastVisit = lastVisitID; // Actualisation de la dernière visite du noeud.

                foreach (Edge nextEdge in _graph.GetNextEdges(bestNode.state.getVertexID, lastVisitID))
                {
                    if (_VertexHasBeenVisited(nextEdge.targetVertex)) // Vérification que le noeud n'ai pas déjà été visité. Si tel est le cas, on le passe.
                    { continue; }
                    cost = _CostEvaluation(nextEdge.targetVertex, endVertex, nextEdge.cost, false, bestNode.state);
                    _AddPriotiyQueueNode(cost,
                                        new State(cost,
                                                nextEdge.targetVertex,
                                                bestNode.state.costOnly + nextEdge.cost,
                                                bestNode.state)
                                        );
                }
            }
            return _NoPathFound();
        }
        /// <summary>
        ///     Fonction implémentant l'algorithme de Dijkstra avec heuristique.
        /// </summary>
        /// <param name="sourceVertexID"> Pareil que pour le Dijkstra simple. </param>
        /// <param name="endVertexID"> Pareil que pour le Dijkstra simple. </param>
        /// <returns> 
        ///     Pareil que pour le Dijkstra simple.
        /// </returns>
        public KeyValuePair<double, State> ComputeShortestPathWithHeuristic(int sourceVertexID, int endVertexID)
        {
              if (!_graph.VertexExist(sourceVertexID) || !_graph.VertexExist(endVertexID)) 
                return _NoPathFound(); // arrête si le noeud de départ ou celui recherché n'existe pas.
            lastVisitID++;
            _ClearQueue();
            totalNumberOfnodes = 0;
            tookNodeNumber = 0;
            Vertex sourceVertex = _graph.GetVertex(sourceVertexID);
            Vertex endVertex = _graph.GetVertex(endVertexID);
            double cost = _CostEvaluation(sourceVertex, endVertex, 0, true, new State(0, sourceVertex, 0));
            _AddPriotiyQueueNode(cost, new State(0, sourceVertex, 0));
            Dictionary<int, double> bestCosts = new Dictionary<int, double>() { }; // Regroupe l'ensemble des noeuds déjà visités avec leur coût.

            while (!_QueueIsEmpty())
            {
                CostWithNode bestNode = _PopHeadPriorityQueue();
                tookNodeNumber++;
                if (bestNode.state.getVertexID == endVertexID) // Condition d'arrêt de l'algorithme.
                { return new KeyValuePair<double, State>(bestNode.cost, bestNode.state); }

                else if (_VertexHasBeenVisited(bestNode.state.vertex)) // Vérification d ufait que le noeud ai déjà été visité.
                {
                    if (bestNode.state.costOnly >= bestCosts[bestNode.state.getVertexID]) // S'il a été visité et que son coût actuel est plus grand, on l'ignore.
                    { continue; }
                    bestCosts[bestNode.state.getVertexID] = bestNode.state.costOnly; // Si son coût est plus faible on actualise le dernier plus court chemin trouvé.
                }
                else { bestCosts.Add(bestNode.state.getVertexID, bestNode.state.costOnly); } // S'il n'a jamais été parcouru, on l'ajoute.
                bestNode.state.vertex.lastVisit = lastVisitID;

                foreach (Edge nextEdge in _graph.GetNextEdges(bestNode.state.getVertexID, lastVisitID))
                {   // Pareil que plus haut, on vérifie si le prochain noeud à déjà été visité et s'il a un coût plus grand on continue.
                    if(_VertexHasBeenVisited(nextEdge.targetVertex) && (bestNode.state.costOnly + nextEdge.cost) > bestCosts[nextEdge.targetVertex.id])
                    { continue; }
                    cost = _CostEvaluation(nextEdge.targetVertex, endVertex, nextEdge.cost, true, bestNode.state);
                    _AddPriotiyQueueNode(cost,
                                        new State(cost,
                                                nextEdge.targetVertex,
                                                bestNode.state.costOnly + nextEdge.cost,
                                                bestNode.state)
                                        );
                }
            }
            return _NoPathFound();
        }
    }

    public class State
    {
        // Coût total (somme du coût des arêtes) pour atteindre le vertex qui sera ajouté à cet état + l'heuristique.
        public double totalCost { get; }
        public Vertex vertex { get; }
        // Etat précédant celui-ci qui permet de retrouvé la succession de vertices qui constituent le chemin. 
        public State previousState { get; set; }
        // Récupération de l'identifiant OSM du vertex.
        public int getVertexID
        {
            get { return vertex.id; }
        }
        // Coût total pour arriver au noeud contenu sans l'évaluation heuristique heuristique.
        public double costOnly { get; }


        public State(double ptotalCost,
                    Vertex pvertex,
                    double pcostOnly,
                    State ppreviousState = null)
        {
            totalCost = ptotalCost;
            vertex = pvertex;
            previousState = ppreviousState;
            costOnly = pcostOnly;
        }
    }

    internal struct CostWithNode
    {
        public double cost { get; }
        public State state { get; }

        public CostWithNode(double pcost, State pstate)
        {
            cost = pcost;
            state = pstate;
        }
    }
}
