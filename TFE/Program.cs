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
            int idNodeTarget = 458523; // bruge
            //int idNodeTarget = 901419;
            //int idNodeTarget = 597177;

            Stopwatch sw = new Stopwatch();

            Console.WriteLine("debut du graph");
            Graph g = new Graph();
            Console.WriteLine("fin du graph");

            //printRoadNameEquality(g, idNodeSource, idNodeTarget);
            var dijkstra = new Dijkstra(g);
            
            sw.Start();
            var r = dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget);
            Console.WriteLine("------------------");
            Console.WriteLine("costS : " + r.Key);
            Console.WriteLine("nbr de noeud pris de la queue : " + dijkstra.tookNodeNumber);
            Console.WriteLine("nbr total de noeud pri : " + dijkstra.totalNumberOfnodes);
            Console.WriteLine("with crow  : " + false);
            //dijkstra(g, idNodeSource, idNodeTarget);
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
            sw.Reset();
            Console.Write("-------------------------");
            
            sw.Start();
            //dijkstra(g, idNodeSource, idNodeTarget, true);
            var rr = dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget, true);
            Console.WriteLine("------------------");
            Console.WriteLine("costS : " + rr.Key);
            Console.WriteLine("nbr de noeud pris de la queue : " + dijkstra.tookNodeNumber);
            Console.WriteLine("nbr total de noeud pri : " + dijkstra.totalNumberOfnodes);
            Console.WriteLine("with crow: " + true);
            sw.Stop();
            Console.Write("temps : ");
            Console.WriteLine(sw.ElapsedMilliseconds);


           // LaunchDijkstraBenchmart(g);

            /*
            using (TextFieldParser parser = new TextFieldParser(@"A:\3)_Bibliotheque\Documents\Ecam\Anne5\TFE\Code\routablePointFromDB.csv"))
            {
                Dijkstra dij = new Dijkstra(g);
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    var x = parser.ReadFields();                   
                    if (g.NodeExist(Convert.ToInt32(x[0])) && g.NodeExist(Convert.ToInt32(x[1])))
                    {
                        dij.ComputeShortestPath(Convert.ToInt32(x[0]), Convert.ToInt32(x[1]));
                    }                    
                }
                Console.WriteLine("finnnnn");
            }*/
        }

        static void dijkstra(Graph g, int idNodeSource, int idNodeTarget, bool withCrowFliesOption = false)
        {
            var dijkstra = new Dijkstra(g);
            var r = dijkstra.ComputeShortestPath(idNodeSource, idNodeTarget, withCrowFliesOption);
            
            int i = 0;
            PriorityQueueNode state = r.Value;
            List<KeyValuePair<GraphNode, string>> id_RoadName = new List<KeyValuePair<GraphNode, string>>();
            
            state = r.Value;
            while (state != null)
            {
                id_RoadName.Add(new KeyValuePair<GraphNode, string>(state.graphNode, state.roadName));
                state = state.previousState;
                i++;
            }
            
            /*
            id_RoadName.Reverse();
            foreach (KeyValuePair<GraphNode, string> nodeNroadName in id_RoadName)
            {
                Console.Write(" graphNode id : " + nodeNroadName.Key.id);
                Console.Write("    road name : " + nodeNroadName.Value);
                Console.WriteLine();
            }
            
            */
            Console.WriteLine("------------------");
            Console.WriteLine("i : " + i);
            Console.WriteLine("costS : " + r.Key);
            Console.WriteLine("nbr de noeud pris de la queue : " + dijkstra.tookNodeNumber);
            Console.WriteLine("nbr total de noeud pri : " + dijkstra.totalNumberOfnodes);
        }
        static void LaunchDijkstraBenchmart(Graph g)
        {
            Stopwatch sw = new Stopwatch();
            List<int> sourceNodes = new List<int>();
            List<int> targetNodes = new List<int>();
            FindRoutablePoint(g, sourceNodes, targetNodes);

            sw.Start();
            DijkstraBenchmark(g, sourceNodes, targetNodes, 50);

            DijkstraBenchmark(g, sourceNodes, targetNodes, 100);

            DijkstraBenchmark(g, sourceNodes, targetNodes, 250);

            DijkstraBenchmark(g, sourceNodes, targetNodes, 500);
            //DijkstraBenchmark(g, sourceNodes, targetNodes, 1000);
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds / 1000);

            sw.Reset();

            sw.Start();
            DijkstraBenchmark(g, sourceNodes, targetNodes, 50, true);

            DijkstraBenchmark(g, sourceNodes, targetNodes, 100, true);

            DijkstraBenchmark(g, sourceNodes, targetNodes, 250, true);

            DijkstraBenchmark(g, sourceNodes, targetNodes, 500, true);
            //DijkstraBenchmark(g, sourceNodes, targetNodes, 1000, true);
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds / 1000);
        }
        static void DijkstraBenchmark(Graph graph, List<int> coordSource, List<int> coordTarget, int numberOfRoutage, bool crowFliesActivate =false)
        {
            Console.WriteLine("Benchmarkt !!");
            Stopwatch sw = new Stopwatch();
            Dijkstra dj = new Dijkstra(graph);
            Random myRand = new Random();
            string fileName = crowFliesActivate ? "Dijkstra_performances_with_crow.txt" : "Dijkstra_performances.txt";
            using (StreamWriter stream = File.AppendText(@"A:\3)_Bibliotheque\Documents\Ecam\Anne5\TFE\banchmark\"+ fileName))
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
                        dj.ComputeShortestPath(idsource, idTarget, crowFliesActivate);
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
                    if(graph.NodeExist(Convert.ToInt32(x[1])))
                    {
                        sourceNodes.Add(Convert.ToInt32(x[1]));
                    }
                    if(graph.NodeExist(Convert.ToInt32(x[2])))
                    {
                        targetNodes.Add(Convert.ToInt32(x[2]));
                    }
                }
            }
        }
        /*
        static void printRoadNameEquality(Graph g, int idNodeSource, int idNodeTarget, string pathFile = "path.csv")
        {
            var r = new Dijkstra(g).ComputeShortestPath(idNodeSource, idNodeTarget);
            PriorityQueueNode priorityQueueNode = r.Value;
            int i = 0;
            List<KeyValuePair<int, int>> nid = new List<KeyValuePair<int, int>>();

            priorityQueueNode = r.Value;
            while (true)
            {
                nid.Add(new KeyValuePair<int, int>(priorityQueueNode.graphNode.id, priorityQueueNode.tag));
                priorityQueueNode = priorityQueueNode.previousState;
                i++;
                if (priorityQueueNode.previousState == null)
                {
                    nid.Add(new KeyValuePair<int, int>(priorityQueueNode.graphNode.id, priorityQueueNode.tag));
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
        */
    }
}
