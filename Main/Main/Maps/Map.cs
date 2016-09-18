using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Main
{
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
            mapEmpty = Preset.Load(Preset.EXAMPLE_01).ToArray();
        }
        public void LoadMapMaze()
        {
            mapEmpty = Preset.Load(Preset.MAZE_01).ToArray();
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
}
