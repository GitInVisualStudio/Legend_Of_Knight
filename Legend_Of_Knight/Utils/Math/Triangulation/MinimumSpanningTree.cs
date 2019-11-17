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
            List<Vector> pointsInside = new List<Vector>();
            pointsInside.Add(points[0]);
            List<Vector> pointsOutside = points.ToList();
            pointsOutside.Remove(points[0]);
            Edges = Calculate(pointsInside, new List<Edge>(), pointsOutside, edges.ToList());
            List<Vector> tempPoints = new List<Vector>();
            Edges.ToList().ForEach(x =>
            {
                if (!tempPoints.Contains(x.A))
                    tempPoints.Add(x.A);
                if (!tempPoints.Contains(x.B))
                    tempPoints.Add(x.B);
            });
            Points = tempPoints.ToArray();
        }

        private Edge[] Calculate(List<Vector> pointsInside, List<Edge> edgesInside, List<Vector> pointsOutside, List<Edge> edgesOutside)
        {
            if (pointsOutside.Count == pointsInside.Count + 1)
                return edgesInside.ToArray();

            Edge minCost = FindMinimumCostEdge(pointsInside[0], edgesOutside);
            for (int i = 1; i < pointsInside.Count; i++) // findet die Kante, die zu einem der Punkte des Baumes verbindet und am kleinsten ist
            {
                Edge e = FindMinimumCostEdge(pointsInside[i], edgesOutside);
                if (minCost == Edge.Null || minCost.Length > e.Length)
                    minCost = e;
            }

            Vector pointToAdd = pointsInside.Contains(minCost.A) ? minCost.B : minCost.A;
            pointsInside.Add(pointToAdd);
            pointsOutside.Remove(pointToAdd);
            edgesInside.Add(minCost);
            edgesOutside.Remove(minCost);

            return Calculate(pointsInside, edgesInside, pointsOutside, edgesOutside);
        }

        private Edge FindMinimumCostEdge(Vector point, List<Edge> edges)
        {
            edges = ((Edge[])edges.ToArray().Clone()).ToList();
            edges.RemoveAll(x => !(x.A == point || x.B == point)); // damit nur die Kanten betrachtet werden, die point mit einem anderen Punkt verbinden
            if (edges.Count == 0)
                return Edge.Null;
            Edge minCost = edges[0];
            for (int i = 1; i < edges.Count; i++)
                minCost = edges[i].Length < minCost.Length ? edges[i] : minCost;
            return minCost;
        }
    }
}
