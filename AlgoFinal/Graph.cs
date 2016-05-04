using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace AlgoFinal
{
    class Graph
    {
        public Node root;
        public Node[] AllNodes;
        private string[] nodeNames;


        public Graph(Node[] nset)
        {
            AllNodes = nset;
            nodeNames = AllNodes.Select(node => node.NodeName).ToArray();
        }
        public string[,] CreateAdjMatrix()
        {
            string[,] hMatrix = new string[nodeNames.Length + 1, nodeNames.Length + 1];// extra row column cause row 0 is names going across and column 0 is names going down
        

            for (int i = 0; i <= nodeNames.Length; i++)
            {
                List<Node> children = null;
                int j = 0;

                if (i == 0) // if we're on [0,0] just put a blank space in that square.
                    hMatrix[i, j] = "  ";
                else
                { 
                    // i for the 2D Array stores data starting at index 1 but grabs the data starting at 0th index.(Example: multi[1(i),0] needs to store data for  AllNodes[0(i-1)] )
                    hMatrix[i, j] = nodeNames[i - 1];
                    Node parent = AllNodes[i - 1];
                    children = parent.children;
                }

                j++; // increment j to go to the next column.

                foreach (Node n in AllNodes) // loop through all Nodes.
                {
                    if (i == 0) // populate the entire 0th row ([0,1], [0,2] , [0,3] etc...) with Nodenames.
                    {
                        hMatrix[i, j] = n.NodeName;
                        j++;
                    }
                    else // else populate the parent's row mapped with it's children.
                    {
                        string fraction = "   1/" + children.Count; // get each nods weight in terms of it's simbling
                        string chance = children.Any(d => d.NodeName == n.NodeName) ? fraction : "     0";
                        hMatrix[i, j] = chance;
                        j++;
                    }
                }
            }

            return hMatrix;
        }

        public void PrintMatrix(string[,] matrix)
        {
           
            int length = (int)Math.Sqrt(matrix.Length);
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    if(i == 0)
                        Console.Write(matrix[i,j] + "    ");
                    else
                        Console.Write(matrix[i,j]);
                    
                }
                Console.WriteLine("\r\n");
            }
        }

        public string[,] CreateStochasticMatrix(string[,] matrix)
        {
            Dictionary<int,bool> rowEmptiness = new Dictionary<int, bool>(); //dictionary to tell me which row is empty(All 0s) true = empty false = not empty.
            int length = (int) Math.Sqrt(matrix.Length);

            // for each row, find out if that row is empty or not.
            for (int i = 1; i < length; i++)
            {
                rowEmptiness.Add(i,true);
                for (int j = 1; j < length; j++)
                {
                    if (!matrix[i, j].Contains("0"))
                    {
                        rowEmptiness[i] = false;
                        break;
                    }
                }
            }

            // convert all the rows that are empty to an array.
           var emptyRows = rowEmptiness.Where(p => p.Value).ToArray();

            // grab the row(row.key) and set and distribute they're chance equally.
            foreach (int row in emptyRows.Select(rows => rows.Key))
            {
                for (int j = 1; j < length; j++)
                {
                    matrix[row, j] = "  1/" + (length - 1);
                }
            }

            return matrix;
        }
        
    }
}