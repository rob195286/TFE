﻿using Priority_Queue; // fast-priority queue https://github.com/BlueRaja/High-CarSpeed-Priority-Queue-for-C-Sharp/wiki/Using-the-FastPriorityQueue
using System;
using System.Collections.Generic;


namespace TFE
{
    public class Dijkstra
    {
        private FastPriorityQueue<PriorityQueueNode> _queue; // fast priority queue
        private Graph _graph;
        private int _priorityQueueMaxCapacity;
        public int lastVisitID = 0;
        public int numberOfnodes = 0;
        public int tookNodeNumber = 0;

        public Dijkstra(Graph graph, int ppriorityQueueMaxCapacity = 3000)
        {
            _graph = graph;
            _priorityQueueMaxCapacity = ppriorityQueueMaxCapacity;
            _queue = new FastPriorityQueue<PriorityQueueNode>(_priorityQueueMaxCapacity);
        }

        private CostNState _PopHeadPriorityQueue()
        {
            PriorityQueueNode s = _queue.Dequeue();
            return new CostNState(s.costS, s);
        }

        private void _AddPriotiyQueueNode(double cost, PriorityQueueNode state)
        {
            if (_queue.Count >= _priorityQueueMaxCapacity)
                Console.WriteLine("attention, capacité max atteinte");
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
        private KeyValuePair<double, PriorityQueueNode> _NoPathFound()
        {
            return new KeyValuePair<double, PriorityQueueNode>();
        }
        /// <summary>
        ///     Fonction ayant pour objectif d'évaluer le coût d'un noeud à ajouter dans la PQ. Pour cela elle évalue les distances à vol d'oiseau entre les différents noeuds.
        /// </summary>
        /// <param name="state"> Prend le priorityQueueNode actuel afin de pouvoir en extraire le coût. </param>
        /// <param name="currentNode"> Prend le noeud courant, celui ayant le coût le plus faible dans la PQ. </param>
        /// <param name="targetNode"> Prend le noeud cible à atteindre, le noeud final dont on veut connaître le plus court chemin. </param>
        /// <returns> 
        ///     Retourne l'évaluation sur le noeud actuel (sur lequel on est en train d'itérer et qui est normalement en tête de la PQ) pour en évaluer le coût par rapport au noeuds target. 
        ///     La valeur retournée, le coût, est une distance qui s'exprime en Km à vol d'oiseau entre le noeud courant fournit et le noeud qu'on veut atteindre au départ.
        /// </returns>
        private double _CostEvaluation(GraphNode currentNode, GraphNode finalNode, PriorityQueueNode priorityQueueNode, double? costSToNextNode)
        {
            // Somme du coût des noeuds    |Le coût en second |
            // précédements visités,       |pour rejoindre le |
            // soit du chemin total        +  prochain noeud  + l'ajout de l'évaluation de la distance entre le noeud courant et le noeud target              
            return priorityQueueNode.costS + costSToNextNode ?? 9999; // + GeometricFunctions.TimeAsCrowFlies(currentNode, finalNode);
        }
        
        public KeyValuePair<double, PriorityQueueNode> ComputeShortestPath_SAV(int sourceNodeID, int targetNodeID)
        {
            /*
            if (!_graph.NodeExist(sourceNodeID) || !_graph.NodeExist(targetNodeID)) return NoPathFound(); // arrête si le noeud de départ ou celui recherché n'existe pas
            lastVisitID++;
            ClearQueue();
            AddNode(0, new PriorityQueueNode(0, _graph.GetNode(sourceNodeID)));
            while (!QueueIsEmpty())
            {
                var bestNode = PopNextItem();
                if (bestNode.Value.graphNode.VisitID == lastVisitID) continue;
                if (bestNode.Value.graphNode.id != targetNodeID) bestNode.Value.graphNode.VisitID = lastVisitID;
                if (bestNode.Value.graphNode.id == targetNodeID) return new KeyValuePair<double, PriorityQueueNode>(bestNode.Key, bestNode.Value);
                foreach (Edge nextEdge in _graph.GetNextEdges(bestNode.Value.graphNode.id, lastVisitID, targetNodeID))
                {
                    AddNode(bestNode.Key + nextEdge.costS,
                            new PriorityQueueNode(bestNode.Key + nextEdge.costS,
                                    nextEdge.finalNode,
                                    bestNode.Value,
                                    nextEdge.roadName,
                                    nextEdge)
                    );
                }
            }
            return NoPathFound();
            */
           return new KeyValuePair<double, PriorityQueueNode>();
        }
        public KeyValuePair<double, PriorityQueueNode> ComputeShortestPath(int sourceNodeID, int targetNodeID)
        {
            if (!_graph.NodeExist(sourceNodeID) || !_graph.NodeExist(targetNodeID)) 
                return _NoPathFound(); // arrête si le noeud de départ ou celui recherché n'existe pas
            lastVisitID++;
            _ClearQueue();
            _AddPriotiyQueueNode(0, new PriorityQueueNode(0, _graph.GetNode(sourceNodeID), null));
            GraphNode finalNode = _graph.GetNode(targetNodeID);
            while (!_QueueIsEmpty())
            {
                CostNState bestNode = _PopHeadPriorityQueue();
                if (bestNode.priorityQueueNode.graphNode.VisitID == lastVisitID) 
                    continue;
                if (bestNode.priorityQueueNode.graphNode.id != targetNodeID) 
                    bestNode.priorityQueueNode.graphNode.VisitID = lastVisitID;
                if (bestNode.priorityQueueNode.graphNode.id == targetNodeID) 
                    return new KeyValuePair<double, PriorityQueueNode>(bestNode.cost, bestNode.priorityQueueNode);
                tookNodeNumber++;
                foreach (Edge nextEdge in _graph.GetNextEdges(bestNode.priorityQueueNode.graphNode.id, lastVisitID, targetNodeID))
                {
                    _AddPriotiyQueueNode(_CostEvaluation(nextEdge.targetNode, finalNode, bestNode.priorityQueueNode, nextEdge.costS),
                            new PriorityQueueNode(_CostEvaluation(nextEdge.targetNode, finalNode, bestNode.priorityQueueNode, nextEdge.costS),
                                    nextEdge.targetNode,
                                    bestNode.priorityQueueNode,
                                    nextEdge.roadName,
                                    nextEdge)
                    );
                }
            }
            return _NoPathFound();
        }
    }

    public class PriorityQueueNode : FastPriorityQueueNode
    {
        public double costS { get; }
        public GraphNode graphNode { get; }
        public PriorityQueueNode previousState { get; }
        public string roadName { get; }
        public Edge edgeToNode { get; }


        public PriorityQueueNode(double pcost,
                                 GraphNode pnode,
                                 PriorityQueueNode ppreviousState = null,
                                 string proadName = "",
                                 Edge pedge = null)
        {
            costS = pcost;
            graphNode = pnode;
            previousState = ppreviousState;
            roadName = proadName;
            edgeToNode = pedge;
        }
    }

    internal struct CostNState{
        public double cost { get; }
        public PriorityQueueNode priorityQueueNode { get; }

        public CostNState(double pcost, PriorityQueueNode ppriorityQueueNode)
        {
            cost = pcost;
            priorityQueueNode = ppriorityQueueNode;
        }
    }
}