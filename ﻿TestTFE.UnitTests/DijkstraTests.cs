using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TFE;

namespace TestTFE.UnitTests
{
    [TestClass]
    public class DijkstraTesta
    {
        Dijkstra dijkstra = new Dijkstra(new Graph());

        //[TestMethod]
        public void TestComputePathWithCostS() // Test lorsque le coût des arêtes est en secondes, soit cost_s.
        {
            int idNodeSource = 589961; // arlon
            int idNodeTarget = 458523; // bruge

            Assert.AreEqual(811, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));

            idNodeTarget = 901419;
            Assert.AreEqual(543, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));

            idNodeSource = 121155;
            idNodeTarget = 597177;
            Assert.AreEqual(165, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));

            idNodeTarget = 458523;
            Assert.AreEqual(381, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));

            idNodeTarget = 901419;
            Assert.AreEqual(271, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));

            idNodeSource = 41527;
            Assert.AreEqual(309, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));
        }

        [TestMethod]
        public void TestComparePath() // Test lorsque le coût des arêtes est l'unité de osm, soit cost.
        {
            int idNodeSource = 589961; // arlon
            int idNodeTarget = 458523; // bruge

            Assert.AreEqual(1428, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));

            idNodeTarget = 901419;
            Assert.AreEqual(888, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));

            idNodeSource = 121155;
            idNodeTarget = 597177;
            Assert.AreEqual(165, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));

            idNodeTarget = 458523;
            Assert.AreEqual(486, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));

            idNodeTarget = 901419;
            Assert.AreEqual(349, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));

            idNodeSource = 41527;
            Assert.AreEqual(362, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));
        }

        private int _ComputeNodeNUmber(State pqNode)
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
