using StrategyGame.Game_Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StrategyGame.Utils
{
  public class Node
    {
        public bool IsPassable;

        //        коорлинаты в матрице - номер столбца, номер строки
        public Point Coords;
        public Node ParentNode;
        public double G;
        public double H;
        public double F { get { return G + H; } }

    }

  public static  class PathFinder
    {

        // передавать единицу стоимости

        public static void InitiatePathFinding(MapCell[,] map, MapCell start,  MapCell end )
        {
            Node[,] nodes = new Node[map.GetLength(0), map.GetLength(1)];
            for (int i = 0; i < nodes.GetLength(0); i++)
            {
                for (int j = 0; j < nodes.GetLength(1); j++)
                {
                    nodes[i, j] = new Node()
                    {
                        Coords = new Point(i, j),
                        IsPassable = !map[i, j].IsTaken
                    };
                }
            }

        }

        public static List<MapCell> FindPath(MapCell[,] map, MapCell start, MapCell goal )
        {
            List<MapCell> route = new List<MapCell>();
            List<Node> openSet = new List<Node>();
            List<Node> closedSet = new List<Node>();
            Node startN = new Node()
            {
                Coords = start.Coords
            };
            Node goalN = new Node()
            {
                Coords = goal.Coords
            };

            openSet.Add(startN);
            startN.G = 0;
            startN.H = GetH(startN.Coords, goalN.Coords);

            while (openSet.Count != 0)
            {
                Node current = GetCurrentNode(openSet);
               // if (current == goalN)

                if (current.Coords == goalN.Coords)
                {
                    // нужно вернуть путь весь
                    route = GetPath(startN, current, map);
                    return route;
                        
                }

                openSet.Remove(current);
                closedSet.Add(current);
                List<Node> neighbours = getNeighbours(current, map, goalN);
                foreach (Node n in neighbours)
                {
                    // поиск по совпадающим координатам

                    // найти ту вершину в списке закрытых вершин,
                    //у которой координаты совпадают 
                    //с координатами соседа n

                    // x, у которого Coords==n.Coords
                    if (closedSet.Find(x  => x.Coords==n.Coords)!=null)
                        continue;
                    double O = current.G + 1;  //???????????
                    // поиск по совпадающим координатам

                    Node osNode = openSet.Find(x => x.Coords == n.Coords);
                    if (osNode == null)
                        openSet.Add(n);
                    else if (O < n.G)
                    {
                        osNode.G = O;
                        osNode.ParentNode = current;

                    }
                }
                // получить все соседние вершины с current

            }
            return null;
        }

        // для вершин проставлены родительские вершины
        //те вершины, из которых приходят в текущую
        static List<MapCell> GetPath(Node start, Node goal, MapCell[,] map)
        {
            List<MapCell> route = new List<MapCell>();
            // этот список нужно заполнить ячейками поля, которые будут составлять путь
            // есть вершина конечная goal
            // для известна та вершина, из которой придем в Goal

            Node current = goal;


            while (current.ParentNode != null)
            {
                // нужно достать клетку поля, которая соттветствует вершине current и добавить ее в route


                MapCell mc = map[ (int)current.Coords.X, (int)current.Coords.Y];

                route.Add(mc);
                current = current.ParentNode;
            }

            route.Reverse();
            return route;
        }


        // получить соседей - для вершины, с помощью матрицы
        static List<Node> getNeighbours(Node current, MapCell[,] map, Node goal)
        {
            List<Node> neighbours = new List<Node>();
            Point[] neighbourCoords = new Point[4];
            neighbourCoords[0] = new Point(current.Coords.X + 1, current.Coords.Y); // 1 2 
            neighbourCoords[1] = new Point(current.Coords.X , current.Coords.Y + 1); // 0 3
            neighbourCoords[2] = new Point(current.Coords.X - 1, current.Coords.Y);// 0, 2  -- -1, 2
            neighbourCoords[3] = new Point(current.Coords.X , current.Coords.Y-1);  //0 1

            for (int i = 0; i < neighbourCoords.Length; i++)
            {
                if (
                    neighbourCoords[i].X < 0 ||
                    neighbourCoords[i].Y < 0 ||
                    neighbourCoords[i].X >= map.GetLength(0) ||
                    neighbourCoords[i].Y >= map.GetLength(1)
                    )
                    continue;
                if (map[(int)neighbourCoords[i].X, (int)neighbourCoords[i].Y].IsTaken)
                    continue;

                Node node = new Node()
                {
                    Coords = new Point((int)neighbourCoords[i].X, (int)neighbourCoords[i].Y),
                    IsPassable = !map[(int)neighbourCoords[i].X, (int)neighbourCoords[i].Y].IsTaken,
                    G=current.G+1,
                    H =GetH(new Point((int)neighbourCoords[i].X, (int)neighbourCoords[i].Y), goal.Coords ),
                    ParentNode=current
                };


                neighbours.Add(node);
                // за пределами матрицы
                // x<0 or y<0 or y>=m.GetLength(1)  or x>=m.GetLength(0) 

            }

            return neighbours;
        }
        static Node GetCurrentNode(List<Node> list)
        {
            Node node = null;
            node = list[0];
            foreach (Node n in list)
            {

                if (n.F< node.F)
                {
                    node = n;
                }
            }
            return node;
        }



        static double GetH(Point a, Point b)
        {
            return Math.Sqrt(     
                Math.Pow((a.X-b.X), 2)
                +
                 Math.Pow((a.Y - b.Y), 2)
                );
        }


        public static List<MapCell> GetPossibleCells(MapCell current,  MapCell[,] map, int radius)
        {
            List<MapCell> maxCells = new List<MapCell>();
            List<MapCell> result = new List<MapCell>();

            //for (int i = (int)current.Coords.X-radius; i <= (int)current.Coords.X + radius; i++)
            //{
            //    for (int j = (int)current.Coords.Y - radius; j <= (int)current.Coords.Y + radius; j++)
            //    {
            //        if (i < 0 || j < 0 || i > map.GetLength(0)-1 || j > map.GetLength(1)-1)
            //            continue;
                    
            //        int diffY = (int)current.Coords.Y - j;
            //        int diffX = (int)current.Coords.X - i;
            //        int sum = Math.Abs(diffX) + Math.Abs(diffY);
            //        if (sum <= radius)
            //        {// доп условия для добавления клетки в result
            //            //if (sum == count && map[i, j].IsTaken)
            //            //    continue;
                       
            //            result.Add(map[i, j]);
            //        }

            //    }
            //}

            List<MapCell> closedSet = new List<MapCell>();
            Queue<MapCell> openSet = new Queue<MapCell>();

            openSet.Enqueue(current);

            while (openSet.Count != 0)
            {
                MapCell mc = openSet.Dequeue();
                closedSet.Add(mc);
                getNeighbours(mc,  map, closedSet, openSet, radius);
                // всех свободных соседей клетки

            }


            return closedSet;


        }

        public static void getNeighbours(MapCell current, MapCell[,] map, List<MapCell> closed, Queue<MapCell> openSet, int radius)
        {

            
            Point[] neighbourCoords = new Point[4];
            neighbourCoords[0] = new Point(current.Coords.X + 1, current.Coords.Y); // 1 2 
            neighbourCoords[1] = new Point(current.Coords.X, current.Coords.Y + 1); // 0 3
            neighbourCoords[2] = new Point(current.Coords.X - 1, current.Coords.Y);// 0, 2  -- -1, 2
            neighbourCoords[3] = new Point(current.Coords.X, current.Coords.Y - 1);  //0 1
            for (int i = 0; i < neighbourCoords.Length; i++)
            {
                if (
                    neighbourCoords[i].X < 0 ||
                    neighbourCoords[i].Y < 0 ||
                    neighbourCoords[i].X >= map.GetLength(0) ||
                    neighbourCoords[i].Y >= map.GetLength(1)
                    )
                    continue;

                int diffY = (int)current.Coords.Y - (int)neighbourCoords[i].Y;
                int diffX = (int)current.Coords.X - (int)neighbourCoords[i].X;
                int sum = Math.Abs(diffX) + Math.Abs(diffY);
              

                if (map[(int)neighbourCoords[i].X, (int)neighbourCoords[i].Y].IsTaken)
                    continue;
                if (closed.Contains(map[(int)neighbourCoords[i].X, (int)neighbourCoords[i].Y]))
                    continue;
                if (openSet.Contains(map[(int)neighbourCoords[i].X, (int)neighbourCoords[i].Y]))
                    continue;
                if (sum <= radius)
                    
                openSet.Enqueue(map[(int)neighbourCoords[i].X, (int)neighbourCoords[i].Y]);


            }

      
        }


    }
}
