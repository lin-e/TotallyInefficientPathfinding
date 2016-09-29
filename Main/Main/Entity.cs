using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Main
{
    public class Entity
    {
        public Map parentMap;
        public Path End;
        List<Path> allPaths = new List<Path>();
        public List<int[]> usedPoints = new List<int[]>();
        int[][] customPoints = { new int[] { }, new int[] { } };
        public Entity(Map parentMap, int[] customStart, int[] customEnd)
        {
            this.parentMap = parentMap;
            customPoints[0] = customStart;
            customPoints[1] = customEnd;
        }
        public int[] FindStart()
        {
            if (!((customPoints[0][0] == customPoints[1][0]) && (customPoints[0][1] == customPoints[1][1])))
            {
                return customPoints[0];
            }
            for (int y = 0; y < Console.WindowHeight; y++)
            {
                int[] currentPoint = { 0, y };
                if (parentMap.mapEmpty.Any(currentPoint.SequenceEqual))
                {
                    return currentPoint;
                }
            }
            return new int[] { 0, 0 };
        }
        public int[] FindEnd()
        {
            if (!((customPoints[0][0] == customPoints[1][0]) && (customPoints[0][1] == customPoints[1][1])))
            {
                return customPoints[1];
            }
            for (int y = 0; y < Console.WindowHeight; y++)
            {
                int[] currentPoint = { Console.WindowWidth - 1, y };
                if (parentMap.mapEmpty.Any(currentPoint.SequenceEqual))
                {
                    return currentPoint;
                }
            }
            return new int[] { 0, 0 };
        }
        public bool SolvePath(bool showProgess, bool allowDiagonal, ConsoleColor progressColour = ConsoleColor.DarkBlue, int threadDelay = 0)
        {
            if (!(parentMap.mapEmpty.Any(FindStart().SequenceEqual)))
            {
                return false;
            }
            if (!(parentMap.mapEmpty.Any(FindEnd().SequenceEqual)))
            {
                return false;
            }
            allPaths.Add(new Path(this));
            bool pathFound = false;
            while (!pathFound)
            {
                if (allPaths.Count == 0)
                {
                    break;
                }
                List<Path> toRemove = new List<Path>();
                List<Path> toAdd = new List<Path>();
                foreach (Path singlePath in allPaths)
                {
                    PointUpdate newUpdate = singlePath.CalculateNext(showProgess, allowDiagonal, progressColour);
                    if (newUpdate.isEnd)
                    {
                        End = singlePath;
                        pathFound = true;
                        break;
                    }
                    else
                    {
                        if (newUpdate.removeSelf)
                        {
                            toRemove.Add(singlePath);
                            if (newUpdate.newPaths.Count > 0)
                            {
                                foreach (Path newPath in newUpdate.newPaths)
                                {
                                    toAdd.Add(newPath);
                                }
                            }
                        }
                    }
                }
                foreach (Path singlePath in toRemove)
                {
                    allPaths.Remove(singlePath);
                }
                foreach (Path singlePath in toAdd)
                {
                    allPaths.Add(singlePath);
                }
                toRemove.Clear();
                toAdd.Clear();
                if (showProgess)
                {
                    Thread.Sleep(threadDelay);
                }
            }
            return pathFound;
        }
    }
}
