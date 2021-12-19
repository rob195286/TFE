using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using static TFE.GeometricFunctions;


namespace TestTFE.UnitTests
{
    [TestClass]
    public class GeomTest
    {
        [TestMethod]
        public void TestGeomOFTwoCoord()
        {
            Assert.AreEqual(1.71, Math.Round(Haversine(50.84849603970693, 4.390396011887681,
                                                              50.83769071995793, 4.40755727599761))
                                             , 2);

            Assert.AreEqual(11.33, Math.Round(Haversine(50.875495617760116, 4.32094751414833,
                                                              50.83142023866952, 4.467112406894378))
                                             , 2);
        }
    }
}
