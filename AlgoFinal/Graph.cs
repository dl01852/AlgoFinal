using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoFinal
{
    class Graph
    {
        public Node root;
        public Node[] AllNodes;
        public NodeSet nodeSet;
        private string[] nodeNames;


        public Graph(EdgeSet eset, NodeSet nset)
        {
            AllNodes = nset.getAllNodes();
            nodeNames = AllNodes.Select(node => node.NodeName).ToArray();
            nodeSet = nset;
        }
        
        public Node CreateRoot(string name)
        {
            root = CreateNode(name);
            return root;
        }

        public Node CreateNode(string name)
        {
            var n = new Node(name);
            //AllNodes.Add(n);
            return n;
        }


        public void CreateAdjMatrix()
        {
            
            Console.Write("      "); // 6 spaces
            for (int i = 0; i < nodeNames.Length; i++)
            {
                Console.Write(nodeNames[i]+"      "); // 4 spaces
            }
            Console.WriteLine();

            for (int i = 0; i < nodeNames.Length; i++)
            {
                Node parent = AllNodes[i];
                List<Node> children = parent.children;
                Console.Write(parent.NodeName);
                bool stop = false;

                foreach (Node n in AllNodes)
                {
                    Console.Write(children.Any(d => d.NodeName == n.NodeName)? "    X" : "    0");
                }
                
                    //foreach (string t in nodeNames)
                    //{
                        
                    //    if (c.NodeName == t)
                    //    {
                    //        Console.Write("    X");
                    //        stop = false;
                    //        break;
                    //    }
                    //     if(!stop)
                    //       Console.Write("    0");
                    //}
                    //stop = true;
                      
                Console.WriteLine("\n");
            }
          
        }

        public void PrintMatrix(ref int?[,] matrix)
        {
            int count = AllNodes.Length;
            Console.WriteLine("     ");
            for (int i = 0; i < count; i++)
            {
                Console.Write("{0}  ",AllNodes[i]);
            }
            Console.WriteLine();

            for (int i = 0; i < count; i++)
            {
                Console.Write("{0} [",AllNodes[i]);

                for (int j = 0; j < count; j++)
                {
                    if (i == j)
                    {
                        Console.Write("0, ");
                    }
                    else if (matrix[i, j] == null)
                    {
                        Console.Write("-1, ");
                    }
                    else
                    {
                        Console.Write("{0}, ", matrix[i,j]);
                    }
                }
                Console.Write(" ]\r\n");
            }
            Console.Write("\r\n");
        }
    }
}
