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

        public Triangle[] Triangles { get => triangles; set => triangles = value; }

        /// <summary>
        /// Trianguliert gegebene Punkte zu einem unstrukturierten Delaunay-Gitter mithilfe des Bowyer-Watson algorithmus
        /// </summary>
        public DelaunayTriangulation(Vector size, Vector[] points)
        {
            List<Triangle> tempTriangles = new List<Triangle>();
            Vector superA = new Vector(-100, -100);
            Vector superB = new Vector(2 * size.X + 100, -100);
            Vector superC = new Vector(-100, 2 * size.Y + 100);
            Triangle super = new Triangle(superA, superB, superC); // Dreieck, dass alle Punkte enthält
            tempTriangles.Add(super);

            foreach (Vector point in points) // inkrementelles Hinzufügen der Punkte
            {
                List<Triangle> badTriangles = new List<Triangle>(); // Dreiecke, die nicht mit Delaunay-Triangulation konform sind => in deren Umkreis sich der neue Punkt point befindet
                foreach (Triangle t in tempTriangles)
                    if (t.PointInCircumcircle(point))
                        badTriangles.Add(t);

                List<Edge> polygon = new List<Edge>();
                foreach (Triangle t in badTriangles)
                    foreach (Edge e in t.Edges)
                        if (!badTriangles.Any(x => x != t && x.Edges.Contains(e))) // DEBUG: unsure if this is semantically correct
                            polygon.Add(e);

                tempTriangles.RemoveAll(x => badTriangles.Contains(x));
                foreach (Edge e in polygon)
                    tempTriangles.Add(new Triangle(e.A, e.B, point));
            }

            foreach (Triangle t in tempTriangles)
                if (t.ContainsPoint(superA) || t.ContainsPoint(superB) || t.ContainsPoint(superC))
                    tempTriangles.Remove(t);

            Triangles = tempTriangles.ToArray();
        }
    }
}
