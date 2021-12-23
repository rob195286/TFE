﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using TFE;

namespace TestTFE.UnitTests
{
    [TestClass]
    public class DijkstraTests
    {
        Graph graph = new Graph();

        //[TestMethod] // test du premier ComputePath
        public void TestFirstComputePath()
        {
            int idNodeSource = 930177;
            int idNodeTarget = 3944;

            Dijkstra dj = new Dijkstra(graph);

            Assert.AreEqual(1.1295318675841068, dj.ComputeShortestPath(idNodeSource, idNodeTarget).Key);

            idNodeTarget = 192943;
            Assert.AreEqual(0.493435568964148, dj.ComputeShortestPath(idNodeSource, idNodeTarget).Key);

            idNodeTarget = 976710;
            Assert.AreEqual(1.2907317535042766, dj.ComputeShortestPath(idNodeSource, idNodeTarget).Key);

            idNodeTarget = 20125;
            Assert.AreEqual(0.4279272117559948, dj.ComputeShortestPath(idNodeSource, idNodeTarget).Key);

            idNodeTarget = 228385;
            Assert.AreEqual(1.7062054610490174, dj.ComputeShortestPath(idNodeSource, idNodeTarget).Key);
        }

        [TestMethod]
        public void TestComputePath()
        {
            int idNodeSource = 589961;
            int idNodeTarget = 458523;

            Dijkstra dj = new Dijkstra(graph, 4000);

            var shortestPath = dj.ComputeShortestPath(idNodeSource, idNodeTarget);
            Assert.AreEqual(9286.65196539782, shortestPath.Key);
            Assert.AreEqual(811, _ComputeNodeNUmber(shortestPath.Value));

            idNodeSource = 589961;
            idNodeTarget = 901419;

            shortestPath = dj.ComputeShortestPath(idNodeSource, idNodeTarget);
            Assert.AreEqual(6311.0951068659515, shortestPath.Key);
            Assert.AreEqual(543, _ComputeNodeNUmber(shortestPath.Value));

            idNodeSource = 121155;
            idNodeTarget = 597177;

            shortestPath = dj.ComputeShortestPath(idNodeSource, idNodeTarget);
            Assert.AreEqual(620.756077052044, shortestPath.Key);
            Assert.AreEqual(165, _ComputeNodeNUmber(shortestPath.Value));

            idNodeSource = 121155;
            idNodeTarget = 458523;

            shortestPath = dj.ComputeShortestPath(idNodeSource, idNodeTarget);
            Assert.AreEqual(3229.47845199183, shortestPath.Key);
            Assert.AreEqual(381, _ComputeNodeNUmber(shortestPath.Value));
        }

        private void _TestCost(double shortestPathCostInS)
        {

        }

        private int _ComputeNodeNUmber(PriorityQueueNode pqNode)
        {
            int i = 0;
            while (pqNode != null)
            {
                pqNode = pqNode.previousState;
                i++;
            }
            return i;
        }
    }
}
