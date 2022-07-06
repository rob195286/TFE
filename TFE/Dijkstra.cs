using Priority_Queue; // https://github.com/BlueRaja/High-CarSpeed-Priority-Queue-for-C-Sharp/wiki/Using-the-FastPriorityQueue
using System;
using System.Collections.Generic;


namespace TFE
{
    public class Dijkstra
    {
        private FastPriorityQueue<State> _queue;
        private Graph _graph;
        private int _priorityQueueMaxCapacity;
        public int lastVisitID = 0;
        public int totalNumberOfnodes;
        public int tookNodeNumber;

        public Dijkstra(Graph graph, int ppriorityQueueMaxCapacity = 2*4000)
        {
            _graph = graph;
            _priorityQueueMaxCapacity = ppriorityQueueMaxCapacity;
            _queue = new FastPriorityQueue<State>(_priorityQueueMaxCapacity);
        }

        private CostWithNode _PopHeadPriorityQueue()
        {
            State s = _queue.Dequeue();
            return new CostWithNode(s.totalCost, s);
        }
        private void _AddPriotiyQueueNode(double cost, State state)
        {
            totalNumberOfnodes++;
            if(_queue.Count >= 12000) throw new ArgumentOutOfRangeException(Messages.MaxPQcapacity);
            if (_queue.Count >= _priorityQueueMaxCapacity-1)
            {
                Console.WriteLine(Messages.MaxPQcapacity);
                throw new ArgumentOutOfRangeException(Messages.MaxPQcapacity);
            }
            _queue.Enqueue(state, (float)cost);  
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
        private double _Heuristics(Vertex nextNode, Vertex finalNode, double costToNextNode)
        {
            //  1.7111
            return GeometricFunctions.EuclideanDistanceFromToInSecond(nextNode, finalNode);
        }
        /// <summary>
        ///     Fonction ayant pour objectif d'évaluer le coût d'un noeud à ajouter dans la PQ. 
        ///     Pour cela elle évalue les distances à vol d'oiseau entre différents noeuds.
        /// </summary>
        /// <param name="nextNode"> Prend le prochain noeud du graphe, qui est celui où mène l'arête afin d'en récupérer les coordonnées géographiques. </param>
        /// <param name="finalNode"> Prend le noeud cible à atteindre, le noeud final dont on veut connaître le plus court chemin. </param>
        /// <param name="totalCost"> Prend le coût du noeud de la priority queue actuel, et donc le coup total qui a été parcouru jusqu'ici. </param>
        /// <param name="costSToNextNode"> Coût de l'arête menant au prochain noeud (nextNode). </param>
        /// <param name="withCrowFlies"> Option permettant de décider si on prend le temps à vol d'oiseau ou pas. </param>
        /// <returns> 
        ///      Retourne l'évaluation sur le noeud actuel (sur lequel on est en train d'itérer et qui est normalement en tête de la PQ) pour en évaluer le coût par rapport au noeuds target. 
        ///      La valeur retournée, le coût, est une distance qui s'exprime en Km à vol d'oiseau entre le noeud courant fournit et le noeud qu'on veut atteindre au départ.
        /// </returns>
        private double _CostEvaluation(Vertex nextNode,
                                       Vertex finalNode,
                                       double totalCost,
                                       double costToNextNode, 
                                       bool withHeuristic,
                                       int heuristicsWeight = 25)
        {
            double g = totalCost + costToNextNode;
            double h = withHeuristic ? _Heuristics(nextNode, finalNode, costToNextNode) : 0;
            return totalCost + // Somme du coût des noeuds précédements visités, soit du chemin total.   
                costToNextNode + // Le coût pour rejoindre le prochain noeud.
                (withHeuristic ? _Heuristics(nextNode, finalNode, costToNextNode) : 0) //* heuristicsWeight
                //((withHeuristic ? GeometricFunctions.EuclideanDistanceFromToInSecond(nextNode, finalNode) : 0)*0/1221.72) // l'ajout de l'évaluation de la distance entre le noeud courant et le noeud target
                ;
        }

        public KeyValuePair<double, State> ComputeShortestPath(int sourceNodeID, int endNodeID, bool withHeuristic = false)
        {
            if (!_graph.NodeExist(sourceNodeID) || !_graph.NodeExist(endNodeID)) 
                return _NoPathFound(sourceNodeID, endNodeID, Messages.NodeDontExist); // arrête si le noeud de départ ou celui recherché n'existe pas.
            lastVisitID++;
            _ClearQueue();
            totalNumberOfnodes = 0;
            _AddPriotiyQueueNode(0, new State(0, _graph.GetNode(sourceNodeID), 0, null));
            Vertex endNode = _graph.GetNode(endNodeID);
            tookNodeNumber = 0;
            while (!_QueueIsEmpty())
            {
                CostWithNode bestNode = _PopHeadPriorityQueue();
                tookNodeNumber++;
                if (bestNode.state.getVertexID == endNodeID)
                    return new KeyValuePair<double, State>(bestNode.cost, bestNode.state);
                if (bestNode.state.vertex.lastVisit == lastVisitID) 
                    continue;
                bestNode.state.vertex.lastVisit = lastVisitID;                

                foreach (Edge nextEdge in _graph.GetNextEdges(bestNode.state.getVertexID, lastVisitID))
                {
                    double cost = _CostEvaluation(nextEdge.targetVertex, endNode, bestNode.cost, nextEdge.cost, withHeuristic);
                    _AddPriotiyQueueNode(cost,
                                        new State(cost,
                                                nextEdge.targetVertex,
                                                //bestNode.state.costSOnly + nextEdge.cost,
                                                0,
                                                bestNode.state)
                    );
                }
            }
            return _NoPathFound(sourceNodeID, endNodeID, Messages.NoPathFound);
        }
    }

    public class State : FastPriorityQueueNode
    {
        // Coût total (smme du cpupt des arêtes) pour atteindre le vertex qui sera ajouté à cet état.
        public double totalCost { get; }
        public Vertex vertex { get; }
        // Etat précédant celui-ci qui permet de retrouvé la succession de vertices qui constituent le chemin. 
        public State previousState { get; set; }
        // Récupération de l'identifiant OSM du vertex.
        public int getVertexID
        {
            get { return vertex.id; }
        }
        public double costSOnly { get; } // todo : à enlever -> on prend seulement les totalCost en seconde 


        public State(double ptotalCost,
                    Vertex pvertex,
                    double pcostSOnly,
                    State ppreviousState = null)
        {
            totalCost = ptotalCost;
            vertex = pvertex;
            previousState = ppreviousState;
            costSOnly = pcostSOnly;
        }
    }

    internal struct CostWithNode{
        public double cost { get; }
        public State state { get; }

        public CostWithNode(double pcost, State pstate)
        {
            cost = pcost;
            state = pstate;
        }
    }
}
