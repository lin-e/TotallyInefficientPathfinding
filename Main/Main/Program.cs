using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading;

namespace Main
{
    class Program
    {
        static void Main(string[] args)
        {
            Map mainMap = new Map(ConsoleColor.White);
            mainMap.LoadMapComplex();
            //mainMap.LoadMapGrid();
            //mainMap.LoadMapEmpty();
            mainMap.Draw();
            Entity testEntity = new Entity(mainMap);
            Stopwatch mainTimer = new Stopwatch();
            mainTimer.Start();
            if (testEntity.SolvePath(true))
            {
                mainTimer.Stop();
                testEntity.End.Draw();
                Console.Title = string.Format("Solved map in {0}ms (Length: {1})", mainTimer.ElapsedMilliseconds, testEntity.End.Length);
            }
            else
            {
                mainTimer.Stop();
                Console.Title = string.Format("Could not solve map in {0}ms", mainTimer.ElapsedMilliseconds);
            }
            Console.ReadLine();
        }
    }
    public class Entity
    {
        public Map parentMap;
        public Path End;
        List<Path> allPaths = new List<Path>();
        List<int[]> usedPoints = new List<int[]>();
        public Entity(Map parentMap)
        {
            this.parentMap = parentMap;
        }
        int[] FindStart()
        {
            //return new int[] { 5, 20 }; // Set custom start point
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
        int[] FindEnd()
        {
            return new int[] { 79, 15 }; // Set custom destination point
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
        public bool SolvePath(bool showProgess = false)
        {
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
                    PointUpdate newUpdate = singlePath.CalculateNext(showProgess);
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
            }
            return pathFound;
        }
        public class Path
        {
            public Entity parentEntity;
            List<int[]> allPoints = new List<int[]>();
            int[][] mainPoints = new int[][] { new int[] { }, new int[] { } };
            int[] currentPoint;
            public Path(Entity parentEntity)
            {
                this.parentEntity = parentEntity;
                mainPoints[0] = parentEntity.FindStart();
                mainPoints[1] = parentEntity.FindEnd();
                currentPoint = mainPoints[0];
                allPoints.Add(currentPoint);
            }
            public int Length
            {
                get
                {
                    return allPoints.Count;
                }
            }
            int[][] CompilePath()
            {
                return allPoints.ToArray();
            }
            Path Copy(Direction direction)
            {
                Path toReturn = new Path(parentEntity);
                foreach (int[] singlePoint in allPoints)
                {
                    toReturn.allPoints.Add(singlePoint);
                }
                toReturn.allPoints.Add(currentPoint);
                toReturn.currentPoint = currentPoint.Move(direction);
                toReturn.allPoints.Add(toReturn.currentPoint);
                return toReturn;
            }
            public PointUpdate CalculateNext(bool showProgress = false)
            {
                if (!(parentEntity.usedPoints.Any(new int[] { currentPoint[0], currentPoint[1] }.SequenceEqual)))
                {
                    parentEntity.usedPoints.Add(new int[] { currentPoint[0], currentPoint[1] });
                }
                if (showProgress)
                {
                    Console.SetCursorPosition(currentPoint[0], currentPoint[1]);
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.Write("#");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                PointUpdate toReturn = new PointUpdate();
                Direction[] allDirections = { Direction.Up, Direction.Down, Direction.Left, Direction.Right };
                List<Direction> possibleNext = new List<Direction>();
                foreach (Direction nextDirection in allDirections)
                {
                    int[] nextPoint = currentPoint.Move(nextDirection);
                    if (allPoints.Any(nextPoint.SequenceEqual))
                    {
                        continue;
                    }
                    if (!(parentEntity.parentMap.mapEmpty.Any(nextPoint.SequenceEqual)))
                    {
                        continue;
                    }
                    if (parentEntity.usedPoints.Any(nextPoint.SequenceEqual))
                    {
                        continue;
                    }
                    parentEntity.usedPoints.Add(nextPoint);
                    possibleNext.Add(nextDirection);
                }
                if (possibleNext.Count() == 1)
                {
                    toReturn.removeSelf = false;
                    currentPoint = currentPoint.Move(possibleNext[0]);
                    allPoints.Add(currentPoint);
                    if ((currentPoint[0] == mainPoints[1][0]) && (currentPoint[1] == mainPoints[1][1]))
                    {
                        toReturn.isEnd = true;
                    }
                }
                else
                {
                    toReturn.removeSelf = true;
                }
                if (possibleNext.Count() > 1)
                {
                    foreach (Direction singleDirection in possibleNext)
                    {
                        toReturn.newPaths.Add(Copy(singleDirection));
                    }
                }
                return toReturn;
            }
            public void Draw(ConsoleColor colour = ConsoleColor.Blue)
            {
                Console.ForegroundColor = colour;
                Console.BackgroundColor = colour;
                foreach (int[] singlePoint in CompilePath())
                {
                    Console.SetCursorPosition(singlePoint[0], singlePoint[1]);
                    Console.Write("#");
                }
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.BackgroundColor = ConsoleColor.Black;
            }
        }
        public class PointUpdate
        {
            public PointUpdate()
            {
                newPaths = new List<Path>();
            }
            public bool isEnd;
            public List<Path> newPaths;
            public bool removeSelf;
        }
    }
    public static class Extensions
    {
        public static int[] Move(this int[] origin, Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return new int[] { origin[0], origin[1] - 1 };
                case Direction.Down:
                    return new int[] { origin[0], origin[1] + 1 };
                case Direction.Left:
                    return new int[] { origin[0] - 1, origin[1] };
                case Direction.Right:
                    return new int[] { origin[0] + 1, origin[1] };
            }
            return null;
        }
    }
    public class Map
    {
        public int[][] mapEmpty = { };
        ConsoleColor wallColour;
        public Map(ConsoleColor colourToUse = ConsoleColor.White)
        {
            wallColour = colourToUse;
        }
        public void LoadMapEmpty()
        {
            List<int[]> newPoints = new List<int[]>();
            for (int x = 0; x < Console.WindowWidth; x++)
            {
                for (int y = 0; y < Console.WindowHeight; y++)
                {
                    newPoints.Add(new int[] { x, y });
                }
            }
            mapEmpty = newPoints.ToArray();
        }
        public void LoadMapGrid()
        {
            List<int[]> newPoints = new List<int[]>();
            for (int x = 0; x < Console.WindowWidth; x++)
            {
                if (x % 5 != 0)
                {
                    continue;
                }
                for (int y = 0; y < Console.WindowHeight; y++)
                {
                    newPoints.Add(new int[] { x, y });
                }
            }
            for (int y = 0; y < Console.WindowHeight; y++)
            {
                if (y % 5 != 0)
                {
                    continue;
                }
                for (int x = 0; x < Console.WindowWidth; x++)
                {
                    newPoints.Add(new int[] { x, y });
                }
            }
            mapEmpty = newPoints.ToArray();
        }
        public void LoadMapComplex()
        {
            List<int[]> newPoints = new List<int[]>();
            string[] mapLoad =
            {
                "00000000000000000000000000000000000000000000000000000000000000000000000000000000",
                "11111100000000000000000000000000000000000000000000000000000000000000000000000000",
                "00000110000000000000000000000000000000000000000000000000000000000000000000000000",
                "00000011111111111111111111111111111111111111111111111111111111111111111111111100",
                "00000000000000000011110000000000000000111111111111111111111111111000100000000000",
                "00000000000000000000010000000000000000000001000000000000000000001111111111111111",
                "00000000000000001111111111111111111111111111111111110000000000000000010000000000",
                "00000000000000011000000000000010000000000000001000000000000000000000011100000000",
                "00000000000000001100000000000010000000000000001000000000000000000000000110000000",
                "00000000000000000111000000000010000000000000001000000000111111111111000010000000",
                "00000000000000000001110000000010000000000000001000000000000000001001111110000000",
                "00000000000000000000011100000010000000000000001000000000000000001000000011111111",
                "00000000000000000000000111000010000000000000001000000000000000011000000010000000",
                "00000000001000000000000001111110000000000000001111111111111111110000000010000000",
                "00000000001000000000000001000010000000000000000000000000000000000000000010000000",
                "00000000001111100000000001111111111111111111111111111111000000000001111111111111",
                "00000000001000000000000001000010000000000100000000000000000000000001000000000000",
                "00000000001111111111111111000010000000000111111111111000000000000001000000000000",
                "00000000000000000001000000000010000000000000000000011111111111111111111111100000",
                "00000000000000000001000000000011111111110000000000000000000000000000000000000000",
                "00000000000000000001000000000010000000011111111111111111110000000000000000000000",
                "00000000000000000001000000000010000000000000000000000000010000000000000000000000",
                "00000000000000000001111111111111111111111111111111111111110000000000000000000000",
                "00000000000000000000000000000000000000000000000000000000011111111111111111111111",
                "00000000000000000000000000000000000000000000000000000000000000000000000000000000",
            };
            /*
            string[] mapLoad =
            {
                "00000000000000000000000000000000000000000000000000000000000000000000000000000000",
                "11111100000000000000000000000000000000000000000000000000000000000000000000000000",
                "00000110000000000000000000000000000000000000000000000000000000000000000000000000",
                "00000011111111111111111111111111111111111111111111111111111111111111111111111100",
                "00000000000000000011110000000000000000111111111111111111111111111000100000000000",
                "00000000000000000000010000000000000000000001000000000000000000001111111111111111",
                "00000000000000001111111111111111111111111111111111110000000000000000010000000000",
                "00000000000000011000000000000010000000000000001000000000000000000000011100000000",
                "00000000000000001100000000000010000000000000001000000000000000000000000110000000",
                "00000000000000000111000000000010000000000000001000000000111111111111000010000000",
                "00000000000000000001110000000010000000000000001000000000000000001001111110000000",
                "00000000000000000000011100000010000000000000001000000000000000001000000011111111",
                "00000000000000000000000111000010000000000000001000000000000000011000000000000000",
                "00000000001000000000000001111110000000000000001111111111111111110000000000000000",
                "00000000001000000000000001000010000000000000000000000000000000000000000000000000",
                "00000000001111100000000001111111111111111111111111111111000000000001111111111111",
                "00000000001000000000000001000010000000000100000000000000000000000001000000000000",
                "00000000001111111111111111000010000000000111111111111000000000000001000000000000",
                "00000000000000000000000000000010000000000000000000011111111111111111111111100000",
                "00000000000000000000000000000011111111110000000000000000000000000000000000000000",
                "00000000000000000000000000000000000000011111111111111111110000000000000000000000",
                "00000000000000000000000000000000000000000000000000000000010000000000000000000000",
                "00000000000000000000001111111111111111111111111111111111110000000000000000000000",
                "00000000000000000000000000000000000000000000000000000000011111111111111111111111",
                "00000000000000000000000000000000000000000000000000000000000000000000000000000000",
            };
            */
            for (int y = 0; y < 25; y++)
            {
                for (int x = 0; x < 80; x++)
                {
                    if (mapLoad[y][x].ToString() == "1")
                    {
                        newPoints.Add(new int[] { x, y });
                    }
                }
            }
            mapEmpty = newPoints.ToArray();
        }
        public void LoadMapSimple(bool drawBridge)
        {
            List<int[]> newPoints = new List<int[]>();
            for (int i = 0; i < Console.WindowWidth - 20; i++)
            {
                newPoints.Add(new int[] { i, 13 });
            }
            newPoints.Add(new int[] { Console.WindowWidth - 20, 13 });
            for (int i = 0; i < 22; i++)
            {
                newPoints.Add(new int[] { Console.WindowWidth - (i - 1), 14 });
            }
            newPoints.Add(new int[] { 50, 14 });
            newPoints.Add(new int[] { 50, 15 });
            newPoints.Add(new int[] { 50, 16 });
            newPoints.Add(new int[] { 50, 17 });
            for (int i = 0; i < 31; i++)
            {
                newPoints.Add(new int[] { Console.WindowWidth - (i - 1), 17 });
            }
            int bridgeHeight = 5;
            if (drawBridge)
            {
                for (int k = 0; k < bridgeHeight; k++)
                {
                    newPoints.Add(new int[] { 20, 12 - k });
                    newPoints.Add(new int[] { 40, 12 - k });
                }
                for (int i = 0; i < 20; i++)
                {
                    newPoints.Add(new int[] { 20 + i, 12 - (bridgeHeight - 1) });
                }
            }
            mapEmpty = newPoints.ToArray();
        }
        public void Draw()
        {
            Console.ForegroundColor = wallColour;
            Console.BackgroundColor = wallColour;
            for (int x = 0; x < Console.WindowWidth; x++)
            {
                for (int y = 0; y < Console.WindowHeight; y++)
                {
                    int[] toAdd = new int[] { x, y };
                    if (!(mapEmpty.Any(toAdd.SequenceEqual)))
                    {
                        if ((x == Console.WindowWidth - 1) && (y == Console.WindowHeight - 1))
                        {
                            continue;
                        }
                        Console.SetCursorPosition(toAdd[0], toAdd[1]);
                        Console.Write("#");
                    }
                }
            }
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;
        }
    }
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
}