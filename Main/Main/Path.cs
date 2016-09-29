using System;
using System.Collections.Generic;
using System.Linq;

namespace Main
{
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
                return allPoints.Distinct().Count();
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
        public PointUpdate CalculateNext(bool showProgress, bool allowDiagonal, ConsoleColor progressColour)
        {
            if (!(parentEntity.usedPoints.Any(new int[] { currentPoint[0], currentPoint[1] }.SequenceEqual)))
            {
                parentEntity.usedPoints.Add(new int[] { currentPoint[0], currentPoint[1] });
            }
            if (showProgress)
            {
                Console.SetCursorPosition(currentPoint[0], currentPoint[1]);
                Console.ForegroundColor = progressColour;
                Console.BackgroundColor = progressColour;
                Console.Write("#");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.BackgroundColor = ConsoleColor.Black;
            }
            PointUpdate toReturn = new PointUpdate();
            if ((currentPoint[0] == mainPoints[1][0]) && (currentPoint[1] == mainPoints[1][1]))
            {
                toReturn.isEnd = true;
                return toReturn;
            }
            Direction[] allDirections = null;
            if (allowDiagonal)
            {
                allDirections = new Direction[] { Direction.Up, Direction.Down, Direction.Left, Direction.Right, Direction.UpLeft, Direction.UpRight, Direction.DownLeft, Direction.DownRight };
            }
            else
            {
                allDirections = new Direction[] { Direction.Up, Direction.Down, Direction.Left, Direction.Right };
            }
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
            foreach (int[] singlePoint in CompilePath().Distinct())
            {
                Console.SetCursorPosition(singlePoint[0], singlePoint[1]);
                Console.Write("#");
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;
        }
    }
}
