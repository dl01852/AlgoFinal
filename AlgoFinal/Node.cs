using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoFinal
{
    class Node
    {
        public string NodeName { get; set; }
        public List<Edge> edges = new List<Edge>();
        public List<Node> children = new List<Node>(); 
         
        public int count = 0;
        public Node(string nodeName)
        {
            NodeName = nodeName;
        }


        public Node AddEdge(Node child)
        {
            edges.Add(new Edge
            {
                ParentNode = this,
                ChildNode = child
            });

            children.Add(child);
            count++;
            return this;
        }

        public override string ToString()
        {
            return NodeName;
        }
    }
}
