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
            //idNodeSource = 193183; // p1 bxl
            // idNodeTarget = 778238; // p2 bxl
            idNodeSource = 354192;
            idNodeTarget = 912989;
           // idNodeSource = 354192; // 28
          //  idNodeTarget = 912989;


            Stopwatch sw = new Stopwatch();

            Console.WriteLine("debut du graph");
            Graph g = new Graph();
            Console.WriteLine("fin du graph");

            var d = new Dijkstra(g);

            sw.Start();
           // dijkstra(d, idNodeSource, idNodeTarget, false);
           // dijkstra(d, idNodeSource, idNodeTarget, true);
            sw.Stop();
            Console.WriteLine("temps : " + sw.ElapsedMilliseconds + " ms");
            Console.WriteLine("");
            printRoadNameEquality(g, idNodeSource, idNodeTarget);
            // LaunchDijkstraBenchmart(g);

            //CompareNAndH(g);
        }

        static void dijkstra(Dijkstra d, int idNodeSource, int idNodeTarget, bool withCrowFliesOption = false)
        {
            var r = d.ComputeShortestPath(idNodeSource, idNodeTarget, withCrowFliesOption);

            int i = 0;
            State state = r.Value;
            List<KeyValuePair<Vertex, string>> id_RoadName = new List<KeyValuePair<Vertex, string>>();

            state = r.Value;
            while (state != null)
            {
                id_RoadName.Add(new KeyValuePair<Vertex, string>(state.vertex, ""));
                state = state.previousState;
                i++;
            }
            
            id_RoadName.Reverse();
            int j = 1;
            foreach (KeyValuePair<Vertex, string> nodeNroadName in id_RoadName)
            {
                Console.Write(j++);
                Console.Write(" vertex id : " + nodeNroadName.Key.id);
                //Console.Write("    road name : " + nodeNroadName.Value);
                Console.WriteLine();
            }            
            
            Console.WriteLine("------------------");
            Console.WriteLine("i : " + i);
            Console.WriteLine("totalCost : " + r.Key);
            Console.WriteLine("costOnly : " + r.Value.costOnly);
            //Console.WriteLine("nbr de noeud pris de la queue : " + d.tookNodeNumber);
           // Console.WriteLine("nbr total de noeud pri : " + d.totalNumberOfnodes);
        }
        static void LaunchDijkstraBenchmart(Graph g)
        {
            Stopwatch sw = new Stopwatch();
            List<int> sourceNodes = new List<int>();
            List<int> targetNodes = new List<int>();
            FindRoutablePoint(g, sourceNodes, targetNodes);
            int i = 0;
            sw.Start();
            /*           
            DijkstraBenchmark(g, sourceNodes, targetNodes, 250);
            for(i = 0; i < 5; i++)
            DijkstraBenchmark(g, sourceNodes, targetNodes, 500);
            for(i = 0; i < 5; i++)
            DijkstraBenchmark(g, sourceNodes, targetNodes, 1000); 
            */
            for (i = 0; i < 5; i++)
                DijkstraBenchmark(g, sourceNodes, targetNodes, 2433);
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds / 1000);

            sw.Reset();

            sw.Start();
            /*
            for(i = 0; i < 5; i++)
            DijkstraBenchmark(g, sourceNodes, targetNodes, 250, true);
            for(i = 0; i < 5; i++)
            DijkstraBenchmark(g, sourceNodes, targetNodes, 500, true);
            for(i = 0; i < 5; i++)
            DijkstraBenchmark(g, sourceNodes, targetNodes, 1000, true);
            */
            for (i = 0; i < 5; i++)
                DijkstraBenchmark(g, sourceNodes, targetNodes, 2433, true);
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds / 1000);
        }
        static void DijkstraBenchmark(Graph graph, List<int> coordSource, List<int> coordTarget, int numberOfRoutage, bool crowFliesActivate = false)
        {
            Console.WriteLine("Benchmarkt !!");
            Stopwatch sw = new Stopwatch();
            Dijkstra dj = new Dijkstra(graph);
            Random myRand = new Random();
            string fileName = crowFliesActivate ? "Dijkstra_performances_with_heuristic.txt" : "Dijkstra_performances.txt";
            using (StreamWriter stream = File.AppendText(@"A:\3)_Bibliotheque\Documents\Ecam\Anne5\TFE\banchmark\" + fileName))
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
                    if (graph.NodeExist(Convert.ToInt32(x[1])))
                    {
                        sourceNodes.Add(Convert.ToInt32(x[1]));
                    }
                    if (graph.NodeExist(Convert.ToInt32(x[2])))
                    {
                        targetNodes.Add(Convert.ToInt32(x[2]));
                    }
                }
            }
        }
        static void CompareNAndH(Graph g)
        {
            bool isOk = true;
            bool isOktmp = true;
            Dijkstra dj = new Dijkstra(g);
            List<int> sourceNodes = new List<int>();
            List<int> targetNodes = new List<int>();
            FindRoutablePoint(g, sourceNodes, targetNodes);
            int error = 0;
            for (int routage = 0; routage < 2330; routage++)
            {
                int idsource = sourceNodes[routage];
                int idTarget = targetNodes[routage];
                isOktmp = dj.ComputeShortestPath(idsource, idTarget).Key == dj.ComputeShortestPath(idsource, idTarget, true).Key;
                if (!isOktmp)
                {
                    Console.WriteLine("routage N° : " + routage);
                    Console.WriteLine("sourceNodes : " + sourceNodes[routage] + " || " + "targetNodes : " + targetNodes[routage]);
                    Console.WriteLine();
                    isOk = false;
                    error++;
                }
            }
            Console.WriteLine("error : "+error);
            Console.WriteLine(isOk);
        }

        static void printRoadNameEquality(Graph g, int idNodeSource, int idNodeTarget, string pathFile = @"A:\3)_Bibliotheque\Documents\Ecam\Anne5\TFE\Code\path.csv")
        {
            var r = new Dijkstra(g).ComputeShortestPath(idNodeSource, idNodeTarget);
            State state = r.Value;
            int i = 0;
            List<int> idL = new List<int>();

            state = r.Value;
            while (true)
            {
                idL.Add(state.vertex.id);
                state = state.previousState;
                i++;
                if (state.previousState == null)
                {
                    idL.Add(state.vertex.id);
                    i++;
                    break;
                }
            }
            idL.Reverse();
            using (TextFieldParser parser = new TextFieldParser(pathFile))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                parser.ReadLine();
                bool isOk = true;
                int j = 0;
                while (!parser.EndOfData)
                {
                    if (j >= idL.Count)
                    {
                        isOk = false;
                        break;
                    }

                    string s = parser.ReadLine();
                    int csvid = Convert.ToInt32(s);
                    int idindex = idL[j++];
                    Console.Write("j+1 : "+j+ "    csvid : " + csvid + " id : " + idindex + " -> == ");
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
