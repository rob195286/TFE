using Priority_Queue; // https://github.com/BlueRaja/High-CarSpeed-Priority-Queue-for-C-Sharp/wiki/Using-the-FastPriorityQueue
using System;
using System.Collections.Generic;


namespace TFE
{
    public class Dijkstra
    {
        private FastPriorityQueue<PriorityQueueNode> _queue;
        private Graph _graph;
        private int _priorityQueueMaxCapacity;
        public int lastVisitID = 0;
        public int totalNumberOfnodes;
        public int tookNodeNumber;

        public Dijkstra(Graph graph, int ppriorityQueueMaxCapacity = 4000)
        {
            _graph = graph;
            _priorityQueueMaxCapacity = ppriorityQueueMaxCapacity;
            _queue = new FastPriorityQueue<PriorityQueueNode>(_priorityQueueMaxCapacity);
        }

        private CostWithNode _PopHeadPriorityQueue()
        {
            PriorityQueueNode s = _queue.Dequeue();
            return new CostWithNode(s.costS, s);
        }
        private void _AddPriotiyQueueNode(double cost, PriorityQueueNode state)
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
        private KeyValuePair<double, PriorityQueueNode> _NoPathFound(int sourceNodeID, int targetNodeID, string message)
        {
            Console.Write(message);
            Console.WriteLine($"Noeud source : {sourceNodeID}, noeud de destination : {targetNodeID}");
            return new KeyValuePair<double, PriorityQueueNode>();
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
        private double _CostEvaluation(GraphNode nextNode,
                                       GraphNode finalNode,
                                       double totalCost,
                                       double costSToNextNode, 
                                       bool withCrowFlies)
        {
            return totalCost + // Somme du coût des noeuds précédements visités, soit du chemin total.   
                costSToNextNode + // Le coût pour rejoindre le prochain noeud.
                ((withCrowFlies ? GeometricFunctions.TimeAsCrowFliesFromTo(nextNode, finalNode) : 0)*0/140) // l'ajout de l'évaluation de la distance entre le noeud courant et le noeud target
                ;
        }

        public KeyValuePair<double, PriorityQueueNode> ComputeShortestPath(int sourceNodeID, int targetNodeID, bool withCrowFliesOption = false)
        {
            if (!_graph.NodeExist(sourceNodeID) || !_graph.NodeExist(targetNodeID)) 
                return _NoPathFound(sourceNodeID, targetNodeID, Messages.NodeDontExist); // arrête si le noeud de départ ou celui recherché n'existe pas.
            lastVisitID++;
            _ClearQueue();
            totalNumberOfnodes = 0;
            _AddPriotiyQueueNode(0, new PriorityQueueNode(0, _graph.GetNode(sourceNodeID), 0, null));
            GraphNode finalNode = _graph.GetNode(targetNodeID);
            tookNodeNumber = 0;
            while (!_QueueIsEmpty())
            {
                CostWithNode bestNode = _PopHeadPriorityQueue();
                tookNodeNumber++;
                if (bestNode.priorityQueueNode.graphNode.id == targetNodeID)
                    return new KeyValuePair<double, PriorityQueueNode>(bestNode.cost, bestNode.priorityQueueNode);
                if (bestNode.priorityQueueNode.graphNode.VisitID == lastVisitID) 
                    continue;
                bestNode.priorityQueueNode.graphNode.VisitID = lastVisitID;                

                foreach (Edge nextEdge in _graph.GetNextEdges(bestNode.priorityQueueNode.graphNode.id, lastVisitID))
                {
                    _AddPriotiyQueueNode(_CostEvaluation(nextEdge.targetNode, finalNode, bestNode.priorityQueueNode.costS, nextEdge.costS, withCrowFliesOption),
                            new PriorityQueueNode(_CostEvaluation(nextEdge.targetNode, finalNode, bestNode.priorityQueueNode.costS, nextEdge.costS, withCrowFliesOption),
                                    nextEdge.targetNode,
                                    bestNode.priorityQueueNode.costSOnly + nextEdge.costS,
                                    bestNode.priorityQueueNode,
                                    nextEdge.roadName,
                                    nextEdge)
                    );
                }
            }
            return _NoPathFound(sourceNodeID, targetNodeID, Messages.NoPathFound);
        }
    }

    public class PriorityQueueNode : FastPriorityQueueNode
    {
        public double costS { get; } // cost en seconde + temps à vol d'soieau
        public GraphNode graphNode { get; }
        public PriorityQueueNode previousState { get; }
        public string roadName { get; }
        public Edge edgeToNode { get; }
        public double costSOnly { get; } // todo : à enlever -> on prend seulement les cost en seconde 


        public PriorityQueueNode(double pcost,
                                 GraphNode pnode,
                                 double pcostSOnly,
                                 PriorityQueueNode ppreviousState = null,
                                 string proadName = "",
                                 Edge pedge = null)
        {
            costS = pcost;
            graphNode = pnode;
            previousState = ppreviousState;
            roadName = proadName;
            edgeToNode = pedge;
            costSOnly = pcostSOnly;
        }
    }

    internal struct CostWithNode{
        public double cost { get; }
        public PriorityQueueNode priorityQueueNode { get; }

        public CostWithNode(double pcost, PriorityQueueNode ppriorityQueueNode)
        {
            cost = pcost;
            priorityQueueNode = ppriorityQueueNode;
        }
    }
}
