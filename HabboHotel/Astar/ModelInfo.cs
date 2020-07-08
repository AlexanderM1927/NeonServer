using System.Runtime.InteropServices;

namespace Neon.HabboHotel.Astar
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ModelInfo
    {
        private readonly byte[,] mMap;
        private readonly int mMaxX;
        private readonly int mMaxY;
        internal ModelInfo(int MaxX, int MaxY, byte[,] Map)
        {
            mMap = Map;
            mMaxX = MaxX;
            mMaxY = MaxY;
        }

        public byte GetState(int x, int y)
        {
            if ((x >= mMaxX) || (x < 0))
            {
                return 0;
            }
            if ((y >= mMaxY) || (y < 0))
            {
                return 0;
            }
            return mMap[x, y];
        }
    }
}