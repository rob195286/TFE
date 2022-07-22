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
            foreach (Vertex node in graph.GetNextVertices(1))
            {
                Assert.AreEqual(true, new List<int>() { 45164, 45062 }.Contains(node.id));
            }
            Assert.AreEqual(2, graph.GetNextVertices(1).Count);

            foreach (Vertex node in graph.GetNextVertices(10))
            {
                Assert.AreEqual(true, new List<int>() { 20670, 11 }.Contains(node.id));
            }
            Assert.AreEqual(2, graph.GetNextVertices(10).Count);

            foreach (Vertex node in graph.GetNextVertices(516))
            {
                Assert.AreEqual(true, new List<int>() { 27399, 20038, 172622, 27400 }.Contains(node.id));
            }
            Assert.AreEqual(4, graph.GetNextVertices(516).Count);

            foreach (Vertex node in graph.GetNextVertices(17395))
            {
                Assert.AreEqual(true, new List<int>() { 43691, 43690, 89419, 170391, 170392, 207163 }.Contains(node.id));
            }
            Assert.AreEqual(6, graph.GetNextVertices(17395).Count);

            foreach (Vertex node in graph.GetNextVertices(82382))
            {
                Assert.AreEqual(true, new List<int>() { 74525, 201346, 694765, 694762, 694763, 694764, 727950 }.Contains(node.id));
            }
            Assert.AreEqual(7, graph.GetNextVertices(82382).Count);

            foreach (Vertex node in graph.GetNextVertices(744271))
            {
                Assert.AreEqual(true, new List<int>() { 154544, 456329, 752586 }.Contains(node.id));
            }
            Assert.AreEqual(3, graph.GetNextVertices(744271).Count);
        }

        [TestMethod]
        public void TestGetEdgesNode()
        {
            foreach (Edge edge in graph.GetVertex(279).outgoingEdges)
            {
                Assert.AreEqual(true, new List<int>() { 30988, 49571, 280 }.Contains(edge.targetVertex.id));
            }

            foreach (Edge edge in graph.GetVertex(361).outgoingEdges)
            {
                Assert.AreEqual(true, new List<int>() { 4930, 270077, 1002204, 18428 }.Contains(edge.targetVertex.id));
            }

            foreach (Edge edge in graph.GetVertex(744271).outgoingEdges)
            {
                Assert.AreEqual(true, new List<int>() { 154544, 752586, 456329 }.Contains(edge.targetVertex.id));
            }

            foreach (Edge edge in graph.GetVertex(501134).outgoingEdges)
            {
                Assert.AreEqual(true, new List<int>() { 1057612, 488382, 500032 }.Contains(edge.targetVertex.id));
            }
        }

        [TestMethod]
        public void TestChangeEdgeCost()
        {
            double edgeCost = graph.GetEdges(1001805435).Find(e=> e.sourceVertex.id == 797530).cost;
            double multiplier = 3;
            graph.ChangeEdgeCost(1001805435, multiplier);
            Assert.AreEqual(edgeCost * multiplier, graph.GetEdges(1001805435).Find(e => e.sourceVertex.id == 797530).cost);
            Assert.AreEqual(edgeCost * multiplier, graph.GetEdges(1001805435).Find(e => e.sourceVertex.id == 1053779).cost);

            edgeCost = graph.GetEdges(1001805435).Find(e => e.sourceVertex.id == 1053779).cost;
            multiplier = 5;
            graph.ChangeEdgeCost(1001805435, multiplier);
            Assert.AreEqual(edgeCost * multiplier, graph.GetEdges(1001805435).Find(e => e.sourceVertex.id == 1053779).cost);
        }

        [TestMethod]
        public void TestEdgeComparison()
        {
            Assert.AreEqual(true, graph.GetEdges(1001805435)[0] != graph.GetEdges(1001805435)[1]);

            Edge edge1 = graph.GetEdges(1001805435).Find(e => e.sourceVertex.id == 1053779);
            Edge edge2 = new Edge(0, edge1.cost, 0, 0, 1001805435, 0, 0);
            edge2.sourceVertex = edge1.sourceVertex;
            edge2.targetVertex = edge1.targetVertex;
            Assert.AreEqual(true, edge2.Equals(edge1));
        }

        [TestMethod]
        public void TestEdgeExist()
        {
            Assert.AreEqual(false, graph.EdgeExist(new Edge(0, 0, 0, 0, 264951447, 0, 0)));
            Assert.AreEqual(false, graph.EdgeExist(new Edge(0,0,0,0,0,0,0)));
            graph._AddEdge(new Edge(0, 0, 0, 0, 0, 0, 0));
            Assert.AreEqual(true, graph.EdgeExist(new Edge(0,0,0,0,0,0,0)));
            Assert.AreEqual(true, graph.EdgeExist(graph.GetEdges(264951447)[0]));
        }

        [TestMethod]
        public void TestGetEdge()
        {
            Assert.AreEqual(true, graph.GetEdge(975383472, 154544, 744271, 0.0013712697638836823) != null);
            Edge fakeEdge = new Edge(0, 0, 0, 0, 0, 0, 0);
            fakeEdge.sourceVertex = new Vertex(154544, 0, 0);
            fakeEdge.targetVertex = new Vertex(744271, 0, 0);
            graph._AddEdge(fakeEdge);
            Assert.AreEqual(true, graph.GetEdge(0, 154544, 744271, 0) != null);
        }

    }
}
