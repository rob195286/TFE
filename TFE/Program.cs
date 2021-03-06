using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace TFE
{
    class Program
    {
        static void Main(string[] args)
        {
            int idNodeSource = 589961; // arlon
            int idNodeTarget = 458523; // bruge
            Stopwatch sw = new Stopwatch();

            Console.WriteLine("debut du graph");
            sw.Start();
            Graph g = new Graph();
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
            Console.WriteLine("fin du graph");

            Dijkstra dijkstra = new Dijkstra(g);
            Console.WriteLine("------------------------------------------------------------------");
            //CompareVertexId(dijkstra, idNodeSource, idNodeTarget);
            sw.Start();
            Dijkstra(dijkstra, idNodeSource, idNodeTarget);
            sw.Stop();
            Console.Write("temps : ");
            Console.WriteLine(sw.ElapsedMilliseconds);
            /*
            LaunchDijkstraBenchmart(g, 50);
            LaunchDijkstraBenchmart(g, 100);
            LaunchDijkstraBenchmart(g, 250);
            LaunchDijkstraBenchmart(g, 500);
            LaunchDijkstraBenchmart(g, 1000);*/
        }

        static void Dijkstra(Dijkstra d, int idNodeSource, int idNodeTarget)
        {
            var r = d.ComputeShortestPath(idNodeSource, idNodeTarget);

            int i = 0;
            State state = r.Value;
            List<KeyValuePair<Vertex, double>> vertexWithCostPAth = new List<KeyValuePair<Vertex, double>>();

            state = r.Value;
            while (state != null)
            {
                vertexWithCostPAth.Add(new KeyValuePair<Vertex, double>(state.vertex, state.totalCost));
                state = state.previousState;
                i++;
            }
            /*
            int j = 1;
            vertexWithCostPAth.Reverse();
            foreach (KeyValuePair<Vertex, double> nodeNroadName in vertexWithCostPAth)
            {
                Console.Write("" + j++);
                Console.Write(" vertex id : " + nodeNroadName.Key.id);
                //Console.Write(" total cost : " + nodeNroadName.Value);
                //Console.Write("    road name : " + nodeNroadName.Value);
                Console.WriteLine();
            }
            */
            Console.WriteLine("------------------");
            Console.WriteLine("i : " + i);
            Console.WriteLine("cost : " + r.Key);
            Console.WriteLine("tookNodeNumber : " + d.tookNodeNumber);
            Console.WriteLine("totalNumberOfnodes : " + d.totalNumberOfnodes);
            //Console.WriteLine("queueNode : " + d.queueNode);
        }
        static void LaunchDijkstraBenchmart(Graph g, int routageNumber)
        {
            List<int> sourceNodes = new List<int>();
            List<int> targetNodes = new List<int>();
            FindRoutablePoint(g, sourceNodes, targetNodes);

            DijkstraBenchmark(g, sourceNodes, targetNodes, routageNumber);

            DijkstraBenchmark(g, sourceNodes, targetNodes, routageNumber);

            DijkstraBenchmark(g, sourceNodes, targetNodes, routageNumber);

            DijkstraBenchmark(g, sourceNodes, targetNodes, routageNumber);

            DijkstraBenchmark(g, sourceNodes, targetNodes, routageNumber);
        }
        static void DijkstraBenchmark(Graph graph, List<int> coordSource, List<int> coordTarget, int numberOfRoutage)
        {
            Console.WriteLine("Benchmarkt !!");
            Stopwatch sw = new Stopwatch();
            Dijkstra dj = new Dijkstra(graph);
            Random myRand = new Random();
            using (StreamWriter stream = File.AppendText(@"A:\3)_Bibliotheque\Documents\Ecam\Anne5\TFE\banchmark\BiDir_Dijkstra_performances.txt"))
            {
                for (int iteration = 0; iteration < 1; iteration++)
                {
                    sw.Reset();
                    sw.Start();
                    for (int routage = 0; routage <= numberOfRoutage - 1; routage++)
                    {
                        //int randomInt = (int)(myRand.NextDouble()*numberOfRoutage);
                        int idsource = coordSource[routage];
                        int idTarget = coordTarget[routage];
                        dj.ComputeShortestPath(idsource, idTarget);
                    }
                    sw.Stop();
                    stream.WriteLine("{");
                    stream.WriteLine(string.Format("    temps de calcul ms/s : {0:F10} ms -> {1:F10} s", sw.ElapsedMilliseconds, sw.ElapsedMilliseconds / 1000));
                    stream.WriteLine("    nombre de routage : " + numberOfRoutage);
                    stream.WriteLine("    nombre de noeud total prit : " + dj.totalNumberOfnodes);
                    stream.WriteLine("    nombre de noeud prit de la pq : " + dj.tookNodeNumber);
                    stream.WriteLine("}");
                }
                stream.Close();
            }
            Console.WriteLine("fin du benchmark! : " + numberOfRoutage);
        }
        static void FindRoutablePoint(Graph graph, List<int> sourceNodes, List<int> targetNodes)
        {
            using (TextFieldParser parser = new TextFieldParser(@"A:\3)_Bibliotheque\Documents\Ecam\Anne5\TFE\Code\routablePointFromDB.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                bool flag = true;
                while (!parser.EndOfData)
                {
                    var x = parser.ReadFields();
                    if (flag)
                    {
                        flag = false;
                        continue;
                    }
                    if (graph.VertexExist(Convert.ToInt32(x[1])))
                    {
                        sourceNodes.Add(Convert.ToInt32(x[1]));
                    }
                    if (graph.VertexExist(Convert.ToInt32(x[2])))
                    {
                        targetNodes.Add(Convert.ToInt32(x[2]));
                    }
                }
            }
        }
        static void CompareVertexId(Dijkstra d, int idNodeSource, int idNodeTarget,
            string pathFile = @"A:\3)_Bibliotheque\Documents\Ecam\Anne5\TFE\Code\path.csv")
        {
            var r = d.ComputeShortestPath(idNodeSource, idNodeTarget);
            State state = r.Value;
            List<KeyValuePair<int, double>> listVid = new List<KeyValuePair<int, double>>();

            while (state != null)
            {
                listVid.Add(new KeyValuePair<int, double>(state.vertex.id, state.totalCost));
                state = state.previousState;
            }
            listVid.Reverse();
            using (TextFieldParser parser = new TextFieldParser(@pathFile))
            {
                NumberFormatInfo provider = new NumberFormatInfo();
                provider.NumberDecimalSeparator = ".";
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                int j = 0;
                bool isOk = true;
                bool flag = true;
                while (!parser.EndOfData)
                {
                    var x = parser.ReadFields();
                    if (flag)
                    {
                        flag = false;
                        continue;
                    }
                    int csvid = Convert.ToInt32(x[0]);
                    //var csvid22222 = Convert.ToDouble(x[1], provider);
                    var vID = listVid[j++];
                    if (vID.Key != csvid)
                        isOk = false;
                    Console.Write("csvid : " + csvid + " || id : " + vID.Key + " -> == " + (csvid == vID.Key));
                    //Console.Write("             csvid : " + csvid22222 + "|| id : " + vID.Value + " -> == " + (vID.Value == csvid22222));
                    Console.WriteLine();
                    if (!isOk)
                        Console.WriteLine(" //////////////////////////////////");
                }
                Console.WriteLine("is ok :" + isOk);
            }
        }
    }
}