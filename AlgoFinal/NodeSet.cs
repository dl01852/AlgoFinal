using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoFinal
{
    class NodeSet
    {
        private List<Node> _nodes;
        public Dictionary<Node, List<Node>> parentChildDictionary = new Dictionary<Node, List<Node>>(); 

        public NodeSet()
        {
            _nodes = new List<Node>();
        }

        public NodeSet(List<Node> nodes)
        {
            _nodes = nodes;
            _nodes.ForEach(n => parentChildDictionary.Add(n,n.children));
        }

        public void Add(Node n)
        {
            if (_nodes.Contains(n))
            {
                Console.WriteLine("Node is already in set");
            }
            else
            {
                _nodes.Add(n);
                parentChildDictionary.Add(n, n.children);
            }

        }

        public void Remove(Node n)
        {
            if (_nodes.Contains(n))
                _nodes.Remove(n);
            else
                Console.WriteLine("Node doesn't exist.");
        }

        public bool Find(Node n)
        {
            return _nodes.Exists(node => node.NodeName == n.NodeName);
        }

        public Node GetNode(string nodeName)
        {
            return _nodes.Find(node => node.NodeName == nodeName);
        }

        public void Describe()
        {
            Console.WriteLine("------Nodset Described------");
            _nodes.ForEach(node => Console.WriteLine(node.ToString()));
        }

        public Node[] getAllNodes()
        {
            return _nodes.ToArray();
        }
    }
}
