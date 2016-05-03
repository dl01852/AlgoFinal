using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoFinal
{
    class Edge
    {
        public Node ParentNode { get; set; }
        public Node ChildNode { get; set; }
        public string EdgeName { get; set; }

        public Edge(Node parent, Node endNode)
        {
            ParentNode = parent;
            ChildNode = endNode;
            EdgeName = ParentNode + "->" + ChildNode;
        }

        public Edge()
        {
            
        }
        public override string ToString()
        {
            return EdgeName;
        }
    }
}
