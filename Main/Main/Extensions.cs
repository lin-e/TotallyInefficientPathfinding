namespace Main
{
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
                case Direction.UpLeft:
                    return new int[] { origin[0] - 1, origin[1] - 1 };
                case Direction.UpRight:
                    return new int[] { origin[0] + 1, origin[1] + 1 };
                case Direction.DownLeft:
                    return new int[] { origin[0] - 1, origin[1] + 1 };
                case Direction.DownRight:
                    return new int[] { origin[0] + 1, origin[1] + 1 };
            }
            return null;
        }
    }
}
