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


        static void Main(string[] args)
        {
            var bleh = File.ReadAllLines("PageRank_04.txt");
            var parseNodes = bleh.Select(line => line.Split(new[] {"NodeName = "}, StringSplitOptions.None)).ToArray(); // grab the nodes
            var parseEdges = bleh.Select(line => line.Split(new[] {"EdgeName = "}, StringSplitOptions.None)).ToArray(); // grab the edges
            List<Node> nodes = new List<Node>();
            List<Edge> edges = new List<Edge>();
           

            foreach (string[] s in parseNodes)
            {
               if(s.Length != 2) // length of 2 means we are dealing with a node, since that's what i split on.
                   continue;
                string name = s[1]; // grab the name
                Node n = new Node(name); // create node
                nodes.Add(n);
            }
          

            foreach (string[] s in parseEdges) // exact same thing but with edges.
            {
                if(s.Length != 2)
                    continue;
               string[] edge = s[1].Replace("->", " ").Split(' '); // "N8->N1" gets replaced with "N8 N1" and then split on that space. 
                string parentEdgeName = edge[0]; // grabe StartnodeName
                string childEdgeName = edge[1]; // grab toNodeName

                // Find the node object corresponding to those NodeNames.
                Node tempParent = nodes.Find(node => node.NodeName == parentEdgeName);
                Node tempChild = nodes.Find(node => node.NodeName == childEdgeName);
                tempParent.AddEdge(tempChild);
                Edge tempEdge = new Edge(tempParent,tempChild); // then create an edge once nodes are found.
                edges.Add(tempEdge);
            }

            var nSet = new NodeSet(nodes);
            var eSet = new EdgeSet(edges);


            Graph graph = new Graph(eSet, nSet);
            graph.CreateAdjMatrix();
            //nSet.Describe();
            //eSet.Describe();
            
            
        }

        
    }
}
