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
            foreach (Edge edge in graph.GetNextEdges(279, -1, false))
            {
                Assert.AreEqual(true, new List<int>() { 30988, 49571, 280 }.Contains(edge.targetNode.id));
            }
            foreach (Edge edge in graph.GetNextEdges(361, -1, false))
            {
                Assert.AreEqual(true, new List<int>() { 4930, 270077, 1002204, 18428 }.Contains(edge.targetNode.id));
            }
            foreach (Edge edge in graph.GetNextEdges(1, -1, false))
            {
                Assert.AreEqual(true, new List<int>() { 45164, 45062 }.Contains(edge.targetNode.id));
            }
            foreach (Edge edge in graph.GetNextEdges(10, -1, false))
            {
                Assert.AreEqual(true, new List<int>() { 20670, 11 }.Contains(edge.targetNode.id));
            }
            foreach (Edge edge in graph.GetNextEdges(516, -1, false))
            {
                Assert.AreEqual(true, new List<int>() { 27399, 20038, 172622, 27400 }.Contains(edge.targetNode.id));
            }
            foreach (Edge edge in graph.GetNextEdges(17395, -1, false))
            {
                Assert.AreEqual(true, new List<int>() { 43691, 43690, 89419, 170391, 170392, 207163 }.Contains(edge.targetNode.id));
            }
            foreach (Edge edge in graph.GetNextEdges(82382, -1, false))
            {
                Assert.AreEqual(true, new List<int>() { 74525, 201346, 694765, 694762, 694763, 694764, 727950 }.Contains(edge.targetNode.id));
            }
        }

        [TestMethod]
        public void TestGetPrivousEdgesFromNode()
        {
            foreach (Edge edge in graph.GetNextEdges(588, -1, true))
            {
                Assert.AreEqual(true, new List<int>() { 41001, 623436, 274154, 662021, 935935 }.Contains(edge.sourceNode.id));
            }
            foreach (Edge edge in graph.GetNextEdges(679595, -1, true))
            {
                Assert.AreEqual(true, new List<int>() { 688737, 688736, 688735, 688734, 688733, 679594 }.Contains(edge.sourceNode.id));
            }
            foreach (Edge edge in graph.GetNextEdges(280, -1, true))
            {
                Assert.AreEqual(true, new List<int>() { 314, 365917, 279, }.Contains(edge.sourceNode.id));
            }
            foreach (Edge edge in graph.GetNextEdges(20929, -1, true))
            {
                Assert.AreEqual(true, new List<int>() { 27830, 164639, 360555, 898581, 797695, 940121 }.Contains(edge.sourceNode.id));
            }
            foreach (Edge edge in graph.GetNextEdges(270371, -1, true))
            {
                Assert.AreEqual(true, new List<int>() { 272413, 521289, 555355, 752449, 272415, 561497 }.Contains(edge.sourceNode.id));
            }            
        }
    }
}
