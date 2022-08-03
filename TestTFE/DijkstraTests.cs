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
            Assert.AreEqual(754, _ComputeNodeNUmber(dijkstra.ComputeShortestPathWithHeuristic(idNodeSource, idNodeTarget, true).Value));
            //  Assert.AreEqual(1.3565801602063976, dijkstra.ComputeShortestPathWithHeuristic(idNodeSource, idNodeTarget, true).Key);

            idNodeSource = 930177;
            idNodeTarget = 3944;
            // Assert.AreEqual(1.1295318675841068, dijkstra.ComputeShortestPathWithHeuristic(idNodeSource, idNodeTarget, true).Key);
            Assert.AreEqual(653, _ComputeNodeNUmber(dijkstra.ComputeShortestPathWithHeuristic(idNodeSource, idNodeTarget, true).Value));

            idNodeTarget = 192943;
            // Assert.AreEqual(0.493435568964148, dijkstra.ComputeShortestPathWithHeuristic(idNodeSource, idNodeTarget, true).Key);
            Assert.AreEqual(594, _ComputeNodeNUmber(dijkstra.ComputeShortestPathWithHeuristic(idNodeSource, idNodeTarget, true).Value));

            idNodeTarget = 976710;
            // Assert.AreEqual(1.2907317535042766, dijkstra.ComputeShortestPathWithHeuristic(idNodeSource, idNodeTarget, true).Key);
            Assert.AreEqual(657, _ComputeNodeNUmber(dijkstra.ComputeShortestPathWithHeuristic(idNodeSource, idNodeTarget, true).Value));

            idNodeTarget = 20125;
            // Assert.AreEqual(0.4279272117559948, dijkstra.ComputeShortestPathWithHeuristic(idNodeSource, idNodeTarget, true).Key);
            Assert.AreEqual(450, _ComputeNodeNUmber(dijkstra.ComputeShortestPathWithHeuristic(idNodeSource, idNodeTarget, true).Value));

            idNodeTarget = 228385;
            // Assert.AreEqual(1.7062054610490174, dijkstra.ComputeShortestPathWithHeuristic(idNodeSource, idNodeTarget, true).Key);
            Assert.AreEqual(324, _ComputeNodeNUmber(dijkstra.ComputeShortestPathWithHeuristic(idNodeSource, idNodeTarget, true).Value));
            //------------------------------------------------------------------------------------------------------------
            idNodeSource = 130225; // 15
            idNodeTarget = 593276;
            // Assert.AreEqual(0.20098997875357846, dijkstra.ComputeShortestPathWithHeuristic(idNodeSource, idNodeTarget, true).Key);
            Assert.AreEqual(161, _ComputeNodeNUmber(dijkstra.ComputeShortestPathWithHeuristic(idNodeSource, idNodeTarget, true).Value));

            idNodeSource = 742732; // 68
            idNodeTarget = 519729;
            Assert.AreEqual(685, _ComputeNodeNUmber(dijkstra.ComputeShortestPathWithHeuristic(idNodeSource, idNodeTarget, true).Value));
            //  Assert.AreEqual(0.6816443003014081, dijkstra.ComputeShortestPathWithHeuristic(idNodeSource, idNodeTarget, true).Key);

            idNodeSource = 97337; // 1
            idNodeTarget = 213071;
            // Assert.AreEqual(0.9742549125833831, dijkstra.ComputeShortestPathWithHeuristic(idNodeSource, idNodeTarget, true).Key);
            Assert.AreEqual(979, _ComputeNodeNUmber(dijkstra.ComputeShortestPathWithHeuristic(idNodeSource, idNodeTarget, true).Value));

            idNodeSource = 354192; // 28
            idNodeTarget = 912989;
            // Assert.AreEqual(0.9501691769514861, dijkstra.ComputeShortestPathWithHeuristic(idNodeSource, idNodeTarget, true).Key);
            Assert.AreEqual(925, _ComputeNodeNUmber(dijkstra.ComputeShortestPathWithHeuristic(idNodeSource, idNodeTarget, true).Value));

            idNodeSource = 29979;
            idNodeTarget = 626925;
            //  Assert.AreEqual(1.074207676940353, dijkstra.ComputeShortestPathWithHeuristic(idNodeSource, idNodeTarget, true).Key);
            Assert.AreEqual(1185, _ComputeNodeNUmber(dijkstra.ComputeShortestPathWithHeuristic(idNodeSource, idNodeTarget, true).Value));

            idNodeSource = 574422;
            idNodeTarget = 758969;
            //  Assert.AreEqual(0.9423672755838142, dijkstra.ComputeShortestPathWithHeuristic(idNodeSource, idNodeTarget, true).Key);
            Assert.AreEqual(811, _ComputeNodeNUmber(dijkstra.ComputeShortestPathWithHeuristic(idNodeSource, idNodeTarget, true).Value));
            //------------------------------------------------------------------------------------------------------------
            idNodeSource = 876837;
            idNodeTarget = 11171;
            //  Assert.AreEqual(0.41988786887408996, dijkstra.ComputeShortestPathWithHeuristic(idNodeSource, idNodeTarget, true).Key);
            Assert.AreEqual(249, _ComputeNodeNUmber(dijkstra.ComputeShortestPathWithHeuristic(idNodeSource, idNodeTarget, true).Value));
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
            //  Assert.AreEqual(1.3565801602063976, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key);

            idNodeSource = 930177;
            idNodeTarget = 3944;
            // Assert.AreEqual(1.1295318675841068, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key);
            Assert.AreEqual(653, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));

            idNodeTarget = 192943;
            // Assert.AreEqual(0.493435568964148, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key);
            Assert.AreEqual(594, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));

            idNodeTarget = 976710;
            // Assert.AreEqual(1.2907317535042766, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key);
            Assert.AreEqual(657, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));

            idNodeTarget = 20125;
            // Assert.AreEqual(0.4279272117559948, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key);
            Assert.AreEqual(450, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));

            idNodeTarget = 228385;
            // Assert.AreEqual(1.7062054610490174, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key);
            Assert.AreEqual(324, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));
            //------------------------------------------------------------------------------------------------------------
            idNodeSource = 130225; // 15
            idNodeTarget = 593276;
            // Assert.AreEqual(0.20098997875357846, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key);
            Assert.AreEqual(161, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));

            idNodeSource = 742732; // 68
            idNodeTarget = 519729;
            Assert.AreEqual(685, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));
            //  Assert.AreEqual(0.6816443003014081, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key);

            idNodeSource = 97337; // 1
            idNodeTarget = 213071;
            // Assert.AreEqual(0.9742549125833831, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key);
            Assert.AreEqual(979, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));

            idNodeSource = 354192; // 28
            idNodeTarget = 912989;
            // Assert.AreEqual(0.9501691769514861, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key);
            Assert.AreEqual(925, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));

            idNodeSource = 29979;
            idNodeTarget = 626925;
            //  Assert.AreEqual(1.074207676940353, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key);
            Assert.AreEqual(1185, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));

            idNodeSource = 574422;
            idNodeTarget = 758969;
            //  Assert.AreEqual(0.9423672755838142, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key);
            Assert.AreEqual(811, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));
            //------------------------------------------------------------------------------------------------------------
            idNodeSource = 876837;
            idNodeTarget = 11171;
            //  Assert.AreEqual(0.41988786887408996, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget, true).Key);
            Assert.AreEqual(249, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));
        }

        // [TestMethod]
        public void TestComputePathCostOneWay()
        {
            int idNodeSource = 589961; // arlon
            int idNodeTarget = 901419; // bruge

            Assert.AreEqual(591, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));

            idNodeSource = 121155;
            idNodeTarget = 597177;
            Assert.AreEqual(235, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));

            idNodeTarget = 458523;
            Assert.AreEqual(385, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));

            idNodeTarget = 901419;
            Assert.AreEqual(386, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));

            idNodeSource = 41527;
            Assert.AreEqual(421, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));
            //------------------------------------------------------------------------------------------------------------
            idNodeSource = 1315;
            idNodeTarget = 318262;
            Assert.AreEqual(358, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));
            //------------------------------------------------------------------------------------------------------------

            idNodeSource = 930177;
            idNodeTarget = 3944;
            // Assert.AreEqual(1.1964282013965417m, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key);
            Assert.AreEqual(536, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));

            idNodeTarget = 192943;
            // Assert.AreEqual(0.5284315977464368m, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key);
            Assert.AreEqual(246, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));

            idNodeTarget = 976710;
            // Assert.AreEqual(1.323522602644284m, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key);
            Assert.AreEqual(609, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));

            idNodeTarget = 20125;
            // Assert.AreEqual(0.43292080078577067m, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key);
            Assert.AreEqual(358, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));

            idNodeTarget = 228385;
            //  Assert.AreEqual(1.7620042123527266, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key);
            Assert.AreEqual(423, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));

            //------------------------------------------------------------------------------------------------------------
            idNodeSource = 130225; // 15
            idNodeTarget = 593276;
            Assert.AreEqual(0, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key);

            idNodeSource = 742732; // 68
            idNodeTarget = 519729;
            Assert.AreEqual(0, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key);

            idNodeSource = 97337; // 1
            idNodeTarget = 213071;
            //   Assert.AreEqual(1.3472074938199141m, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key);
            Assert.AreEqual(588, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Value));

            idNodeSource = 354192; // 28
            idNodeTarget = 912989;
            Assert.AreEqual(0, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key);

            idNodeSource = 29979;
            idNodeTarget = 626925;
            Assert.AreEqual(0, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key);
            //------------------------------------------------------------------------------------------------------------

            idNodeSource = 876837;
            idNodeTarget = 11171;
            Assert.AreEqual(0, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget).Key);
        }

        // [TestMethod]
        public void TestComputePathCostOneWayWithHeuristic()
        {
            int idNodeSource = 589961; // arlon
            int idNodeTarget = 901419; // bruge

            Assert.AreEqual(591, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget, true).Value));

            idNodeSource = 121155;
            idNodeTarget = 597177;
            Assert.AreEqual(235, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget, true).Value));

            idNodeTarget = 458523;
            Assert.AreEqual(385, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget, true).Value));

            idNodeTarget = 901419;
            Assert.AreEqual(386, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget, true).Value));

            idNodeSource = 41527;
            Assert.AreEqual(421, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget, true).Value));
            //------------------------------------------------------------------------------------------------------------
            idNodeSource = 1315;
            idNodeTarget = 318262;
            Assert.AreEqual(358, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget, true).Value));
            //------------------------------------------------------------------------------------------------------------

            idNodeSource = 930177;
            idNodeTarget = 3944;
            // Assert.AreEqual(1.1964282013965417m, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget, true).Key);
            Assert.AreEqual(536, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget, true).Value));

            idNodeTarget = 192943;
            // Assert.AreEqual(0.5284315977464368m, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget, true).Key);
            Assert.AreEqual(246, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget, true).Value));

            idNodeTarget = 976710;
            // Assert.AreEqual(1.323522602644284m, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget, true).Key);
            Assert.AreEqual(609, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget, true).Value));

            idNodeTarget = 20125;
            // Assert.AreEqual(0.43292080078577067m, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget, true).Key);
            Assert.AreEqual(358, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget, true).Value));

            idNodeTarget = 228385;
            //  Assert.AreEqual(1.7620042123527266, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget, true).Key);
            Assert.AreEqual(423, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget, true).Value));

            //------------------------------------------------------------------------------------------------------------
            idNodeSource = 130225; // 15
            idNodeTarget = 593276;
            Assert.AreEqual(0, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget, true).Key);

            idNodeSource = 742732; // 68
            idNodeTarget = 519729;
            Assert.AreEqual(0, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget, true).Key);

            idNodeSource = 97337; // 1
            idNodeTarget = 213071;
            //   Assert.AreEqual(1.3472074938199141m, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget, true).Key);
            Assert.AreEqual(588, _ComputeNodeNUmber(dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget, true).Value));

            idNodeSource = 354192; // 28
            idNodeTarget = 912989;
            Assert.AreEqual(0, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget, true).Key);

            idNodeSource = 29979;
            idNodeTarget = 626925;
            Assert.AreEqual(0, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget, true).Key);
            //------------------------------------------------------------------------------------------------------------

            idNodeSource = 876837;
            idNodeTarget = 11171;
            Assert.AreEqual(0, dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget, true).Key);
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
