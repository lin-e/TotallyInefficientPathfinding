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
            Console.WindowWidth = 81;
            Console.WindowHeight = 25;
            bool keepLooping = true;
            while (keepLooping)
            {
                Console.Clear();
                Map mainMap = new Map(ConsoleColor.White);
                int mazeMode = 5;
                Console.Title = "";
                switch (mazeMode)
                {
                    case 0:
                        mainMap.LoadMapComplex();
                        break;
                    case 1:
                        mainMap.LoadMapGrid();
                        break;
                    case 2:
                        mainMap.LoadMapEmpty();
                        break;
                    case 3:
                        mainMap.LoadMapMaze();
                        break;
                    case 4:
                        mainMap.mapEmpty = Preset.Load(Preset.EXAMPLE_02).ToArray();
                        break;
                    case 5:
                        Stopwatch mazeGenTime = new Stopwatch();
                        mazeGenTime.Start();
                        mainMap.LoadNewMaze(Console.WindowHeight * Console.WindowWidth * (int)Math.Pow(2, 10));
                        mazeGenTime.Stop();
                        Console.Title += string.Format("Generated maze in {0}ms. ", mazeGenTime.ElapsedMilliseconds);
                        break;
                }
                mainMap.Draw();
                Entity testEntity = new Entity(mainMap, new int[] { 1, 1 }, new int[] { 79, 23 });
                Stopwatch mainTimer = new Stopwatch();
                mainTimer.Start();
                if (testEntity.SolvePath(true, false, ConsoleColor.DarkCyan, 1))
                {
                    mainTimer.Stop();
                    testEntity.End.Draw();
                    Console.Title += string.Format("Solved map in {0}ms (Length: {1})", mainTimer.ElapsedMilliseconds, testEntity.End.Length);
                }
                else
                {
                    mainTimer.Stop();
                    Console.Title += string.Format("Could not solve map in {0}ms", mainTimer.ElapsedMilliseconds);
                }
                int[] entityStart = testEntity.FindStart();
                int[] entityEnd = testEntity.FindEnd();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.BackgroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(entityStart[0], entityStart[1]);
                Console.Write("#");
                Console.SetCursorPosition(entityEnd[0], entityEnd[1]);
                Console.Write("#");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.BackgroundColor = ConsoleColor.Black;
                if (keepLooping)
                {
                    Thread.Sleep(1000);
                }
            }
            Console.ReadLine();
        }
    }
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        UpLeft,
        UpRight,
        DownLeft,
        DownRight
    }
}