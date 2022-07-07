using Microsoft.VisualStudio.TestTools.UnitTesting;
using TFE;

namespace TestTFE.UnitTests
{
    [TestClass]
    public class DijkstraTests
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

            Assert.AreEqual(9286.65196539782, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key);
            Assert.AreEqual(811, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));
           // Assert.AreEqual(_ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value), _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget, true).Value));

            idNodeTarget = 901419;
            Assert.AreEqual(6311.0951068659515, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key);
            Assert.AreEqual(543, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));
           // Assert.AreEqual(_ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value), _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget, true).Value));

            idNodeSource = 121155;
            idNodeTarget = 597177;
            Assert.AreEqual(620.756077052044, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key);
            Assert.AreEqual(165, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));
          //  Assert.AreEqual(_ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value), _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget, true).Value));

            idNodeTarget = 458523;
            Assert.AreEqual(3229.47845199183, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key);
            Assert.AreEqual(381, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));
          //  Assert.AreEqual(_ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value), _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget, true).Value));
        }
        //[TestMethod]
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
