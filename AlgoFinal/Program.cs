using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace AlgoFinal
{
    class Program
    {
       
       static List<Node> nodes = new List<Node>();
       static List<Edge> edges = new List<Edge>();
       static NodeSet nSet = new NodeSet(nodes);
       static EdgeSet eSet = new EdgeSet(edges);
       static Dictionary<string,double> pageRank = new Dictionary<string, double>(); 

        static void Main(string[] args)
        {

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("CSCI 5330 Spring 2016");
            sb.AppendLine("David Lewis");
            sb.AppendLine("900732205");
            sb.AppendLine("Graph 03");
            
            ParseFile();
            Graph graph = new Graph(nSet.getAllNodes());
            Console.WriteLine(sb.ToString());
            nSet.Describe();
            eSet.Describe();
            double[,] matrixDouble;
           var matrix = graph.CreateAdjMatrix(out matrixDouble);
            Console.WriteLine("---------H Matrix-----------");
            graph.PrintMatrix(matrix);
            
            Console.WriteLine("---------Stochastic Matrix-----------");
            matrix = graph.CreateStochasticMatrix(matrix);
            graph.PrintMatrix(matrix);
           var gmatrix = graph.CreateGMatrix(matrixDouble);
            Console.WriteLine("---------G Matrix-----------");
            graph.PrintMatrix(gmatrix);
           var rank = graph.pageRank(matrixDouble, 4);

            for (int i = 0; i < rank.GetLength(0); i++)
            {
                
                for (int j = 1; j < rank.GetLength(1); j++)
                {
                    pageRank.Add("N"+j,rank[i,j]);
                    
                }
                
            }
            var order = pageRank.OrderByDescending(d => d.Value); // order the page rank.
            Console.WriteLine("-----Page Rank after 4 Iterations---------");
            foreach (KeyValuePair<string, double> keyValuePair in order)
            {
                Console.WriteLine("{0} - {1:0.000}",keyValuePair.Key,keyValuePair.Value);
            }

            Console.Read();
        }


        public static void ParseFile()
        {
            var bleh = File.ReadAllLines("PageRank_04.txt");
            var parseNodes = bleh.Select(line => line.Split(new[] { "NodeName = " }, StringSplitOptions.None)).ToArray(); // grab the nodes
            var parseEdges = bleh.Select(line => line.Split(new[] { "EdgeName = " }, StringSplitOptions.None)).ToArray(); // grab the edges

            foreach (string[] s in parseNodes)
            {
                if (s.Length != 2) // length of 2 means we are dealing with a node, since that's what i split on.
                    continue;
                string name = s[1]; // grab the name
                Node n = new Node(name); // create node
                nodes.Add(n);
            }


            foreach (string[] s in parseEdges) // exact same thing but with edges.
            {
                if (s.Length != 2)
                    continue;
                string[] edge = s[1].Replace("->", " ").Split(' '); // "N8->N1" gets replaced with "N8 N1" and then split on that space. 
                string parentEdgeName = edge[0]; // grabe StartnodeName
                string childEdgeName = edge[1]; // grab toNodeName

                // Find the node object corresponding to those NodeNames.
                Node tempParent = nodes.Find(node => node.NodeName == parentEdgeName);
                Node tempChild = nodes.Find(node => node.NodeName == childEdgeName);
                tempParent.AddEdge(tempChild);
                Edge tempEdge = new Edge(tempParent, tempChild); // then create an edge once nodes are found.
                edges.Add(tempEdge);
            }


        }

    }
}
