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
        Dictionary<int, bool> rowEmptiness = new Dictionary<int, bool>();


        public Graph(Node[] nset)
        {
            AllNodes = nset;
            nodeNames = AllNodes.Select(node => node.NodeName).ToArray();
        }

        public string[,] CreateAdjMatrix(out double[,] matrixDouble)
        {
            string[,] hMatrix = new string[nodeNames.Length + 1, nodeNames.Length + 1];
                // extra row column cause row 0 is names going across and column 0 is names going down
            double[,] dubMatrix = new double[nodeNames.Length + 1, nodeNames.Length + 1];

            for (int i = 0; i <= nodeNames.Length; i++)
            {
                List<Node> children = null;
                int j = 0;
                int k = 0; // used for dub matrix.

                if (i == 0) // if we're on [0,0] just put a blank space in that square.
                {
                    hMatrix[i, j] = "  ";
                    dubMatrix[i, j] = 0;
                }
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
                        dubMatrix[i, j] = Array.IndexOf(AllNodes, n) + 1;
                    }
                    else // else populate the parent's row mapped with it's children.
                    {
                        string fraction = "   1/" + children.Count; // get each nods weight in terms of it's simbling
                        string chance = children.Any(d => d.NodeName == n.NodeName) ? fraction : "     0";
                        hMatrix[i, j] = chance;

                        double frac = (1.0/children.Count);
                        frac = Math.Round(frac, 1, MidpointRounding.AwayFromZero);
                        double chan = children.Any(d => d.NodeName == n.NodeName) ? frac : 0;
                        dubMatrix[i, k + 1] = chan;
                    }
                    if (k == 0)
                        dubMatrix[i, k] = i;


                    j++;
                    k++;
                }
            }
            matrixDouble = dubMatrix;
            return hMatrix;
        }

        public void PrintMatrix(string[,] matrix)
        {
            int length = (int) Math.Sqrt(matrix.Length);
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    if (i == 0)
                        Console.Write(matrix[i, j] + "    ");
                    else
                        Console.Write(matrix[i, j]);
                }
                Console.WriteLine("\r\n");
            }
        }

        public void PrintMatrix(double[,] matrix)
        {
            int length = (int) Math.Sqrt(matrix.Length);
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    if(i == 0)
                        Console.Write(matrix[i,j] + "      ");
                    else
                        Console.Write(matrix[i,j] + "  ");
                }
                Console.WriteLine("\r\n");
            }
        }

        public string[,] CreateStochasticMatrix(string[,] matrix)
        {
            
                //dictionary to tell me which row is empty(All 0s) true = empty false = not empty.
            int length = (int) Math.Sqrt(matrix.Length);

            // for each row, find out if that row is empty or not.
            for (int i = 1; i < length; i++)
            {
                rowEmptiness.Add(i, true);
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

        public double[,] CreateGMatrix(double[,] matrix)
        {
            var emptyRows = rowEmptiness.Where(p => p.Value).ToDictionary(p => p.Key, p => p.Value);
            double[,] gMatrix = new double[matrix.GetLength(0),matrix.GetLength(1)];
            for (int i = 0; i < gMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < gMatrix.GetLength(1)-1; j++)
                {
                    if (i == 0)
                    {
                        gMatrix[i, j+1] = matrix[i, j+1];
                        continue;
                    }
                    if (j == 0 && !emptyRows.ContainsKey(i))
                    {
                       gMatrix[i, j] = matrix[i, j];
                        double val = (matrix[i, j + 1]*0.9) + (0.1*(1.0/matrix.GetLength(0)));
                        gMatrix[i, j + 1] = Math.Truncate(1000.0 * val) / 1000.0;
                        continue;
                    }
                    if (j == 0 && emptyRows.ContainsKey(i))
                    {
                        gMatrix[i, j] = matrix[i, j];
                        gMatrix[i, j + 1] = 1.0/(matrix.GetLength(0) - 1);
                        continue;
                    }
                    if (emptyRows.ContainsKey(i))
                        gMatrix[i, j + 1] = 1.0/(matrix.GetLength(0) - 1);
                    else
                    {
                        double val = (matrix[i, j + 1]*0.9) + (0.1*(1.0/matrix.GetLength(0)));
                        double truncated = Math.Truncate(1000.0 * val) / 1000.0;
                        gMatrix[i, j + 1] = truncated;
                    }
                }
            }

            return gMatrix;
        }
        public void pageRank(double[,] B, int iteration) // matrix multiplicationn.
        {

            double[,] A  = new double[1, B.GetLength(0) -1]; // A matrix will be 1 row with a length of the number of nodes. my example[1,10]
            
            // populating A matrix with an equal opportunity to hit each page soo 1/N (n being the number of Nodes)
            for (int i = 0; i < A.GetLength(0); i++) // GetLength(0) get's the row length
            {
                for (int j = 0; j < A.GetLength(1); j++) // GetLength(1) gets the column length.
                {
                    A[i, j] = 1.0/(B.GetLength(0) - 1); // have it as minus 1 because i have an extra row/column for node names.
                }
            }
            
            // Matrix Multiplication rules:
                // Column length of matrix A must equal Row Length of matrix B.
                // Matrix C will be rowA by Column B.

            // grab the row/column length of the matrices i wan't to multiply. 
            int rowA = A.GetLength(0);
            int columnA = A.GetLength(1);
            int rowB = B.GetLength(0);
            int columnB = B.GetLength(1);
            double[,] C = new double[rowA,columnB]; // create the new matrix based on rules above.
            if (columnA != rowB) 
            { // checking to make sure before we multiply.
                Console.WriteLine("Matrix can't be multiplied.");
            }
            else
            {
                while (iteration > 0)
                {
                    for (int i = 0; i < rowA; i++) // iterating across matrix A
                    {
                        for (int j = 0; j < columnB - 1; j++)
                        {
                            double temp = 0;
                            for (int k = 0; k < columnA - 1; k++)
                            {
                                double aVal = A[i, k];
                                double Bval = B[k + 1, j + 1];
                                double tempVal = aVal*Bval;
                                temp += tempVal;
                            }
                            C[i, j+1] = temp;
                        }
                    }
                    double[,] tempIterator = C;
                    A = tempIterator;
                    iteration--;
                }
            }
        }

        public void ConvertToDouble(string[,] matrix)
        {
            int length = (int) Math.Sqrt(matrix.Length);
            double[,] dubMatrix = new double[length - 1, length - 1];

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    dubMatrix[i, j] = Double.Parse(matrix[(i + 1), (j + 1)]);
                }
            }
        }
    }
}