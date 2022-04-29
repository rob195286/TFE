using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TFE;

namespace TestTFE.UnitTests
{
    [TestClass]
    public class DijkstraTesta
    {
        Dijkstra dijkstra = new Dijkstra(new Graph());

        //[TestMethod] // test du premier ComputePath
        public void TestFirstComputePath()
        {
            int idNodeSource = 930177;
            int idNodeTarget = 3944;

            Assert.AreEqual(1.1295318675841068, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key);

            idNodeTarget = 192943;
            Assert.AreEqual(0.493435568964148, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key);

            idNodeTarget = 976710;
            Assert.AreEqual(1.2907317535042766, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key);

            idNodeTarget = 20125;
            Assert.AreEqual(0.4279272117559948, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key);

            idNodeTarget = 228385;
            Assert.AreEqual(1.7062054610490174, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key);
        }

        [TestMethod]
        public void TestComputePath()
        {
            int idNodeSource = 589961; // arlon
            int idNodeTarget = 458523; // bruge

            //Assert.AreEqual(9286.65196539782, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key);
            //Assert.AreEqual(9286.65196539782, Math.Round(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key, 11));
            Assert.AreEqual(811, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));

            idNodeTarget = 901419;
            //Assert.AreEqual(6311.0951068659515, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key);
            //Assert.AreEqual(6311.0951068659515, Math.Round(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key, 13));
            //Assert.AreEqual(6311.095106865957, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key);
            Assert.AreEqual(543, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));

            idNodeSource = 121155;
            idNodeTarget = 597177;
            //Assert.AreEqual(620.756077052044, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key);
            //Assert.AreEqual(620.756077052044, Math.Round(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key, 11));
            //Assert.AreEqual(620.7560770520442, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key);
            Assert.AreEqual(165, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));

            idNodeTarget = 458523;
            //Assert.AreEqual(3229.47845199183, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key);
            //Assert.AreEqual(3229.47845199183, Math.Round(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key, 11));
            //Assert.AreEqual(3229.4784519918285, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key);
            Assert.AreEqual(381, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));
        }
        [TestMethod]
        public void TestComparePath()
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
