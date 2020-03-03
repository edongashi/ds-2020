using System;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace Steganography
{
    class Program
    {
        static void Main(string[] args)
        {
            // stegano write foto.png "Pershendetje"
            // stegano read foto.stegano.png

            if (args.Length < 2)
            {
                Usage();
                return;
            }

            string cmd = args[0];
            string filePath = args[1];
            Bitmap bitmap = new Bitmap(filePath);

            switch (cmd)
            {
                case "read":
                    Console.WriteLine(
                        ImageSteganography.Read(bitmap)
                    );

                    break;

                case "write":
                    if (args.Length < 3)
                    {
                        Usage();
                        return;
                    }

                    string text = args[2];
                    Bitmap newBitmap = ImageSteganography.Write(bitmap, text);

                    string newFilePath = ReplaceExtension(filePath, ".stegano.png");
                    newBitmap.Save(newFilePath, ImageFormat.Png);
                    break;

                default:
                    Usage();
                    return;
            }
        }

        static string ReplaceExtension(string path, string newExtension)
        {
            // foto.png -> foto.stegano.png
            string realPath = Path.GetFullPath(path);
            string directory = Path.GetDirectoryName(realPath);
            string fileName = Path.GetFileNameWithoutExtension(realPath);
            return Path.Combine(
                directory,
                fileName + newExtension
            );
        }

        static void Usage()
        {
            Console.WriteLine("Sintakse e gabuar.");
        }
    }
}
