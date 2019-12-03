using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legend_Of_Knight.Utils.Math.Triangulation
{
    public class Forest
    {
        private List<Vector> verticies;
        private List<Edge> edges;

        public List<Vector> Verticies { get => verticies; set => verticies = value; }
        public List<Edge> Edges { get => edges; set => edges = value; }

        public Forest()
        {
            Verticies = new List<Vector>();
            Edges = new List<Edge>();
        }
    }
}
