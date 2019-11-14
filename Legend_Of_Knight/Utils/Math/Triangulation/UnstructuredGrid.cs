using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legend_Of_Knight.Utils.Math.Triangulation
{
    public class UnstructuredGrid
    {
        private Vector[] points;
        private Connection[] connections;

        public UnstructuredGrid(params Vector[] points)
        {
            this.points = points;
            DelauneyTriangulation();
        }

        public void DelauneyTriangulation()
        {
            List<Connection> res = new List<Connection>();
            foreach (Vector p in points)
                foreach (Vector k in points)
                {
                    if (p.Equals(k) || ConnectionExists(p, k))
                        continue;

                    res.Add(new Connection(p, k));
                    connections = res.ToArray();
                }

            Line[] vLines = new Line[connections.Length];
            for (int i = 0; i < connections.Length; i++)
                vLines[i] = connections[i].GetNormal();


        }

        public bool ConnectionExists(Vector a, Vector b)
        {
            foreach (Connection c in connections)
                if (c.Connects(a, b))
                    return true;
            return false;
        }
    }
}
