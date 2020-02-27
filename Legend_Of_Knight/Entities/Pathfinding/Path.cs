using Legend_Of_Knight.Utils.Math;
using Legend_Of_Knight.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legend_Of_Knight.Entities.Pathfinding
{
    public class Path
    {
        private Node startNode;
        private Node endNode;
        private Node currentNode;
        private Dungeon d;

        public Path(Vector startPosition, Vector endPosition, Dungeon d)
        {
            startNode = new Node(startPosition)
            {
                G = 0,
                H = 0
            };
            endNode = new Node(endPosition)
            {
                G = 0,
                H = 0
            };
            this.d = d;

            AStar();
            currentNode = startNode;
        }

        private void AStar()
        {
            List<Node> open = new List<Node>();
            List<Node> closed = new List<Node>();
            List<Vector> relativeChildCoordinates = new List<Vector>();
            for (int x = -1; x <= 1; x++)
                for (int y = -1; y <= 1; y++)
                    if (x == 0 && y == 0)
                        continue;
                    else
                        relativeChildCoordinates.Add(new Vector(x, y));

            open.Add(startNode);

            while(open.Count > 0)
            {
                Node current = GetCheapestNode(open);
                open.Remove(current);
                closed.Add(current);

                if (current == endNode)
                {
                    Backtrack(current);
                    return;
                }
                    
                foreach (Vector relChildCoords in relativeChildCoordinates)
                {
                    Vector childCoords = current.Position + relChildCoords;
                    Node child = new Node(childCoords, current);
                    if (d.Fields[(int)childCoords.X, (int)childCoords.Y].Type != FieldType.Floor || closed.Contains(child))
                        continue;

                    child.G = current.G + 1;
                    child.H = child.DistanceTo(endNode);

                    Node otherChild = open.Find(x => x == child);
                    if (otherChild != null)
                        if (otherChild.G < child.G)
                            continue;
                    open.Add(child);
                }
            }
        }

        private Node GetCheapestNode(List<Node> nodes)
        {
            if (nodes.Count == 0)
                return null;
            Node cheapest = nodes[0];
            for (int i = 1; i < nodes.Count; i++)
                cheapest = nodes[i].F < cheapest.F ? nodes[i] : cheapest;
            return cheapest;
        }

        private void Backtrack(Node endNode)
        {
            if (endNode.Parent == null)
                return;

            endNode.Parent.Child = endNode;
            Backtrack(endNode.Parent);
        }

        public Node GetNextNode()
        {
            if (currentNode == null)
                return null;
            Node res = currentNode;
            currentNode = currentNode.Child;
            return res;
        }

        public void Reset()
        {
            currentNode = startNode;
        }
    }
}
