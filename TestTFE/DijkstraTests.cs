using Microsoft.VisualBasic.FileIO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
            
            Assert.AreEqual(_ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value), _ComputeNodeNUmber(dijkstra.ComputeShortestPathWithHeuristic(idNodeSource, idNodeTarget).Value));

            idNodeTarget = 901419;
            Assert.AreEqual(_ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value), _ComputeNodeNUmber(dijkstra.ComputeShortestPathWithHeuristic(idNodeSource, idNodeTarget).Value));

            idNodeSource = 121155;
            idNodeTarget = 597177;
            Assert.AreEqual(_ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value), _ComputeNodeNUmber(dijkstra.ComputeShortestPathWithHeuristic(idNodeSource, idNodeTarget).Value));

            idNodeTarget = 458523;
            Assert.AreEqual(_ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value), _ComputeNodeNUmber(dijkstra.ComputeShortestPathWithHeuristic(idNodeSource, idNodeTarget).Value));
            
            idNodeSource = 41527;
            Assert.AreEqual(754, _ComputeNodeNUmber(dijkstra.ComputeShortestPathWithHeuristic(idNodeSource, idNodeTarget).Value));

            idNodeSource = 930177;
            idNodeTarget = 3944;
            Assert.AreEqual(653, _ComputeNodeNUmber(dijkstra.ComputeShortestPathWithHeuristic(idNodeSource, idNodeTarget).Value));

            idNodeTarget = 192943;
            Assert.AreEqual(594, _ComputeNodeNUmber(dijkstra.ComputeShortestPathWithHeuristic(idNodeSource, idNodeTarget).Value));

            idNodeTarget = 976710;
            Assert.AreEqual(657, _ComputeNodeNUmber(dijkstra.ComputeShortestPathWithHeuristic(idNodeSource, idNodeTarget).Value));

            idNodeTarget = 20125;
            Assert.AreEqual(450, _ComputeNodeNUmber(dijkstra.ComputeShortestPathWithHeuristic(idNodeSource, idNodeTarget).Value));

            idNodeTarget = 228385;
            Assert.AreEqual(324, _ComputeNodeNUmber(dijkstra.ComputeShortestPathWithHeuristic(idNodeSource, idNodeTarget).Value));

            idNodeSource = 130225; 
            idNodeTarget = 593276;
            Assert.AreEqual(161, _ComputeNodeNUmber(dijkstra.ComputeShortestPathWithHeuristic(idNodeSource, idNodeTarget).Value));

            idNodeSource = 742732; 
            idNodeTarget = 519729;
            Assert.AreEqual(685, _ComputeNodeNUmber(dijkstra.ComputeShortestPathWithHeuristic(idNodeSource, idNodeTarget).Value));

            idNodeSource = 97337; 
            idNodeTarget = 213071;
            Assert.AreEqual(979, _ComputeNodeNUmber(dijkstra.ComputeShortestPathWithHeuristic(idNodeSource, idNodeTarget).Value));

            idNodeSource = 354192;
            idNodeTarget = 912989;
            Assert.AreEqual(925, _ComputeNodeNUmber(dijkstra.ComputeShortestPathWithHeuristic(idNodeSource, idNodeTarget).Value));

            idNodeSource = 29979;
            idNodeTarget = 626925;
            Assert.AreEqual(1185, _ComputeNodeNUmber(dijkstra.ComputeShortestPathWithHeuristic(idNodeSource, idNodeTarget).Value));

            idNodeSource = 574422;
            idNodeTarget = 758969;
            Assert.AreEqual(811, _ComputeNodeNUmber(dijkstra.ComputeShortestPathWithHeuristic(idNodeSource, idNodeTarget).Value));

            idNodeSource = 876837;
            idNodeTarget = 11171;
            Assert.AreEqual(249, _ComputeNodeNUmber(dijkstra.ComputeShortestPathWithHeuristic(idNodeSource, idNodeTarget).Value));
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
            Assert.AreEqual(653, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));

            idNodeTarget = 192943;
            Assert.AreEqual(594, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));

            idNodeTarget = 976710;
            Assert.AreEqual(657, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));

            idNodeTarget = 20125;
            Assert.AreEqual(450, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));

            idNodeTarget = 228385;
            Assert.AreEqual(324, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));

            idNodeSource = 130225;
            idNodeTarget = 593276;
            Assert.AreEqual(161, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));

            idNodeSource = 742732; 
            idNodeTarget = 519729;
            Assert.AreEqual(685, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));

            idNodeSource = 97337; 
            idNodeTarget = 213071;
            Assert.AreEqual(979, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));

            idNodeSource = 354192; 
            idNodeTarget = 912989;
            Assert.AreEqual(925, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));

            idNodeSource = 29979;
            idNodeTarget = 626925;
            Assert.AreEqual(1185, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));

            idNodeSource = 574422;
            idNodeTarget = 758969;
            Assert.AreEqual(811, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));

            idNodeSource = 876837;
            idNodeTarget = 11171;
            Assert.AreEqual(249, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));
        }

        [TestMethod]
        public void TestManyPath()
        {
            int idsource = 1315;
            int j = 0;

            using (TextFieldParser parser = new TextFieldParser(@"A:\3)_Bibliotheque\Documents\Ecam\Anne5\TFE\Code\nodes.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(", ");
                parser.ReadLine();
                while (!parser.EndOfData)
                {
                    j++;
                    string[] s = parser.ReadLine().Split(',');
                    int csvTarget = Convert.ToInt32(s[0]);
                    int csvPathN = Convert.ToInt32(s[1]);
                    int numbeNode = _ComputeNodeNUmber(dijkstra.ComputeShortestPathWithHeuristic(idsource, csvTarget).Value);
                    Assert.AreEqual(csvPathN, numbeNode);
                }
            }
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
