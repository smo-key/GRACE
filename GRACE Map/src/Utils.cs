using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRACEMap
{
    public class Utils
    {
        public struct HSV
        {
            public HSV(double h, double s, double v)
            {
                this.h = h;
                this.s = s;
                this.v = v;
            }
            double h, s, v;
        }

        public struct RGB
        {
            public RGB(double r, double g, double b)
            {
                this.r = r;
                this.g = g;
                this.b = b;
            }
            double r, g, b;
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

            return new HSV(h, s, v);
        }
    }
}
