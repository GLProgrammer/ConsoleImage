using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleImage
{
    class Program
    {
        static string imageLocation;
        static int resolutionX;
        static int resolutionY;

        static bool useDoubleChars = false;

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            GetImageLocation();
            Bitmap image;
            try
            {
                image = new Bitmap(imageLocation);
            }
            catch (Exception)
            {
                Console.WriteLine("Image isn´t valid!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"Width: {image.Width}; Height: {image.Height}");

            GetResolution();


            Console.Write("Do you want to use double character? Y/N: ");
            if (Console.ReadLine().ToLower() == "y")
                useDoubleChars = true;
            else
                useDoubleChars = false;


            //Console.Write("Do you want to save it as Image? Y/N");  //Add saving

            //string saveAsImage = Console.ReadLine().ToLower();

            Console.Write("Enter color offset (-40 is highly recomended!): ");
            int adder = int.Parse(Console.ReadLine());

            Console.WriteLine("Check if resolution and buffer is OK, zoom out and press ENTER");
            Console.ReadLine();

            Console.Clear();
            CreateImage(image, adder);

            while (true)
            {
                Console.Write("Change color offset: ");
                adder = int.Parse(Console.ReadLine());
                Console.Clear();
                CreateImage(image, adder);
            }

        }

        static void GetImageLocation()
        {
            Console.Write("Image location: ");
            imageLocation = Console.ReadLine();

            if (!File.Exists(imageLocation))
            {
                Console.WriteLine("File doesn´t exist!");
                Console.WriteLine();
                GetImageLocation();
            }
        }

        static void GetResolution()
        {
            Console.Write("Insert X resolution: ");
            resolutionX = int.Parse(Console.ReadLine());        //Add try-catch
            Console.Write("Insert Y resolution: ");
            resolutionY = int.Parse(Console.ReadLine());        //Add try-catch

            //Add check for right resolution
        }

        static void CreateImage(Bitmap image, int adder)
        {
            Console.WriteLine("Creating image....");
            Color color;
            int colorR;
            int colorG;
            int colorB;

            int num;

            for (int offsetY = 0; offsetY < image.Height; offsetY += resolutionY)
            {
                for (int offsetX = 0; offsetX < image.Width; offsetX += resolutionX)
                {
                    colorR = 0;
                    colorG = 0;
                    colorB = 0;

                    num = 0;

                    for (int y = offsetY; y < offsetY + resolutionY; y++)
                    {
                        for (int x = offsetX; x < offsetX + resolutionX; x++)
                        {
                            try
                            {
                                color = image.GetPixel(x, y);
                                colorR += color.R;
                                colorG += color.G;
                                colorB += color.B;
                                num++;
                            }
                            catch (Exception e)
                            {
                                //Out of bounds
                            }
                        }
                    }

                    if (num != 0)
                    {
                        colorR = (colorR + adder) / num;
                        colorG = (colorG + adder) / num;
                        colorB = (colorB + adder) / num;
                    }

                    if (colorR > 255)
                        colorR = 255;
                    if (colorG > 255)
                        colorG = 255;
                    if (colorB > 255)
                        colorB = 255;

                    if (colorR < 0)
                        colorR = 0;
                    if (colorG < 0)
                        colorG = 0;
                    if (colorB < 0)
                        colorB = 0;

                    color = Color.FromArgb(colorR, colorG, colorB);
                    Console.ForegroundColor = ClosestConsoleColor(color.R, color.G, color.B);
                    if (useDoubleChars)
                        Console.Write("##");
                    else
                        Console.Write('#');
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("\n");
            }
        }

        static ConsoleColor ClosestConsoleColor(byte r, byte g, byte b)
        {
            ConsoleColor ret = 0;
            double rr = r, gg = g, bb = b, delta = double.MaxValue;

            foreach (ConsoleColor cc in Enum.GetValues(typeof(ConsoleColor)))
            {
                var n = Enum.GetName(typeof(ConsoleColor), cc);
                var c = System.Drawing.Color.FromName(n == "DarkYellow" ? "Orange" : n); // bug fix
                var t = Math.Pow(c.R - rr, 2.0) + Math.Pow(c.G - gg, 2.0) + Math.Pow(c.B - bb, 2.0);
                if (t == 0.0)
                    return cc;
                if (t < delta)
                {
                    delta = t;
                    ret = cc;
                }
            }
            return ret;
        }

    }
}
