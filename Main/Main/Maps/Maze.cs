using System;
using System.Collections.Generic;
using System.Linq;

namespace Main
{
    public class Maze
    {
        public List<string> allLines = new List<string>();
        int gridHeight;
        int gridWidth;
        int nodeHeight;
        int nodeWidth;
        int upperBound;
        Random mainRandom;
        int[,] nodeWeight;
        List<int[]> deadNodes = new List<int[]>();
        List<int[]> activeNodes = new List<int[]>();
        public Maze(int upperBound = 64, int genSeed = 0)
        {
            this.upperBound = upperBound;
            mainRandom = (genSeed == 0) ? new Random() : new Random(genSeed);
            gridHeight = Console.WindowHeight - 2;
            gridWidth = Console.WindowWidth - 2;
            nodeWidth = (int)Math.Ceiling((decimal)gridWidth / 2);
            nodeHeight = (int)Math.Ceiling((decimal)gridHeight / 2);
            for (int y = 0; y < gridHeight + 2; y++)
            {
                allLines.Add(string.Join("", Enumerable.Repeat("0", gridWidth + 2)));
            }
            nodeWeight = new int[nodeWidth, nodeHeight];
            for (int x = 0; x < nodeWidth; x++)
            {
                for (int y = 0; y < nodeHeight; y++)
                {
                    nodeWeight[x, y] = mainRandom.Next(1, this.upperBound);
                }
            }
        }
        public void Generate(bool showProgress = false)
        {
            int[] startPoint = { 0, 0 };
            activeNodes.Add(startPoint);
            while (activeNodes.Count < (nodeWidth * nodeHeight))
            {
                List<Arc> possibleArcs = new List<Arc>();
                foreach (int[] singleNode in activeNodes)
                {
                    foreach (int[] newNode in getSurrouding(singleNode))
                    {
                        possibleArcs.Add(new Arc(this, singleNode, newNode));
                    }
                }
                Arc currentArc = new Arc();
                int currentMinDistance = upperBound + 2;
                foreach (Arc singleArc in possibleArcs)
                {
                    if (singleArc.absDistance < currentMinDistance)
                    {
                        currentMinDistance = singleArc.absDistance;
                        currentArc = singleArc;
                        if (singleArc.absDistance == 0)
                        {
                            break;
                        }
                    }
                }
                activeNodes.Add(currentArc.endNode);
                int[] pathPos = getExportPoint(currentArc.startNode[0], currentArc.startNode[1]);
                pathPos = pathPos.Move(currentArc.moveDir);
                setPoint(getExportPoint(currentArc.startNode[0], currentArc.startNode[1]), '1');
                setPoint(pathPos, '1');
                setPoint(getExportPoint(currentArc.endNode[0], currentArc.endNode[1]), '1');
                if (!showProgress)
                {
                    continue;
                }
                Console.SetCursorPosition(0, 0);
                Preview();
            }
        }
        public void Preview()
        {
            string x = string.Join("", allLines).Replace("1", " ").Replace("0", "█");
            Console.Write(x.Remove(x.Length - 1, 1));
        }

        List<int[]> getSurrouding(int[] currentPoint)
        {
            List<int[]> possiblePoints = new List<int[]>();
            if (deadNodes.Any(currentPoint.SequenceEqual))
            {
                return possiblePoints;
            }
            Direction[] possibleDirections = { Direction.Up, Direction.Down, Direction.Left, Direction.Right };
            List<Direction> toSkip = new List<Direction>();
            if (currentPoint[0] == 0)
            {
                toSkip.Add(Direction.Left);
            }
            if (currentPoint[0] == nodeWidth - 1)
            {
                toSkip.Add(Direction.Right);
            }
            if (currentPoint[1] == 0)
            {
                toSkip.Add(Direction.Up);
            }
            if (currentPoint[1] == nodeHeight - 1)
            {
                toSkip.Add(Direction.Down);
            }
            foreach (Direction singleDirection in possibleDirections)
            {
                if (toSkip.Contains(singleDirection))
                {
                    continue;
                }
                int[] newPoint = currentPoint.Move(singleDirection);
                if (activeNodes.Any(newPoint.SequenceEqual))
                {
                    continue;
                }
                possiblePoints.Add(newPoint);
            }
            if (possiblePoints.Count == 0)
            {
                deadNodes.Add(currentPoint);
            }
            return possiblePoints;
        }
        int[] getExportPoint(int x, int y)
        {
            return new int[] { (2 * x) + 1, (2 * y) + 1 };
        }
        char getPoint(int[] checkPoint)
        {
            return allLines[checkPoint[1]][checkPoint[0]];
        }
        void setPoint(int[] setPoint, char newVal)
        {
            string changeLineContent = allLines[setPoint[1]];
            char[] lineArray = changeLineContent.ToCharArray();
            lineArray[setPoint[0]] = newVal;
            allLines[setPoint[1]] = new string(lineArray);
        }
        public class Arc
        {
            public int[] startNode;
            public int[] endNode;
            public int absDistance;
            public Direction moveDir;
            public Arc() { }
            public Arc(Maze parentMaze, int[] startNode, int[] endNode)
            {
                this.startNode = startNode;
                this.endNode = endNode;
                absDistance = Math.Abs(parentMaze.nodeWeight[startNode[0], startNode[1]] - parentMaze.nodeWeight[endNode[0], endNode[1]]);
                if (startNode[0] == endNode[0])
                {
                    if (startNode[1] > endNode[1])
                    {
                        moveDir = Direction.Up;
                    }
                    else
                    {
                        moveDir = Direction.Down;
                    }
                }
                else
                {
                    if (startNode[0] > endNode[0])
                    {
                        moveDir = Direction.Left;
                    }
                    else
                    {
                        moveDir = Direction.Right;
                    }
                }
            }
        }
    }
}
