using Priority_Queue; // fast-priority queue
using System;
using System.Collections.Generic;

namespace TFE
{
    public class Dijkstra
    {
        private FastPriorityQueue<State> _queue; // fast priority queue
        private Graph _graph;
        private int _priorityQueueMaxCapacity;
        public int lastVisitID = 0;
        public int totalNumberOfnodes;
        public int tookNodeNumber;

        public Dijkstra(Graph graph, int ppriorityQueueMaxCapacity = 4000)
        {
            _graph = graph;
            _priorityQueueMaxCapacity = ppriorityQueueMaxCapacity;
            _queue = new FastPriorityQueue<State>(_priorityQueueMaxCapacity);
        }

        private CostWithNode _PopHeadPriorityQueue()
        {
            State s = _queue.Dequeue();
            return new CostWithNode(s.totalCostS, s);
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
            throw new InvalidOperationException("problèmmeeeeeeeeeeeeeeeeeeeee");
            return new KeyValuePair<double, State>();
        }
        private double _CostEvaluation(double totalCostS,
                                       double costSToNextNode)
        {
            return totalCostS + // Somme du coût des noeuds précédements visités, soit du chemin total.   
                   costSToNextNode // Le coût pour rejoindre le prochain noeud.
                   ;
        }

        public KeyValuePair<double, State> ComputeShortestPath(int sourceNodeID, int endNodeID)
        {
            if (!_graph.NodeExist(sourceNodeID) || !_graph.NodeExist(endNodeID)) return _NoPathFound(sourceNodeID, endNodeID, Messages.NodeDontExist); // arrête si le noeud de départ ou celui recherché n'existe pas
            lastVisitID++;
            _ClearQueue();
            _AddPriotiyQueueNode(0, new State(0, _graph.GetNode(sourceNodeID)));
            while (!_QueueIsEmpty())
            {
                CostWithNode bestNode = _PopHeadPriorityQueue();
                tookNodeNumber++;
                if (bestNode.State.node.id == endNodeID)
                    return new KeyValuePair<double, State>(bestNode.cost, bestNode.State);
                if (bestNode.State.node.visitID == lastVisitID)
                    continue;
                bestNode.State.node.visitID = lastVisitID;

                foreach (Edge nextEdge in _graph.GetNextEdges(bestNode.State.node.id, lastVisitID))
                {
                    _AddPriotiyQueueNode(_CostEvaluation(bestNode.State.totalCostS, nextEdge.costS),
                            new State(_CostEvaluation(bestNode.State.totalCostS, nextEdge.costS),
                                    nextEdge.targetNode,
                                    bestNode.State)
                    );
                }
            }
            return _NoPathFound(sourceNodeID, endNodeID, Messages.NoPathFound);
        }
    }
    public class State : FastPriorityQueueNode
    {
        public double totalCostS { get; } 
        public Node node { get; }
        public State previousState { get; }
        public string roadName { get; }
        public Edge edgeToNode { get; }


        public State(double ptotalCost,
                    Node pnode,
                    State ppreviousState = null
                    )
        {
            totalCostS = ptotalCost;
            node = pnode;
            previousState = ppreviousState;
        }
    }

    internal struct CostWithNode
    {
        public double cost { get; }
        public State State { get; }

        public CostWithNode(double pcost, State pstate)
        {
            cost = pcost;
            State = pstate;
        }
    }

}
