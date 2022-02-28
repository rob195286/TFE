using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        public void TestGetNextNode()
        {
            foreach (Node node in graph.GetNextNodes(1))
            {
                Assert.AreEqual(true, new List<int>() { 45164, 45062 }.Contains(node.id));
            }
            Assert.AreEqual(2, graph.GetNextNodes(1).Count);

            foreach (Node node in graph.GetNextNodes(10))
            {
                Assert.AreEqual(true, new List<int>() { 20670, 11 }.Contains(node.id));
            }
            Assert.AreEqual(2, graph.GetNextNodes(10).Count);

            foreach (Node node in graph.GetNextNodes(516))
            {
                Assert.AreEqual(true, new List<int>() { 27399, 20038, 172622, 27400 }.Contains(node.id));
            }
            Assert.AreEqual(4, graph.GetNextNodes(516).Count);

            foreach (Node node in graph.GetNextNodes(17395))
            {
                Assert.AreEqual(true, new List<int>() { 43691, 43690, 89419, 170391, 170392, 207163 }.Contains(node.id));
            }
            Assert.AreEqual(6, graph.GetNextNodes(17395).Count);

            foreach (Node node in graph.GetNextNodes(82382))
            {
                Assert.AreEqual(true, new List<int>() { 74525, 201346, 694765, 694762, 694763, 694764, 727950 }.Contains(node.id));
            }
            Assert.AreEqual(7, graph.GetNextNodes(82382).Count);
        }

        [TestMethod]
        public void TestEdgesNode()
        {
            foreach (Edge edge in graph.GetNode(279).outgoingEdges)
            {
                Assert.AreEqual(true, new List<int>() { 30988, 49571, 280 }.Contains(edge.targetNode.id));
            }

            foreach (Edge edge in graph.GetNode(361).outgoingEdges)
            {
                Assert.AreEqual(true, new List<int>() { 4930, 270077, 1002204, 18428 }.Contains(edge.targetNode.id));
            }
        }
    }
}
