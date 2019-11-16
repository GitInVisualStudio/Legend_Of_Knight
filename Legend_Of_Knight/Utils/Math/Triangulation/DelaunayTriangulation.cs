using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legend_Of_Knight.Utils.Math.Triangulation
{
    public class DelaunayTriangulation
    {
        private Triangle[] triangles;
        private Edge[] edges;
        private Triangle super;

        private Vector[] points;

        public Triangle[] Triangles { get => triangles; set => triangles = value; }
        public Triangle Super { get => super; set => super = value; }
        public Edge[] Edges { get => edges; set => edges = value; }

        public Vector[] Points { get => points; set => points = value; }

        /// <summary>
        /// Trianguliert gegebene Punkte zu einem unstrukturierten Delaunay-Gitter mithilfe des Bowyer-Watson Algorithmus
        /// </summary>
        public DelaunayTriangulation(Vector size, Vector[] points)
        {
            Points = points;
            List<Triangle> tempTriangles = new List<Triangle>();
            Vector superA = new Vector(-100, -100);
            Vector superB = new Vector(2 * size.X + 100, -100);
            Vector superC = new Vector(-100, 2 * size.Y + 100);
            super = new Triangle(superA, superB, superC); // Dreieck, dass alle Punkte enthält
            tempTriangles.Add(super);

            foreach (Vector point in points) // inkrementelles Hinzufügen der Punkte
            {
                List<Triangle> badTriangles = new List<Triangle>(); // Dreiecke, die nicht mit Delaunay-Triangulation konform sind => in deren Umkreis sich der neue Punkt point befindet
                foreach (Triangle t in tempTriangles)
                    if (t.PointInCircumcircle(point))
                        badTriangles.Add(t);

                List<Edge> polygon = new List<Edge>();
                foreach (Triangle t in badTriangles)
                    foreach (Edge e in t.Edges) // Formt ein Polygon aus den Kanten, die nur in einem einzigen schlechten Dreieck existieren
                    {
                        bool shared = false;
                        foreach (Triangle other in badTriangles)
                        {
                            if (other == t)
                                continue;

                            foreach (Edge f in other.Edges)
                                if (f == e)
                                    shared = true;
                        }
                        if (!shared)
                            polygon.Add(e);
                    }
                        

                tempTriangles.RemoveAll(x => badTriangles.Contains(x));
                foreach (Edge e in polygon)
                    tempTriangles.Add(new Triangle(e.A, e.B, point));
            }
            List<Edge> tempEdges = new List<Edge>();
            foreach (Triangle t in tempTriangles)
                tempEdges.AddRange(t.Edges);
            tempEdges.RemoveAll(x => x.A == superA || x.B == superB || x.A == superB || x.B == superB || x.A == superC || x.B == superC);
            edges = tempEdges.ToArray();

            List<Triangle> toRemove = new List<Triangle>();
            for(int i = 0; i < tempTriangles.Count; i++)
                if (tempTriangles[i].ContainsPoint(superA) || tempTriangles[i].ContainsPoint(superB) || tempTriangles[i].ContainsPoint(superC))
                {
                    toRemove.Add(tempTriangles[i]);
                }
            tempTriangles.RemoveAll(x => toRemove.Contains(x));
            Triangles = tempTriangles.ToArray();
        }
    }
}
