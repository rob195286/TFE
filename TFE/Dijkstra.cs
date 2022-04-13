﻿using Priority_Queue; // fast-priority queue
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

        public Dijkstra(Graph graph, int ppriorityQueueMaxCapacity = 4000*2/2)
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
        private void _AddNode(double cost, State state, bool backwardPQ)
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
                //_AddNode(cost, new State(cost, nextEdge.targetVertex, headForward.state));
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
        private State _RebuildPath(State s1, State s2)
        {
            s2.previousState = s1; // On fait une union entre le chemin de l'avant (s1) et le premier noeud du second chemin (s2).
            State tempState = s2;
            while (tempState.nextState != null)
            {
                tempState = tempState.nextState;
                tempState.previousState = s2;
                s2 = tempState;
            }
            return tempState;
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
            Dictionary<int, State> bestPathNodesForward = new Dictionary<int, State>();
            Dictionary<int, State> bestPathNodesBackward = new Dictionary<int, State>();
            if (!_graph.VertexExist(sourceVertexID) || !_graph.VertexExist(destinationVertexID)) 
                return _NoPathFound(sourceVertexID, destinationVertexID, Messages.NodeDontExist); // arrête si le noeud de départ ou celui recherché n'existe pas
            lastVisit++;
            _ClearQueue();
            _AddNode(0, new State(0, _graph.GetVertex(sourceVertexID)), false);
            _AddNode(0, new State(0, _graph.GetVertex(destinationVertexID)), true);           
            double mu = Double.PositiveInfinity;
            State lastStateFrontward = null;
            State lastStateBackward = null;
            // double backwardBestCost = Double.PositiveInfinity;
            int x = 5;
            while (!_QueueIsEmpty(true) && !_QueueIsEmpty(false))
            {
                CostWithNode headForward = _PopPriorityQueueHead(false);
                CostWithNode headBackward = _PopPriorityQueueHead(true);
                if (headForward.state.getVertexID == 866027)
                    x = 5;
                if (headBackward.state.getVertexID == 866027)
                    x = 5;
                tookNodeNumber++;
                if (headForward.cost + headBackward.cost >= mu)
                    //return new KeyValuePair<double, State>(lastStateFrontward.totalCostS + lastStateBackward.totalCostS, _RebuildPath(lastStateFrontward, lastStateFrontward));
                    return new KeyValuePair<double, State>(headForward.cost + headBackward.cost, _RebuildPath(headForward.state, headBackward.state));
                bool forwardVertexHasBeenVisited = headForward.state.vertex.lastVisitForward == lastVisit;    // On doit les mettre ici car après l'actualisation à l'itération acuel (liast visit) fait
                bool backwardVertexHasBeenVisited = headBackward.state.vertex.lastVisitBackward == lastVisit; //    qu'on rentre pas dans la boucle for. 
                headForward.state.vertex.lastVisitForward = lastVisit;
                headBackward.state.vertex.lastVisitBackward = lastVisit;
                
                if (forwardVertexHasBeenVisited)
                    bestPathNodesForward[headForward.state.getVertexID] = headForward.state;
                else
                    bestPathNodesForward.Add(headForward.state.getVertexID, headForward.state);
                if (backwardVertexHasBeenVisited)
                    bestPathNodesBackward[headBackward.state.getVertexID] = headBackward.state;
                else
                    bestPathNodesBackward.Add(headBackward.state.getVertexID, headBackward.state);


                if (!forwardVertexHasBeenVisited)
                {                   
                    foreach (Edge nextEdge in _graph.GetNextEdges(headForward.state.vertex.id, lastVisit)) // forward
                    {
                        if (nextEdge.targetVertex.id == 866027)
                            x = 5;
                        double cost = _CostEvaluation(headForward.cost, nextEdge.costS);
                        _AddNode(cost, new State(cost, nextEdge.targetVertex, headForward.state), false);
                        //if ((cost + headBackward.cost < mu) && nextEdge.targetVertex.lastVisitForward == lastVisit)
                        if (bestPathNodesBackward.ContainsKey(nextEdge.targetVertex.id) && cost + bestPathNodesBackward[nextEdge.targetVertex.id].totalCostS < mu)
                        {
                            mu = cost + bestPathNodesBackward[nextEdge.targetVertex.id].totalCostS;
                            lastStateFrontward = headForward.state;
                        }
                    }
                }                
                //-------------------------------------------------------------------------------------------------------
                if (!backwardVertexHasBeenVisited) 
                {                   
                    foreach (Edge nextEdge in _graph.GetPreviousEdges(headBackward.state.vertex.id, lastVisit)) // backward
                    {
                        if (nextEdge.sourceVertex.id == 866027)
                            x = 5;
                        double cost = _CostEvaluation(headBackward.cost, nextEdge.costS);
                        _AddNode(cost, new State(cost, nextEdge.sourceVertex, null, headBackward.state), true);
                        //if ((cost + headForward.cost < mu) && nextEdge.sourceVertex.visitIDbackward == lastVisit)
                        if (bestPathNodesForward.ContainsKey(nextEdge.sourceVertex.id) && cost + bestPathNodesForward[nextEdge.sourceVertex.id].totalCostS < mu)
                        {
                            mu = cost + bestPathNodesForward[nextEdge.sourceVertex.id].totalCostS;
                            lastStateBackward = headBackward.state;
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
        public State previousState { get; set; }
        public State nextState { get; set; }
        public int getVertexID
        {
            get { return vertex.id;  }
        }
       // public string roadName { get; }
        //public Edge edgeToNode { get; }
       // public bool isBackwardNode { get; }

        public State(double ptotalCost,
                    Vertex pvertex,
                    State ppreviousState = null,
                    State pnextState = null
                    //bool pisBackwardNode = false
                    )
        {
            totalCostS = ptotalCost;
            vertex = pvertex;
            previousState = ppreviousState;
            nextState = pnextState;
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
