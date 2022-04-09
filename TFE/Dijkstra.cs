using Priority_Queue; // fast-priority queue
using System;
using System.Collections.Generic;

namespace TFE
{
    public class Dijkstra
    {
        private FastPriorityQueue<State> _queue; // fast priority queue
        private FastPriorityQueue<State> _queueB; // fast Backward priority queue
        private Graph _graph;
        private int _priorityQueueMaxCapacity;
        public int lastVisitID = 0;
        public int totalNumberOfnodes;
        public int tookNodeNumber;

        public Dijkstra(Graph graph, int ppriorityQueueMaxCapacity = 4000/2)
        {
            _graph = graph;
            _priorityQueueMaxCapacity = ppriorityQueueMaxCapacity;
            _queue = new FastPriorityQueue<State>(_priorityQueueMaxCapacity);
            _queueB = new FastPriorityQueue<State>(_priorityQueueMaxCapacity);
        }

        private CostWithNode _PopHeadPriorityQueue(bool backwardPQ = false)
        {
            if (backwardPQ)
            {
                State s = _queueB.Dequeue();
                return new CostWithNode(s.totalCostS, s);
            }
            else
            {
                State s = _queue.Dequeue();
                return new CostWithNode(s.totalCostS, s);
            }
        }
        private void _AddPriotiyQueueNode(double cost, State state, bool backwardPQ = false)
        {
            totalNumberOfnodes++;
            if (_queue.Count >= _priorityQueueMaxCapacity - 1 || _queueB.Count >= _priorityQueueMaxCapacity - 1)
            {
                Console.WriteLine(Messages.MaxPQcapacity);
                throw new ArgumentOutOfRangeException(Messages.MaxPQcapacity);
            }
            if (backwardPQ)
                _queueB.Enqueue(state, (float)cost);
            else
                _queue.Enqueue(state, (float)cost);
        }
        private void _ClearQueue()
        {
            _queue.Clear();
            _queueB.Clear();
        }
        private bool _QueueIsEmpty(bool backwardPQ = false)
        {
            if (backwardPQ)
                return true ? _queueB.Count == 0 : false;
            else
                return true ? _queue.Count == 0 : false;
        }
        private void _ExtendNodesToVisit(CostWithNode bestNodeF, CostWithNode bestNodeB, double mu, bool backwardPQ)
        {
            CostWithNode bestNode = backwardPQ ? bestNodeB : bestNodeF;
            foreach (Edge nextEdge in _graph.GetNextEdges(bestNode.State.node.id, lastVisitID))
            {
                double cost = _CostEvaluation(bestNode.State.totalCostS, nextEdge.costS);
                _AddPriotiyQueueNode(cost, new State(cost, nextEdge.targetNode, bestNode.State));
                if ((cost + bestNodeB.cost) < mu)
                {
                    mu = cost + bestNodeB.cost;
                }
            }
        }
        private KeyValuePair<double, State> _NoPathFound(int sourceNodeID, int targetNodeID, string message)
        {
            Console.Write(message);
            Console.WriteLine($"Noeud source : {sourceNodeID}, noeud de destination : {targetNodeID}");
            throw new InvalidOperationException("problèmme");
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
            if (!_graph.NodeExist(sourceNodeID) || !_graph.NodeExist(endNodeID)) 
                return _NoPathFound(sourceNodeID, endNodeID, Messages.NodeDontExist); // arrête si le noeud de départ ou celui recherché n'existe pas
            lastVisitID++;
            _ClearQueue();
            _AddPriotiyQueueNode(0, new State(0, _graph.GetNode(sourceNodeID)));
            _AddPriotiyQueueNode(0, new State(0, _graph.GetNode(endNodeID)), true);
            double mu = Double.PositiveInfinity;
            while (!_QueueIsEmpty() && !_QueueIsEmpty(true))
            {
                CostWithNode bestNode = _PopHeadPriorityQueue();
                CostWithNode bestNodeB = _PopHeadPriorityQueue(true);
                tookNodeNumber++;
                if (bestNode.cost + bestNodeB.cost >= mu)
                    return new KeyValuePair<double, State>(bestNode.cost + bestNodeB.cost, bestNode.State); // modifier les states
                
                bestNode.State.node.visitID = lastVisitID;
                bestNodeB.State.node.visitIDbackward = lastVisitID;



               // _ExtendNodesToVisit(bestNode, bestNodeB, mu, false);
                //_ExtendNodesToVisit(bestNode, bestNodeB, mu, true);
                /*
                foreach (Edge nextEdge in _graph.GetNextEdges(bestNode.State.node.id, lastVisitID))
                {
                    if (bestNode.State.node.visitID == lastVisitID)
                        continue;
                    double cost = _CostEvaluation(bestNode.State.totalCostS, nextEdge.costS);
                    _AddPriotiyQueueNode(cost, new State(cost, nextEdge.targetNode, bestNode.State));
                    if ((cost + bestNodeB.cost) < mu)
                    {
                        mu = cost + bestNodeB.cost;
                    }
                }  
                foreach (Edge nextEdge in _graph.GetNextEdges(bestNodeB.State.node.id, lastVisitID, true))
                {
                    if (bestNode.State.node.visitIDbackward == lastVisitID)
                        continue;
                    double cost = _CostEvaluation(bestNodeB.State.totalCostS, nextEdge.costS);
                    _AddPriotiyQueueNode(cost, new State(cost, nextEdge.sourceNode, bestNodeB.State));
                    if ((cost + bestNodeB.cost) < mu)
                    {
                        mu = cost + bestNode.cost;
                    }
                } 
                */
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
