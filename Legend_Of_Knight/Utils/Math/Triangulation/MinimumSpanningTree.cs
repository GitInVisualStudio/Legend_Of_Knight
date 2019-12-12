using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legend_Of_Knight.Utils.Math.Triangulation
{
    /// <summary>
    /// Ein Satz von Punkten und Verbindungen, bei dem jeder Punkt mit einem anderen verbunden ist und die Gesamtlänge aller Verbindungen am kleinsten ist 
    /// </summary>
    public class MinimumSpanningTree
    {
        private Vector[] points;
        private Edge[] edges;

        public Vector[] Points { get { return points; } set { points = value; } }
        public Edge[] Edges { get { return edges; } set { edges = value; } }

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
            // implementiert nach https://en.wikipedia.org/wiki/Prim's_algorithm#Description
            Dictionary<Vector, float> c = new Dictionary<Vector, float>(); // Dictionary, das für einen Punkt die Kosten für eine Verbindung mit ihm angibt
            for (int i = 0; i < points.Length; i++)
                c.Add(points[i], float.MaxValue);

            Dictionary<Vector, Edge> e = new Dictionary<Vector, Edge>(); // Dictionary, das für einen Punkt die kürzeste (=> günstigste) Verbindung mit ihm angibt
            for (int i = 0; i < points.Length; i++)
                e.Add(points[i], Edge.Null);

            Forest forest = new Forest();
            List<Vector> q = points.ToList(); // alle noch nicht verbundenen Punkte

            while (q.Count > 0)
            {
                Vector v = MinKey(q, c); // Punkt, zu dem die Verbindung am billigsten ist
                q.Remove(v);
                forest.Verticies.Add(v);
                if (e[v] != Edge.Null) // falls eine Verbindung zu einem vorher hinzugefügten Punkt existiert (sollte nur beim ersten Punkt false sein)
                    forest.Edges.Add(e[v]);

                Edge[] connectingEdges = GetConnectingEdges(v, edges);
                foreach (Edge vw in connectingEdges) // bestimmt die günstigste Verbindung, die den momentanen Punkt mit anderen, noch nicht behandelten Punkten verbindet
                {
                    Vector w = vw.A == v ? vw.B : vw.A; // der andere Punkt der Verbindung
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

        /// <summary>
        /// Sucht den Schlüssel, der das minimalste Ergebnis aus einem Dictionary von Vektoren und Floats liefert
        /// </summary>
        /// <param name="acceptable">Alle Vektoren, die als Ergebnis in Frage kommen</param>
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

        /// <summary>
        /// Gibt für einen Punkt alle Verbindungen mit diesem Punkt zurück
        /// </summary>
        private Edge[] GetConnectingEdges(Vector vertex, IEnumerable<Edge> edges)
        {
            List<Edge> connectingEdges = new List<Edge>(); // alle Kanten, die mit point verbinden
            foreach (Edge e in edges)
                if (e.A == vertex || e.B == vertex)
                    connectingEdges.Add(e);
            return connectingEdges.ToArray();
        }
    }
}
