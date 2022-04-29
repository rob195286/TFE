using Priority_Queue; // fast-priority queue
using System.Collections.Generic;

namespace TFE
{
    public class Dijkstra
    {
        private FastPriorityQueue<PriorityQueueNode> _queue = new FastPriorityQueue<PriorityQueueNode>(2000); // fast priority queue
        private Graph _graph;
        public int lastVisitID = 0;
        public int totalNumberOfnodes;
        public int tookNodeNumber;

        public Dijkstra(Graph graph)
        {
            _graph = graph;
        }

        private KeyValuePair<double, State> PopNextItem()
        {
            PriorityQueueNode qi = _queue.Dequeue();
            return new KeyValuePair<double, State>(qi.key, qi.value);
        }

        private void AddNode(double cost, State state)
        {
            _queue.Enqueue(new PriorityQueueNode(cost, state), (float)cost);
            totalNumberOfnodes++;
        }
        private void ClearQueue()
        {
            _queue.Clear();
        }
        private bool QueueIsEmpty()
        {
            return true ? _queue.Count == 0 : false;
        }
        private KeyValuePair<double, State> NoPathFound()
        {
            return new KeyValuePair<double, State>();
        }
        public KeyValuePair<double, State> ComputeShortestPath(int sourceNodeID, int targetNodeID)
        {
            if (!_graph.NodeExist(sourceNodeID) || !_graph.NodeExist(targetNodeID)) return NoPathFound(); // arrête si le noeud de départ ou celui recherché n'existe pas
            lastVisitID++;
            ClearQueue();
            AddNode(0, new State(0, _graph.GetNode(sourceNodeID)));
            tookNodeNumber = 0;
            totalNumberOfnodes = 0;
            while (!QueueIsEmpty())
            {
                var bestNodeAndCost = PopNextItem();
                tookNodeNumber++;
                if (bestNodeAndCost.Value.node.VisitID == lastVisitID) continue;
                if (bestNodeAndCost.Value.node.id != targetNodeID) bestNodeAndCost.Value.node.VisitID = lastVisitID;
                if (bestNodeAndCost.Value.node.id == targetNodeID) return new KeyValuePair<double, State>(bestNodeAndCost.Key, bestNodeAndCost.Value);
                foreach (Edge nextEdge in _graph.GetNextEdges(bestNodeAndCost.Value.node.id, lastVisitID, targetNodeID))
                {
                    AddNode(bestNodeAndCost.Key + nextEdge.cost,
                            new State(bestNodeAndCost.Key + nextEdge.cost,
                                    nextEdge.targetNode,
                                    bestNodeAndCost.Value,
                                    nextEdge.roadName,
                                    nextEdge)
                    );
                }
            }
            return NoPathFound();
        }
    }
    public class State
    {
        public double cost { get; private set; }
        public double cost_s { get; private set; }
        public double length_m { get; private set; }
        public Node node { get; private set; }
        public State previousState { get; private set; }
        public string roadName { get; private set; }
        public int tag { get; private set; }
        public Edge edgeToNode { get; private set; }

        public State(double pcost, Node pnode, State ppreviousState = null, string proadName = "", Edge pedge = null)
        {
            cost = pcost;
            cost_s = -1;
            length_m = -1;
            node = pnode;
            previousState = ppreviousState;
            roadName = proadName;
            edgeToNode = pedge;
        }
    }

    internal class PriorityQueueNode : FastPriorityQueueNode
    {
        public double key { get; }
        public State value { get; }

        public PriorityQueueNode(double cost, State state)
        {
            key = cost;
            value = state;
        }
    }
    
}
