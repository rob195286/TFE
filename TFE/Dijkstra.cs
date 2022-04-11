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
        public int lastVisit = 0;
        public int totalNumberOfnodes;
        public int tookNodeNumber;

        public Dijkstra(Graph graph, int ppriorityQueueMaxCapacity = 4000/2)
        {
            _graph = graph;
            _priorityQueueMaxCapacity = ppriorityQueueMaxCapacity;
            _queue = new FastPriorityQueue<State>(_priorityQueueMaxCapacity);
            _queueB = new FastPriorityQueue<State>(_priorityQueueMaxCapacity);
        }

        private CostWithNode _PopPriorityQueueHead(bool backwardPQ)
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
        private void _AddPriotiyQueueNode(double cost, State state, bool backwardPQ)
        {
            totalNumberOfnodes++;
            if (_queue.Count >= _priorityQueueMaxCapacity - 1 || _queueB.Count >= _priorityQueueMaxCapacity - 1)
            {
                Console.WriteLine(Messages.MaxPQcapacity);
                Console.WriteLine(_queue.Count);
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
        private bool _QueueIsEmpty(bool backwardPQ)
        {
            if (backwardPQ)
                return _queueB.Count == 0;
            else
                return _queue.Count == 0;
        }
        private void _SearchNextNodes(CostWithNode bestNodeF, CostWithNode bestNodeB, double mu, bool backwardPQ)
        {
            CostWithNode bestNode = backwardPQ ? bestNodeB : bestNodeF;
            foreach (Edge nextEdge in _graph.GetNextEdges(bestNode.state.vertex.id, lastVisit))
            {
                double cost = _CostEvaluation(bestNode.state.totalCostS, nextEdge.costS);
                //_AddPriotiyQueueNode(cost, new State(cost, nextEdge.targetVertex, headForward.state));
                if ((cost + bestNodeB.cost) < mu)
                {
                    mu = cost + bestNodeB.cost;
                }
            }
        }
        private KeyValuePair<double, State> _NoPathFound(int sourceVertexID, int destinationVertexID, string message)
        {
            Console.Write(message);
            Console.WriteLine($"Noeud source -> {sourceVertexID}, noeud de destination -> {destinationVertexID}");
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

        public KeyValuePair<double, State> ComputeShortestPath(int sourceVertexID, int destinationVertexID)
        {
            Dictionary<int, double> bestPathNodesForward = new Dictionary<int, double>();
            Dictionary<int, double> bestPathNodesBackward = new Dictionary<int, double>();
            if (!_graph.VertexExist(sourceVertexID) || !_graph.VertexExist(destinationVertexID)) 
                return _NoPathFound(sourceVertexID, destinationVertexID, Messages.NodeDontExist); // arrête si le noeud de départ ou celui recherché n'existe pas
            lastVisit++;
            _ClearQueue();
            _AddPriotiyQueueNode(0, new State(0, _graph.GetVertex(sourceVertexID)), false);
            _AddPriotiyQueueNode(0, new State(0, _graph.GetVertex(destinationVertexID)), true);
            //_AddPriotiyQueueNode(0, new State(0, 0, 0, _graph.GetVertex(sourceVertexID)));
            //_AddPriotiyQueueNode(0, new State(0, 0, 0, _graph.GetVertex(destinationVertexID), true));
            double mu = Double.PositiveInfinity;
           // double backwardBestCost = Double.PositiveInfinity;
            while (!_QueueIsEmpty(false) && !_QueueIsEmpty(true))
            {
                CostWithNode headForward = _PopPriorityQueueHead(false);
                CostWithNode headBackward = _PopPriorityQueueHead(true);
                bool forwardHeadIsVisited = headForward.state.vertex.lastVisitForward == lastVisit;
                bool backwardHeadIsVisited = headBackward.state.vertex.lastVisitBackward == lastVisit;
                headForward.state.vertex.lastVisitForward = lastVisit;
                headBackward.state.vertex.lastVisitBackward = lastVisit;
                bestPathNodesForward.Add(headForward.state.vertex.id, headForward.cost);
                bestPathNodesBackward.Add(headBackward.state.vertex.id, headBackward.cost);
                tookNodeNumber++;
                if (headForward.cost + headBackward.cost >= mu)
                    return new KeyValuePair<double, State>(headForward.cost + headBackward.cost, headForward.state); // modifier les states
                /*
                if (headForward.state.totalCostSFrontward + headForward.state.totalCostSBackward >= mu)
                    return new KeyValuePair<double, State>(headForward.state.totalCostSFrontward + headForward.state.totalCostSBackward, headForward.state); // modifier les states

                if (headForward.state.vertex.lastVisitForward == lastVisit)
                    continue;

                if (headForward.state.isBackwardNode) // backward
                {
                    headForward.state.vertex.visitIDbackward = lastVisit;
                    foreach (Edge nextEdge in _graph.GetPreviousEdges(headForward.state.vertex.id, lastVisit))
                    {
                        double cost = _CostEvaluation(headForward.state.totalCostS, nextEdge.costS);
                        _AddPriotiyQueueNode(cost, new State(cost, cost, headForward.state.totalCostSFrontward, nextEdge.sourceVertex, true, headForward.state));
                        if (headForward.state.vertex.lastVisitForward == lastVisit && nextEdge.targetVertex.lastVisitForward == lastVisit)
                            mu = cost + headForward.state.totalCostSFrontward;
                    }
                }
                else // forward
                {
                    headForward.state.vertex.lastVisitForward = lastVisit;                    
                    foreach (Edge nextEdge in _graph.GetNextEdges(headForward.state.vertex.id, lastVisit))
                    {
                        double cost = _CostEvaluation(headForward.state.totalCostS, nextEdge.costS);
                        _AddPriotiyQueueNode(cost, new State(cost, headForward.state.totalCostSBackward, cost, nextEdge.targetVertex, false, headForward.state));
                        if (headForward.state.vertex.lastVisitForward == lastVisit && nextEdge.targetVertex.lastVisitForward == lastVisit)
                            mu = cost + headForward.state.totalCostSBackward;
                        //if (headForward.state.vertex.lastVisitForward == lastVisit && nextEdge.targetVertex.visitIDbackward == lastVisit)
                         //   mu = cost + backwardBestCost;
                    }
                }
                */



                // _ExtendNodesToVisit(headForward, headBackward, mu, false);
                //_ExtendNodesToVisit(headForward, headBackward, mu, true);
                if (!forwardHeadIsVisited)
                {
                    foreach (Edge nextEdge in _graph.GetNextEdges(headForward.state.vertex.id, lastVisit)) // forward
                    {
                        double cost = _CostEvaluation(headForward.state.totalCostS, nextEdge.costS);
                        _AddPriotiyQueueNode(cost, new State(cost, nextEdge.targetVertex, headForward.state), true);
                        //if ((cost + headBackward.cost < mu) && nextEdge.targetVertex.lastVisitForward == lastVisit)
                        if (bestPathNodesBackward.ContainsKey(nextEdge.targetVertex.id) && cost + bestPathNodesBackward[nextEdge.targetVertex.id] < mu)
                        {
                            mu = cost + bestPathNodesBackward[nextEdge.targetVertex.id];
                        }
                    }
                }
                //-------------------------------------------------------------------------------------------------------
                if (!backwardHeadIsVisited)
                {
                    foreach (Edge nextEdge in _graph.GetPreviousEdges(headBackward.state.vertex.id, lastVisit))
                    {
                        if (backwardHeadIsVisited)
                            break;
                        double cost = _CostEvaluation(headBackward.state.totalCostS, nextEdge.costS);
                        _AddPriotiyQueueNode(cost, new State(cost, nextEdge.sourceVertex, headBackward.state), false);
                        //if ((cost + headForward.cost < mu) && nextEdge.sourceVertex.visitIDbackward == lastVisit)
                        if (bestPathNodesForward.ContainsKey(nextEdge.sourceVertex.id) && cost + bestPathNodesForward[nextEdge.sourceVertex.id] < mu)
                        {
                            mu = cost + bestPathNodesForward[nextEdge.sourceVertex.id];
                        }
                    }
                }
            }
            return _NoPathFound(sourceVertexID, destinationVertexID, Messages.NoPathFound);
        }
    }
    public class State : FastPriorityQueueNode
    {
        public double totalCostS { get; } 
        public Vertex vertex { get; }
        public State previousState { get; }
        public string roadName { get; }
        public Edge edgeToNode { get; }
        //public bool isBackwardNode { get; }

        public State(double ptotalCost,
                    Vertex pvertex,
                    //bool pisBackwardNode = false,
                    State ppreviousState = null
                    )
        {
            totalCostS = ptotalCost;
            vertex = pvertex;
            previousState = ppreviousState;
            //isBackwardNode = pisBackwardNode;
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
