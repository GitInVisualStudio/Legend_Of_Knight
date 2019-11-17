using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legend_Of_Knight.Utils.Math.Triangulation
{
    public class MinimumSpanningTree
    {
        private Vector[] points;
        private Edge[] edges;

        public Vector[] Points { get => points; set => points = value; }
        public Edge[] Edges { get => edges; set => edges = value; }

        public MinimumSpanningTree(Vector[] points, Edge[] edges)
        {
            Calculate(points, edges);
        }

        public MinimumSpanningTree(DelaunayTriangulation triangulation)
        {
            Calculate(triangulation.Points, triangulation.Edges);
        }

        private void Calculate(Vector[] points, Edge[] edges)
        {
            // https://en.wikipedia.org/wiki/Prim's_algorithm#Description
            Dictionary<Vector, float> c = new Dictionary<Vector, float>(); // 1
            for (int i = 0; i < points.Length; i++)
                c.Add(points[i], float.MaxValue);

            Dictionary<Vector, Edge> e = new Dictionary<Vector, Edge>();
            for (int i = 0; i < points.Length; i++)
                e.Add(points[i], Edge.Null);

            Forest forest = new Forest(); // 2
            List<Vector> q = points.ToList();

            while (q.Count > 0) // 3
            {
                Vector v = MinKey(q, c); // Punkt, zu dem die Verbindung am billigsten ist | 3.a
                q.Remove(v);
                forest.Verticies.Add(v); // 3.b
                if (e[v] != Edge.Null)
                    forest.Edges.Add(e[v]);

                Edge[] connectingEdges = GetConnectingEdges(v, edges); // 3.c
                foreach (Edge vw in connectingEdges)
                {
                    Vector w = vw.A == v ? vw.B : vw.A;
                    if (q.Contains(w) && vw.Length < c[w])
                    {
                        c[w] = vw.Length;
                        e[w] = vw;
                    }
                }
            }

            Points = forest.Verticies.ToArray();
            Edges = forest.Edges.ToArray();
        }

        private Vector MinKey(IEnumerable<Vector> acceptable, Dictionary<Vector, float> dict)
        {
            if (dict.Count == 0)
                return Vector.Null;
            Vector minKey = dict.First().Key;
            for (int i = 0; i < dict.Count; i++)
                if (acceptable.Contains(dict.Keys.ElementAt(i)) && dict[dict.Keys.ElementAt(i)] < dict[minKey])
                    minKey = dict.Keys.ElementAt(i);
            return minKey;
        }

        private int MinIndex(IEnumerable<float> array)
        {
            if (array.Count() == 0)
                return -1;
            int min = 0;
            for (int i = 1; i < array.Count(); i++)
                min = array.ElementAt(i) < array.ElementAt(min) ? i : min;
            return min;
        }

        private float Min(IEnumerable<float> array)
        {
            return array.ElementAt(MinIndex(array));
        }

        private Edge[] GetConnectingEdges(Vector vertex, IEnumerable<Edge> edges)
        {
            List<Edge> connectingEdges = new List<Edge>(); // alle Kanten, die mit point verbinden
            foreach (Edge e in edges)
                if (e.A == vertex || e.B == vertex)
                    connectingEdges.Add(e);
            return connectingEdges.ToArray();
        }

        private Edge FindCheapestConnection(Vector vertex, IEnumerable<Edge> edges)
        {
            Edge[] connectingEdges = GetConnectingEdges(vertex, edges);
            Edge cheapest = connectingEdges[0];
            for (int i = 0; i < connectingEdges.Length; i++)
                cheapest = cheapest.Length > connectingEdges[i].Length ? connectingEdges[i] : cheapest;
            return cheapest;
        }
    }
}
