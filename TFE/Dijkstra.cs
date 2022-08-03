using System;
using System.Collections.Generic;
using Priority_Queue;

namespace TFE
{
    public class Dijkstra
    {
       // private FastPriorityQueue<State> _queue;
        private PriorityQueue<State, double> _queue;
        private Graph _graph;
        private int _priorityQueueMaxCapacity;
        public int lastVisitID = 0;
        public int totalNumberOfnodes;
        public int tookNodeNumber;

        public Dijkstra(Graph graph, int ppriorityQueueMaxCapacity = 4000)
        {
            _graph = graph;
            _priorityQueueMaxCapacity = ppriorityQueueMaxCapacity;
            //_queue = new FastPriorityQueue<State>(_priorityQueueMaxCapacity);
            _queue = new PriorityQueue<State, double>(_priorityQueueMaxCapacity);
        }

        private CostWithNode _PopHeadPriorityQueue()
        {
            State s = _queue.Dequeue();
            return new CostWithNode(s.totalCost, s);
        }
        private void _AddPriotiyQueueNode(double cost, State state)
        {
            totalNumberOfnodes++;
            if (_queue.Count >= 12000) throw new ArgumentOutOfRangeException(Messages.MaxPQcapacity);
            if (_queue.Count >= _priorityQueueMaxCapacity - 1)
            {
                Console.WriteLine(Messages.MaxPQcapacity);
                throw new ArgumentOutOfRangeException(Messages.MaxPQcapacity);
            }
            _queue.Enqueue(state, cost);
        }
        private void _ClearQueue()
        {
            _queue.Clear();
        }
        private bool _QueueIsEmpty()
        {
            return true ? _queue.Count == 0 : false;
        }
        private KeyValuePair<double, State> _NoPathFound(int sourceNodeID, int targetNodeID, string message)
        {
            Console.Write(message);
            Console.WriteLine($"Noeud source : {sourceNodeID}, noeud de destination : {targetNodeID}");
            return new KeyValuePair<double, State>();
        }
        private bool _VertexHasBeenVisited(Vertex vertex)
        {
            return vertex.lastVisit == lastVisitID;
        }
        /// <summary>
        ///     Fonction ayant pour objectif d'évaluer le coût d'un noeud à ajouter dans la PQ. 
        ///     Pour cela elle évalue les distances à vol d'oiseau entre différents noeuds.
        /// </summary>
        /// <param name="nextVertex"> Prend le prochain noeud du graphe, qui est celui où mène l'arête afin d'en récupérer les coordonnées géographiques. </param>
        /// <param name="finalVertex"> Prend le noeud cible à atteindre, le noeud final dont on veut connaître le plus court chemin. </param>
        /// <param name="totalCost"> Prend le coût du noeud de la priority queue actuel, et donc le coup total qui a été parcouru jusqu'ici. </param>
        /// <param name="costSToNextNode"> Coût de l'arête menant au prochain noeud (nextNode). </param>
        /// <param name="withCrowFlies"> Option permettant de décider si on prend le temps à vol d'oiseau ou pas. </param>
        /// <returns> 
        ///      Retourne l'évaluation sur le noeud actuel (sur lequel on est en train d'itérer et qui est normalement en tête de la PQ) pour en évaluer le coût par rapport au noeuds target. 
        ///      La valeur retournée, le coût, est une distance qui s'exprime en Km à vol d'oiseau entre le noeud courant fournit et le noeud qu'on veut atteindre au départ.
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

        public KeyValuePair<double, State> ComputeShortestPath(int sourceVertexID, int endVertexID, bool x = false)
        {
            // if (!_graph.VertexExist(sourceVertexID) || !_graph.VertexExist(endVertexID))
            //    return _NoPathFound(sourceVertexID, endVertexID, Messages.NodeDontExist); // arrête si le noeud de départ ou celui recherché n'existe pas.
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
                if (bestNode.state.getVertexID == endVertexID)
                { return new KeyValuePair<double, State>(bestNode.cost, bestNode.state); }

                if (_VertexHasBeenVisited(bestNode.state.vertex))
                { continue; }

                bestNode.state.vertex.lastVisit = lastVisitID;

                foreach (Edge nextEdge in _graph.GetNextEdges(bestNode.state.getVertexID, lastVisitID))
                {
                    cost = _CostEvaluation(nextEdge.targetVertex, endVertex, nextEdge.cost, false, bestNode.state);
                    _AddPriotiyQueueNode(cost,
                                        new State(cost,
                                                nextEdge.targetVertex,
                                                bestNode.state.costOnly + nextEdge.cost,
                                                bestNode.state)
                                        );
                }
            }
            return _NoPathFound(sourceVertexID, endVertexID, Messages.NoPathFound);
        }

        public KeyValuePair<double, State> ComputeShortestPathWithHeuristic(int sourceVertexID, int endVertexID, bool withHeuristic = false)
        {
            //  if (!_graph.VertexExist(sourceVertexID) || !_graph.VertexExist(endVertexID)) 
            //    return _NoPathFound(sourceVertexID, endVertexID, Messages.NodeDontExist); // arrête si le noeud de départ ou celui recherché n'existe pas.
            lastVisitID++;
            _ClearQueue();
            totalNumberOfnodes = 0;
            tookNodeNumber = 0;
            Vertex sourceVertex = _graph.GetVertex(sourceVertexID);
            Vertex endVertex = _graph.GetVertex(endVertexID);
            State initalState = new State(0, sourceVertex, 0);
            double cost = _CostEvaluation(sourceVertex, endVertex, 0, true, initalState);
            _AddPriotiyQueueNode(cost, initalState);
            Dictionary<int, State> bestPaths = new Dictionary<int, State>();

            while (!_QueueIsEmpty())
            {
                CostWithNode bestNode = _PopHeadPriorityQueue();
                tookNodeNumber++;
                if (bestNode.state.getVertexID == endVertexID)
                { return new KeyValuePair<double, State>(bestNode.cost, bestNode.state); }

                if (_VertexHasBeenVisited(bestNode.state.vertex))
                { continue; }

                else { bestPaths.Add(bestNode.state.getVertexID, bestNode.state); }
                bestNode.state.vertex.lastVisit = lastVisitID;

                foreach (Edge nextEdge in _graph.GetNextEdges(bestNode.state.getVertexID, lastVisitID))
                {
                    cost = _CostEvaluation(nextEdge.targetVertex, endVertex, nextEdge.cost, true, bestNode.state);
                    _AddPriotiyQueueNode(cost,
                                        new State(cost,
                                                nextEdge.targetVertex,
                                                bestNode.state.costOnly + nextEdge.cost,
                                                bestNode.state)
                                        );
                }
            }
            return _NoPathFound(sourceVertexID, endVertexID, Messages.NoPathFound);
        }
    }

    public class State : FastPriorityQueueNode
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
