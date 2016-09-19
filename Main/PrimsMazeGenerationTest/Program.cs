using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrimsMazeGenerationTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Maze test = new Maze();
            Console.WriteLine(string.Join("", test.Export));
            Console.ReadLine();
        }
    }
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
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
    public class Maze
    {
        string[] allPoints;
        List<int[]> wallPoints = new List<int[]>();
        List<int[]> mazeCells = new List<int[]>();
        int gridHeight;
        int gridWidth;
        public Maze()
        {
            gridHeight = Console.WindowHeight - 2;
            gridWidth = Console.WindowWidth - 2;
            List<string> rawPoints = new List<string>();
            for (int y = 0; y < gridHeight; y++)
            {
                rawPoints.Add(string.Join("", Enumerable.Repeat("0", gridWidth)));
            }
            allPoints = rawPoints.ToArray();
        }
        public string[] Export
        {
            get
            {
                List<string> allLines = new List<string>();
                allLines.Add(string.Join("", Enumerable.Repeat("0", Console.WindowWidth)));
                foreach (string singleLine in allPoints)
                {
                    allLines.Add(string.Format("0{0}0", singleLine));
                }
                allLines.Add(string.Join("", Enumerable.Repeat("0", Console.WindowWidth)));
                return allLines.ToArray();
            }
        }
        public void Generate()
        {
            int[] startPoint = { 0, 0 };

        }
        List<int[]> getWalls(int[] currentPoint)
        {
            List<int[]> toReturn = new List<int[]>();
            return null;
        }
        bool isEmpty(int[] checkPoint)
        {
            try
            {
                return allPoints[checkPoint[1]][checkPoint[0]] == '0';
            }
            catch
            {
                return false;
            }
        }
    }
}
