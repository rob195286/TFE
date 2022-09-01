﻿using CsvHelper;
using CsvHelper.Configuration.Attributes;
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
            Graph g = new Graph();
            Console.WriteLine("fin du graph");
            var d = new Dijkstra(g);

            sw.Start();
            //  dijkstra(d, idNodeSource, idNodeTarget, false);
             dijkstra(d, idNodeSource, idNodeTarget, true);
            sw.Stop();
            Console.WriteLine("temps : " + sw.ElapsedMilliseconds + " ms");
            Console.WriteLine("");
            //  LaunchDijkstraBenchmart(g);
        }

        static void dijkstra(Dijkstra d, int idNodeSource, int idNodeTarget, bool withCrowFliesOption = false)
        {
            var r = d.ComputeShortestPathWithHeuristic(idNodeSource, idNodeTarget);

            int i = 0;
            State state = r.Value;
            List<Vertex> verticesPath = new List<Vertex>();

            state = r.Value;
            while (state != null)
            {
                verticesPath.Add(state.vertex);
                state = state.previousState;
                i++;
            }

            verticesPath.Reverse();
            int j = 1;
            foreach (Vertex v in verticesPath)
            {
                Console.Write(j++);
                Console.Write(" vertex id : " + v.id);
                Console.WriteLine();
            }

            Console.WriteLine("------------------");
            Console.WriteLine("i : " + i);
            Console.WriteLine("totalCost : " + r.Key);
            Console.WriteLine("costOnly : " + r.Value.costOnly);
            Console.WriteLine("nbr de noeud pris de la queue : " + d.tookNodeNumber);
            Console.WriteLine("nbr total de noeud pri : " + d.totalNumberOfnodes);
        }
        static void LaunchDijkstraBenchmart(Graph g)
        {
            List<int> sourceNodes = new List<int>();
            List<int> targetNodes = new List<int>();
            FindRoutablePoint(g, sourceNodes, targetNodes);
            int i = 0;
            int iteration = 5;
            /*
            for(i = 0; i < iteration; i++)
            DijkstraBenchmark(g, sourceNodes, targetNodes, 100);
            for(i = 0; i < iteration; i++)
            DijkstraBenchmark(g, sourceNodes, targetNodes, 250);
            for (i = 0; i < iteration; i++)
            DijkstraBenchmark(g, sourceNodes, targetNodes, 500);
            for(i = 0; i < iteration; i++)*/
            DijkstraBenchmark(g, sourceNodes, targetNodes, 1000);               
            //for (i = 0; i < iteration; i++)
                DijkstraBenchmark(g, sourceNodes, targetNodes, 2500);
            
            Console.WriteLine("------------------------------------");
            /*
            for (i = 0; i < iteration; i++)
            DijkstraBenchmark(g, sourceNodes, targetNodes, 100, true);
             for (i = 0; i < iteration; i++)
                 DijkstraBenchmark(g, sourceNodes, targetNodes, 250, true);
             for(i = 0; i < iteration; i++)
             DijkstraBenchmark(g, sourceNodes, targetNodes, 500, true);
             for(i = 0; i < iteration; i++)*/
             DijkstraBenchmark(g, sourceNodes, targetNodes, 1000, true);
             //for (i = 0; i < iteration; i++)
                 DijkstraBenchmark(g, sourceNodes, targetNodes, 2500, true);
        }
        static void DijkstraBenchmark(Graph graph, List<int> coordSource, List<int> coordTarget, int numberOfRoutage, bool crowFliesActivate = false)
        {
            Stopwatch sw = new Stopwatch();
            Dijkstra dj = new Dijkstra(graph);
            string fileName = crowFliesActivate ? "Dijkstra_performances_with_heuristic.txt" : "Dijkstra_performances.txt";
            using (StreamWriter stream = File.AppendText(@"A:\3)_Bibliotheque\Documents\Ecam\Anne5\TFE\banchmark\" + fileName))
            {
                for (int iteration = 0; iteration < 1; iteration++)
                {
                    sw.Reset();
                    sw.Start();
                    for (int routage = 0; routage <= numberOfRoutage - 1; routage++)
                    {
                        int idsource = coordSource[routage];
                        int idTarget = coordTarget[routage];
                        if (crowFliesActivate)
                        { dj.ComputeShortestPathWithHeuristic(idsource, idTarget);  }
                        else
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
                    sourceNodes.Add(Convert.ToInt32(x[0]));
                    targetNodes.Add(Convert.ToInt32(x[1]));                 
                }
            }
        }
    }
}
