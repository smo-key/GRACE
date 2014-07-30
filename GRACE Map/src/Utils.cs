using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRACEdata;
using System.Drawing;

namespace GRACEMap
{
    public class Utils
    {
        /// <summary>
        /// Takes a CoercedBin location (0 to 360 and -90 to 90) and converts it to a location on the map (800x400)
        /// </summary>
        /// <param name="bin"></param>
        /// <returns></returns>
        public static RectangleF BinToMap(Structs.AreaBox box)
        {
            // 20/9 = scale factor in all directions
            float x = (float)((box.topleft.x / 9) * 20);
            float y = (float)(((box.topleft.y + 90) / 9) * 20);
            float w = (float)(Math.Abs(box.topright.x - box.topleft.x) * 20 / 9);
            float h = (float)(Math.Abs((box.bottomleft.y + 90) - (box.topleft.y + 90)) * 20 / 9);
            RectangleF rect = new RectangleF(x, y, w, h);
            return rect;
        }

        /// <summary>
        /// Blue to red scale, blue = 0, red = 100
        /// </summary>
        /// <param name="value">A value between 0 and 100</param>
        /// <param name="alpha">Alpha value of the color</param>
        /// <param name="max">maximum number of passes</param>
        /// <returns>System.Drawing.Color output</returns>
        public static System.Drawing.Color BlueToRedScale(double value, int max, double sensitivity, int alpha)
        {
            double x = value;
            double a = 1 / (10 - sensitivity);
            double b = 100 / Math.Pow((double)max, a);
            double y = b * Math.Pow(x, a);
            HSV color = new HSV(y, 100, 100);
            RGB output = HSVtoRGB(color.h, color.s, color.v);
            return System.Drawing.Color.FromArgb(alpha, (int)(output.r * 255), (int)(output.g * 255), (int)(output.b * 255));
        }

        /// <summary>
        /// Blue to red scale, blue = 0, red = 100
        /// </summary>
        /// <param name="value">A value between 0 and 100</param>
        /// <param name="max">maximum number of passes</param>
        /// <returns>System.Drawing.Color output</returns>
        public static System.Drawing.Color BlueToRedScale(double value, int max, double sensitivity)
        {
            double x = value;
            double a = 1 / (10 - sensitivity);
            double b = 100 / Math.Pow((double)max, a);
            double y = GRACEdata.Utils.coerce(b * Math.Pow(x, a), 0, 100);
            //HSV color = new HSV(y, 100, 100);
            HSV color = new HSV(y / 3 + 100, 100, 100);
            //HSV color = new HSV(0, y, 100);
            RGB output = HSVtoRGB(color.h, color.s, color.v);
            int alpha = (int)Math.Floor((GRACEdata.Utils.coerce(y, 0, 100)) * 255 / 100);
            return System.Drawing.Color.FromArgb(alpha, (int)(output.r * 255), (int)(output.g * 255), (int)(output.b * 255));
        }

        //*RGB->HSV*//
        public struct HSV
        {
            public HSV(double h, double s, double v)
            {
                this.h = h;
                this.s = s;
                this.v = v;
            }
            public double h, s, v;
        }

        public struct RGB
        {
            public RGB(double r, double g, double b)
            {
                this.r = r;
                this.g = g;
                this.b = b;
            }
            public double r, g, b;
        }
        public static HSV RGBtoHSV(double r, double g, double b)
        {
            double red = r / 255;
            double green = g / 255;
            double blue = b / 255;
            double max = (double)(Math.Max(red, Math.Max(green, blue)));
            double min = (double)(Math.Min(red, Math.Min(green, blue)));
            double delta = max - min;
            double h;
            double s;
            double v = max;

            //*FIND HUE*//
            if (max == red)
            {
                if(green >= blue)
                {
                    if (delta == 0)
                    {
                        h = 0;
                    }

                    else 
                    {
                        h = 60 * Math.Abs(green - blue) / delta;
                    }
                }

                else
                {
                    h = 60 * Math.Abs(green - blue) / (delta + 360);
                }
            }

            else if(max == green)
            {
                h = 60 * Math.Abs(blue - red) / (delta + 120);
            }

            else
            {
                h = 60 * Math.Abs(red - green) / (delta + 240);
            }

            //*FIND SATURATION*//
            if(max == 0)
            {
                s = 0;
            }

            else
            {
                s = 1 - (min / max);
            }

            h *= 100;
            s *= 100;
            return new HSV(h, s, v);
        }

        //*HSV->RGB*//
        public static RGB HSVtoRGB(double h, double s, double v)
        {
            double hue = h;
            double sat = s / 100;
            double val = v / 100;
            double r;
            double g;
            double b;

            if (sat == 0)
            {
                r = g = b = val;
            }

            else
            {
                double sectorPos = hue / 60;
                int sectorNum = (int)(Math.Floor(sectorPos));
                double fractionalSector = sectorPos - sectorNum;
                double p = val * (1 - sat);
                double q = val * (1 - (sat * fractionalSector));
                double t = val * (1 - (sat * (1 - fractionalSector)));
                if(sectorNum == 0 || sectorNum == 6)
                {
                    r = val;
                    g = t;
                    b = p;
                }
                else if (sectorNum == 1)
                {
                    r = q;
                    g = val;
                    b = p;
                }
                else if (sectorNum == 2)
                {
                    r = p;
                    g = val;
                    b = t;
                }
                else if (sectorNum == 3)
                {
                    r = p;
                    g = q;
                    b = val;
                }
                else if (sectorNum == 4)
                {
                    r = t;
                    g = p;
                    b = val;
                }
                else
                {
                    r = val;
                    g = p;
                    b = q;
                }
            }

            return new RGB(r, g, b);
        }
    }
}
