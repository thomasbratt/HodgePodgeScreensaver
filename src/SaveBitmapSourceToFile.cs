using System.IO;
using System.Windows.Media.Imaging;

namespace HodgePodge
{
    public static class SaveBitmapSourceToFile
    {
        public static void SaveAsJpg(this BitmapSource source, string fileName, int quality)
        {
            var encoder = new JpegBitmapEncoder();
            encoder.QualityLevel = quality;

            SaveBitmapSourceToFile.Save(fileName, encoder, source);
        }

        public static void SaveAsPng(this BitmapSource source, string fileName)
        {
            var encoder = new PngBitmapEncoder();

            SaveBitmapSourceToFile.Save(fileName, encoder, source);
        }

        private static void Save(string fileName, BitmapEncoder encoder, BitmapSource source)
        {
            BitmapFrame outputFrame = BitmapFrame.Create(source);
            encoder.Frames.Add(outputFrame);

            using (FileStream file = File.OpenWrite(fileName))
            {
                encoder.Save(file);
            }
        }
    }
}
