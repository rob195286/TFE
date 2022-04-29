using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFE
{
    public class GraphTest
    {
        private Dictionary<int, Vertex> _vertices = new Dictionary<int, Vertex>();


        public GraphTest() {
            _CreateGraph();
        }

        private void _CreateGraph()
        {
            double mutiplier1 = 1;
            Edge edge1 = new Edge(0, "", 0, 8* mutiplier1, 0, 0);
            Edge edge2 = new Edge(0, "", 0, 3* mutiplier1, 0, 0);
            Edge edge3 = new Edge(0, "", 0, 13* mutiplier1, 0, 0);
            _AddVertexWithOutgoingEdges(new Vertex(1, 0, 0), new List<Edge>() { edge1, edge2, edge3 });

            edge1 = new Edge(0, "", 0, 8 * mutiplier1, 0, 0);
            edge2 = new Edge(0, "", 0, 8 * mutiplier1, 0, 0);
            edge3 = new Edge(0, "", 0, 8 * mutiplier1, 0, 0);
            _AddVertexWithOutgoingEdges(new Vertex(2, 0, 0), new List<Edge>() { });
            _AddVertexWithOutgoingEdges(new Vertex(3, 0, 0), new List<Edge>() { });
            _AddVertexWithOutgoingEdges(new Vertex(4, 0, 0), new List<Edge>() { });
            _AddVertexWithOutgoingEdges(new Vertex(5, 0, 0), new List<Edge>() { });
            _AddVertexWithOutgoingEdges(new Vertex(6, 0, 0), new List<Edge>() { });
            _AddVertexWithOutgoingEdges(new Vertex(7, 0, 0), new List<Edge>() { });
            _AddVertexWithOutgoingEdges(new Vertex(8, 0, 0), new List<Edge>() { });
            _AddVertexWithOutgoingEdges(new Vertex(9, 0, 0), new List<Edge>() { });

            for (int i = 1; i <= 9; i++){
            }
        }
        private void _AddVertexWithOutgoingEdges(Vertex v, List<Edge> le)
        {
            foreach(Edge e in le)
                v.AddOutgoingEdge(e);
            _vertices.Add(v.id, v);
        }
        private void _AddVertexWithIncomingEdges(Vertex v, List<Edge> le)
        {
            foreach(Edge e in le)
                v.AddReverseOutgoingEdge(e);
            _vertices.Add(v.id, v);
        }
    }


    internal class VertexTest
    {
        public int id { get; }
        public List<EdgeTest> outgoingEdges { get; }
        public List<EdgeTest> reverseOutgoingEdges { get; }

        public VertexTest(int pid)
        {
            id = pid;
        }

        public void AddOutgoingEdge(EdgeTest edge)
        {
            outgoingEdges.Add(edge);
        }
        public void AddReverseOutgoingEdge(EdgeTest edge)
        {
            reverseOutgoingEdges.Add(edge);
        }
    }

    internal class EdgeTest
    {
        public VertexTest sourceVertex { get; set; }
        public VertexTest targetVertex { get; set; }
        public double costS { get; }

        public EdgeTest(double cost) {
            costS = cost;
        }
    }
}
