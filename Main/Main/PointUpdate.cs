using System.Collections.Generic;

namespace Main
{
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