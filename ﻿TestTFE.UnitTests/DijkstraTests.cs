using Microsoft.VisualStudio.TestTools.UnitTesting;
using TFE;

namespace TestTFE.UnitTests
{
    [TestClass]
    public class DijkstraTesta
    {
        Graph graph = new Graph();

        [TestMethod]
        public void TestComputePath()
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
    }
}
