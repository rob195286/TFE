using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TFE;
using static TFE.GeometricFunctions;


namespace TestTFE.UnitTests
{
    [TestClass]
    public class GeomTest
    {
        Vertex node1;
        Vertex node2;

        [TestMethod]
        public void TestTimeAsCrowFlies()
        {
            double heuristicValue = 111.257;
            node1 = new Vertex(1, 4.390396011887681, 50.84849603970693);
            node2 = new Vertex(2, 4.40755727599761, 50.83769071995793);

            Assert.AreEqual(1.71, Math.Round(EuclideanDistanceCostFromTo(node1, node2, 1)), 2);

            node1 = new Vertex(1, 4.32094751414833, 50.875495617760116);
            node2 = new Vertex(2, 4.467112406894378, 50.83142023866952);

            Assert.AreEqual(11.33, Math.Round(EuclideanDistanceCostFromTo(node1, node2, 1)), 2);
        }
    }
}
