using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace TFE
{
    class Program
    {
        static void Main(string[] args)
        {
            //int idNodeSource = 121155;
            int idNodeSource = 589961; // arlon
            //int idNodeTarget = 901419;
            int idNodeTarget = 458523; // bruge
            //int idNodeTarget = 597177;
            Stopwatch sw = new Stopwatch();

            Console.WriteLine("debut du graph");
            Graph g = new Graph();
            Console.WriteLine("fin du graph");

            //printRoadNameEquality(g, idNodeSource, idNodeTarget);
            
            sw.Start();
            dijkstra(g, idNodeSource, idNodeTarget);
            sw.Stop();
            Console.Write("temps : ");
            Console.WriteLine(sw.ElapsedMilliseconds);
            /*
            List<int> sourceNodes = new List<int>();
            List<int> targetNodes = new List<int>();
            using (TextFieldParser parser = new TextFieldParser(@"routablePointFromDB.csv"))
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
                    sourceNodes.Add(Convert.ToInt32(x[0]));
                    targetNodes.Add(Convert.ToInt32(x[1]));
                }
            }
            
            sw.Start();
            DijkstraBenchmark(g, sourceNodes, targetNodes, 50);

            DijkstraBenchmark(g, sourceNodes, targetNodes, 100);

            DijkstraBenchmark(g, sourceNodes, targetNodes, 250);

            DijkstraBenchmark(g, sourceNodes, targetNodes, 500);
            DijkstraBenchmark(g, sourceNodes, targetNodes, 1000);
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds/1000);
            */
        }

        static void dijkstra(Graph g, int idNodeSource, int idNodeTarget)
        {
            var r = new Dijkstra(g).ComputeShortestPath(idNodeSource, idNodeTarget);
            
            int i = 0;
            State state = r.Value;
            List<KeyValuePair<Node, string>> id_RoadName = new List<KeyValuePair<Node, string>>();
            
            state = r.Value;
            while (state != null)
            {
                //id_RoadName.Add(new KeyValuePair<Node, string>(state.node, state.roadName));
                state = state.previousState;
                i++;
            }
            /*
            id_RoadName.Reverse();
            foreach (KeyValuePair<Node, string> nodeNroadName in id_RoadName)
            {
                Console.Write(" node id : " + nodeNroadName.Key.id);
                Console.Write("    road name : " + nodeNroadName.Value);
                Console.WriteLine();
            }
            */
            Console.WriteLine("------------------");
            Console.WriteLine("i : " + i);
            Console.WriteLine("cost : " + r.Key);
        }
        static void DijkstraBenchmark(Graph graph, List<int> coordSource, List<int> coordTarget, int numberOfRoutage)
        {
            Stopwatch sw = new Stopwatch();
            Dijkstra dj = new Dijkstra(graph);
            Random myRand = new Random();
            using (StreamWriter stream = File.AppendText(@"A:\3)_Bibliotheque\Documents\Ecam\Anne5\TFE\stage\banchmark\Dijkstra_performances.txt"))
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
                    stream.WriteLine("}");
                }
                stream.Close();
            }
            Console.WriteLine("fin du benchmark! : " + numberOfRoutage);
        }
        static void printRoadNameEquality(Graph g, int idNodeSource, int idNodeTarget, string pathFile = "path.csv")
        {
            var r = new Dijkstra(g).ComputeShortestPath(idNodeSource, idNodeTarget);
            State state = r.Value;
            int i = 0;
            List<KeyValuePair<int, int>> nid = new List<KeyValuePair<int, int>>();

            state = r.Value;
            while (true)
            {
                nid.Add(new KeyValuePair<int, int>(state.node.id, state.tag));
                state = state.previousState;
                i++;
                if (state.previousState == null)
                {
                    nid.Add(new KeyValuePair<int, int>(state.node.id, state.tag));
                    i++;
                    break;
                }
            }
            nid.Reverse();
            using (TextFieldParser parser = new TextFieldParser(@pathFile))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                bool isOk = true;
                int j = 0;
                bool flag = true;
                while (!parser.EndOfData)
                {
                    if (j >= nid.Count)
                    {
                        isOk = false;
                        break;
                    }
                    if (flag)
                    {
                        var kkkk = parser.ReadLine();
                        flag = false;
                        continue;
                    }

                    int csvid = Convert.ToInt32(parser.ReadLine());
                    var keyval = nid[j++];
                    int idindex = keyval.Key;
                    Console.Write("csvid : " + csvid + " id : " + idindex + " -> == ");
                    Console.Write(csvid == idindex);
                    if (csvid != idindex)
                    {
                        isOk = false;
                        Console.WriteLine(" //////////////////////////////////");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine(isOk);
            }
        }
    }
}
