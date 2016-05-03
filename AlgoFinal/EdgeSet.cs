using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoFinal
{
    class EdgeSet
    {
        private List<Edge> _edges;

        public EdgeSet()
        {
            _edges = new List<Edge>();
        }

        public EdgeSet(List<Edge> edges)
        {
            _edges = edges;
        }

        public void Add(Edge e)
        {
           if(_edges.Contains(e))
               Console.WriteLine("That edge already exists!");
           else 
               _edges.Add(e);
        }

        public void Remove(Edge e)
        {
            if (_edges.Contains(e))
                _edges.Remove(e);
            else
                Console.WriteLine("That edge doesn't exist!");
            
        }

        public bool Find(Edge e)
        {
            return _edges.Contains(e);
        }

        public Edge GetEdge(Edge e)
        {
            return Find(e) ? _edges.Find(edge => edge.EdgeName == e.EdgeName) : null;
        }

        public void Describe()
        {
           _edges.ForEach(edge => Console.WriteLine(edge.ToString()));
        }
    }
}
