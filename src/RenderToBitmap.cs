using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HodgePodge
{
    /// <summary>
    /// Renders an array of integer values to a image.
    /// </summary>
    public class RenderToBitmap
    {        
        private int width;
        private int height;
        private int numberOfStates;
        private int[] palette;
        private int[] pixels;

        public RenderToBitmap(int width, int height, int numberOfStates, int[] palette)
        {
            this.width = width;
            this.height = height;
            this.numberOfStates = numberOfStates;
            this.palette = palette;
            this.pixels = new int[width * height];
        }

        public WriteableBitmap Render(int[] values)
        {
            if (values.Length != this.pixels.Length)
            {
                throw new ArgumentException("Number of raw values must equal width*height");
            }

            // Convert raw values to ARGB format pixels.
            this.ConvertToPixels(values);

            // Copy pixels to frame buffer.
            var bitmap = new WriteableBitmap(   this.width,
                                                this.height,
                                                0,
                                                0,
                                                PixelFormats.Pbgra32,
                                                null);
            int widthInBytes = 4 * this.width;
            var sourceRect = new Int32Rect(0, 0, this.width, this.height);
            bitmap.WritePixels( sourceRect,
                                pixels,
                                widthInBytes,
                                0);

            return bitmap;
        }

        // Convert raw values to ARGB format pixels.
        // 0xFF0000FF blue
        // 0xFF00FF00 green
        // 0xFFFF0000 red
        private void ConvertToPixels(int[] values)
        {
            for (int y = 0; y < this.height; ++y)
            {
                for (int x = 0; x < this.width; ++x)
                {
                    int raw = values[x + y * this.width];
                    raw = (255 * raw) / (this.numberOfStates-1);

                    // Blue.
                    // pixels[x + y * this.width] = unchecked((int)0xFF000000 | raw); 

                    pixels[x + y * this.width] = palette[raw];
                }
            }
        }
    }
}
