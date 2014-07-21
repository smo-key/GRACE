using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRACEMap
{
    public class Utils
    {
        //*RGB->HSV*//
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
