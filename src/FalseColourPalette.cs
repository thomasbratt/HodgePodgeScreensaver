
namespace HodgePodge
{
    /// <summary>
    /// Generates a 256 entry false colour palette in ARGB format.
    /// </summary>
    public static class FalseColourPalette
    {
        //	{102,..,255} in 51 steps of 3.
        private const int BrightnessStart = 105;
        private const int BrightnessStop = 255;
        private const int BrightnessIncrement = 3;

        public static int[] Generate()
        {
            var palette = new int[256];

            int index = 0;

            //	Black.
            palette[index++] = unchecked((int)0xFF000000);

            //	Blue.
            for (   int offset = BrightnessStart;
                    offset <= BrightnessStop;
                    offset += BrightnessIncrement)
            {
                palette[index++] = unchecked((int)0xFF000000 | offset);
            }

            //	Cyan.
            for (   int offset = BrightnessStart;
                    offset <= BrightnessStop;
                    offset += BrightnessIncrement)
            {
                palette[index++] = unchecked((int)0xFF000000 | (offset << 8) | (offset));
                // printf("%d %d %d\n", 0, offset, offset);
            }

            //	Green.
            for (   int offset = BrightnessStart;
                    offset <= BrightnessStop;
                    offset += BrightnessIncrement)
            {
                palette[index++] = unchecked((int)0xFF000000 | (offset << 8));
                // printf("%d %d %d\n", 0, offset, 0);
            }

            //	Yellow.
            for (   int offset = BrightnessStart;
                    offset <= BrightnessStop;
                    offset += BrightnessIncrement)
            {
                palette[index++] = unchecked((int)0xFF000000 | (offset << 16) | (offset << 8));
                // printf("%d %d %d\n", offset, offset, 0);
            }

            //	Red.
            for (   int offset = BrightnessStart;
                    offset <= BrightnessStop;
                    offset += BrightnessIncrement)
            {
                palette[index++] = unchecked((int)0xFF000000 | (offset << 16));
                // printf("%d %d %d\n", offset, 0, 0);
            }

            return palette;
        }
    }
}
