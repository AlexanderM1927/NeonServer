using System.Runtime.InteropServices;

namespace Neon.HabboHotel.Astar
{
    [StructLayout(LayoutKind.Sequential)]
    public struct HeightInfo
    {
        private readonly double[,] mMap;
        private readonly int mMaxX;
        private readonly int mMaxY;

        public HeightInfo(int MaxX, int MaxY, double[,] Map)
        {
            mMap = Map;
            mMaxX = MaxX;
            mMaxY = MaxY;
        }

        public double GetState(int x, int y)
        {
            if ((x >= mMaxX) || (x < 0))
            {
                return 0.0;
            }
            if ((y >= mMaxY) || (y < 0))
            {
                return 0.0;
            }
            return mMap[x, y];
        }
    }
}