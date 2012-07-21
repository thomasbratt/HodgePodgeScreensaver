
namespace HodgePodge
{
    /// <summary>
    /// Generates a 256 entry monochrome blue colour palette in ARGB format.
    /// </summary>
    public static class MonochromeBluePalette
    {
        public static int[] Generate()
        {
            var palette = new int[256];

            for (int index = 0; index < 256; ++index)
            {
                palette[index] = unchecked((int)0xFF000000 | index);
            }

            return palette;
        }
    }
}
