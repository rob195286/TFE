using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using TFE;

namespace TestTFE.UnitTests
{
    [TestClass]
    public class GraphTests
    {
        Graph graph = new Graph();

        [TestInitialize()]
        public void testsInitialize()
        {
        }


        [TestMethod]
        public void TestGetNextEdgesFromNode()
        {
            foreach (Edge edge in graph.GetNextEdges(279, -1))
            {
                Assert.AreEqual(true, new List<int>() { 30988, 49571, 280 }.Contains(edge.targetVertex.id));
            }
            foreach (Edge edge in graph.GetNextEdges(361, -1))
            {
                Assert.AreEqual(true, new List<int>() { 4930, 270077, 1002204, 18428 }.Contains(edge.targetVertex.id));
            }
            foreach (Edge edge in graph.GetNextEdges(1, -1))
            {
                Assert.AreEqual(true, new List<int>() { 45164, 45062 }.Contains(edge.targetVertex.id));
            }
            foreach (Edge edge in graph.GetNextEdges(10, -1))
            {
                Assert.AreEqual(true, new List<int>() { 20670, 11 }.Contains(edge.targetVertex.id));
            }
            foreach (Edge edge in graph.GetNextEdges(516, -1))
            {
                Assert.AreEqual(true, new List<int>() { 27399, 20038, 172622, 27400 }.Contains(edge.targetVertex.id));
            }
            foreach (Edge edge in graph.GetNextEdges(17395, -1))
            {
                Assert.AreEqual(true, new List<int>() { 43691, 43690, 89419, 170391, 170392, 207163 }.Contains(edge.targetVertex.id));
            }
            foreach (Edge edge in graph.GetNextEdges(82382, -1))
            {
                Assert.AreEqual(true, new List<int>() { 74525, 201346, 694765, 694762, 694763, 694764, 727950 }.Contains(edge.targetVertex.id));
            }
            foreach (Edge edge in graph.GetNextEdges(34152, -1))
            {
                Assert.AreEqual(true, new List<int>() { 34151, 789798 }.Contains(edge.targetVertex.id));
            }
        }

        [TestMethod]
        public void TestGetPrivousEdgesFromNode()
        {
            foreach (Edge edge in graph.GetNextReverseEdges(588, -1))
            {
                Assert.AreEqual(true, new List<int>() { 41001, 623436, 274154, 662021, 935935 }.Contains(edge.sourceVertex.id));
            }
            foreach (Edge edge in graph.GetNextReverseEdges(679595, -1))
            {
                Assert.AreEqual(true, new List<int>() { 688737, 688736, 688735, 688734, 688733, 679594 }.Contains(edge.sourceVertex.id));
            }
            foreach (Edge edge in graph.GetNextReverseEdges(280, -1))
            {
                Assert.AreEqual(true, new List<int>() { 314, 365917, 279, }.Contains(edge.sourceVertex.id));
            }
            foreach (Edge edge in graph.GetNextReverseEdges(20929, -1))
            {
                Assert.AreEqual(true, new List<int>() { 27830, 164639, 360555, 898581, 797695, 940121 }.Contains(edge.sourceVertex.id));
            }
            foreach (Edge edge in graph.GetNextReverseEdges(270371, -1))
            {
                Assert.AreEqual(true, new List<int>() { 272413, 521289, 555355, 752449, 272415, 561497 }.Contains(edge.sourceVertex.id));
            }
            foreach (Edge edge in graph.GetNextReverseEdges(34152, -1))
            {
                Assert.AreEqual(true, new List<int>() { 789764 }.Contains(edge.sourceVertex.id));
            }
        }

        [TestMethod]
        public void TestChangeEdgeCost()
        {
            Assert.AreEqual(0.00004491603277117362, graph._edges[1001805435][0].cost);
            Assert.AreEqual(0.0004181054256197113, graph._edges[1001805435][1].cost);

            double multiplier = 3;
            graph.ChangeEdgeCost(1001805435, multiplier);
            Assert.AreEqual(0.0004181054256197113 * multiplier, graph._edges[1001805435][1].cost);

            graph.ChangeEdgeCost(1001805435, 1/multiplier);
            multiplier = 5;
            graph.ChangeEdgeCost(1001805435, multiplier);
            Assert.AreEqual(0.00004491603277117362 * multiplier, graph._edges[1001805435][0].cost);
        }
    }
}
