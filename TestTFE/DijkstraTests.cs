using Microsoft.VisualStudio.TestTools.UnitTesting;
using TFE;

namespace TestTFE.UnitTests
{
    [TestClass]
    public class DijkstraTests
    {
        Dijkstra dijkstra = new Dijkstra(new Graph());

        /// <summary>
        ///     Test lorsque le coût des arêtes est l'unité de osm, soit cost avec l'heuristic.
        /// </summary>
        [TestMethod]
        public void TestComputePathCostWithHeuristic()
        {
            int idNodeSource = 589961; // arlon
            int idNodeTarget = 458523; // bruge

            Assert.AreEqual(_ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value), _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget, true).Value));

            idNodeTarget = 901419;
            Assert.AreEqual(_ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value), _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget, true).Value));

            idNodeSource = 121155;
            idNodeTarget = 597177;
            Assert.AreEqual(_ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value), _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget, true).Value));

            idNodeTarget = 458523;
            Assert.AreEqual(_ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value), _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget, true).Value));

            idNodeSource = 41527;
            Assert.AreEqual(754, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget, true).Value));
            Assert.AreEqual(1.3565801602063976, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget, true).Key);

            idNodeSource = 930177;
            idNodeTarget = 3944;
            Assert.AreEqual(1.1295318675841068, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget, true).Key);

            idNodeTarget = 192943;
            Assert.AreEqual(0.493435568964148, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget, true).Key);

            idNodeTarget = 976710;
            Assert.AreEqual(1.2907317535042766, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget, true).Key);

            idNodeTarget = 20125;
            Assert.AreEqual(0.4279272117559948, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget, true).Key);

            idNodeTarget = 228385;
            Assert.AreEqual(1.7062054610490174, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget, true).Key);
            //------------------------------------------------------------------------------------------------------------
            idNodeSource = 130225; // 15
            idNodeTarget = 593276;
            Assert.AreEqual(0.20098997875357846, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget, true).Key);

            idNodeSource = 742732; // 68
            idNodeTarget = 519729;
            Assert.AreEqual(0.6816443003014081, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget, true).Key);

            idNodeSource = 97337; // 1
            idNodeTarget = 213071;
            Assert.AreEqual(0.9742549125833831, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget, true).Key);

            idNodeSource = 354192; // 28
            idNodeTarget = 912989;
           // Assert.AreEqual(0.9501691769514861, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget, true).Key);
        }
        //[TestMethod]
        public void TestComputePathCost_s()
        {
            int idNodeSource = 589961; // arlon
            int idNodeTarget = 458523; // bruge

            Assert.AreEqual(9286.65196539782, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key);
            Assert.AreEqual(811, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));

            idNodeTarget = 901419;
            Assert.AreEqual(6311.0951068659515, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key);
            Assert.AreEqual(543, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));

            idNodeSource = 121155;
            idNodeTarget = 597177;
            Assert.AreEqual(620.756077052044, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key);
            Assert.AreEqual(165, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));

            idNodeTarget = 458523;
            Assert.AreEqual(3229.47845199183, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key);
            Assert.AreEqual(381, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));
        }
        /// <summary>
        ///     Test lorsque le coût des arêtes est l'unité de osm, soit cost.
        /// </summary>
        [TestMethod]
        public void TestComputePathCost()
        {
            int idNodeSource = 589961; // arlon
            int idNodeTarget = 901419; // bruge

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

            idNodeSource = 930177;
            idNodeTarget = 3944;
            Assert.AreEqual(1.1295318675841068, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key);

            idNodeTarget = 192943;
            Assert.AreEqual(0.493435568964148, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key);

            idNodeTarget = 976710;
            Assert.AreEqual(1.2907317535042766, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key);

            idNodeTarget = 20125;
            Assert.AreEqual(0.4279272117559948, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key);

            idNodeTarget = 228385;
            Assert.AreEqual(1.7062054610490174, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key);
            //------------------------------------------------------------------------------------------------------------
            idNodeSource = 130225; // 15
            idNodeTarget = 593276;
            Assert.AreEqual(0.20098997875357846, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key);

            idNodeSource = 742732; // 68
            idNodeTarget = 519729;
            Assert.AreEqual(0.6816443003014081, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key);

            idNodeSource = 97337; // 1
            idNodeTarget = 213071;
            Assert.AreEqual(0.9742549125833831, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key);

            idNodeSource = 354192; // 28
            idNodeTarget = 912989;
          //  Assert.AreEqual(0.9501691769514861, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key);
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
