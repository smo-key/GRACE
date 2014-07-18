using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace GRACE_CMD
{
    struct GPSData
    {
        public GPSData(DateTime time, double latA, double lonA, double altA,
            double latB, double lonB, double altB)
        {
            this.time = time;
            this.latA = latA;
            this.lonA = lonA;
            this.altA = altA;
            this.latB = latB;
            this.lonB = lonB;
            this.altB = altB;
        }

        public DateTime time;
        public double latA, lonA, altA, latB, lonB, altB;
    }

    enum Satellite { GraceA, GraceB }

    struct Point
    {
        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        public double x, y;
    }

    enum Anchor { TopLeft, Center }

    struct AreaBox
    {
        public AreaBox(double x, double y, double width, double height, Anchor anchor)
        {
            if (anchor == Anchor.TopLeft)
            {
                this.topleft = new Point(x, y);
                this.topright = new Point(x + width, y);
                this.bottomleft = new Point(x, y + height);
                this.bottomright = new Point(x + width, y + height);
                this.center = new Point(x + (width/2), y + (height/2));
                this.width = width;
                this.height = height;
                this.anchortype = Anchor.TopLeft;
                this.x = x;
                this.y = y;
                return;
            }
            if (anchor == Anchor.Center)
            {
                this.topleft = new Point(x - (width/2), y - (height/2));
                this.topright = new Point(x + (width/2), y - (height/2));
                this.bottomleft = new Point(x - (width/2), y + (height/2));
                this.bottomright = new Point(x + (width/2), y + (height/2));
                this.center = new Point(x, y);
                this.anchortype = Anchor.Center;
                this.width = width;
                this.height = height;
                this.x = x;
                this.y = y;
                return;
            }
            this.topleft = new Point();
            this.topright = new Point();
            this.bottomleft = new Point();
            this.bottomright = new Point();
            this.center = new Point();
            this.width = 0;
            this.height = 0;
            this.x = 0;
            this.y = 0;
            this.anchortype = Anchor.Center;
            return;
        }

        public Point topleft, topright, bottomleft, bottomright, center;
        public double width, height, x, y;
        public Anchor anchortype;
    }

    class Program
    {
        static readonly double gridsize = 3; //degrees

        static int GetGridLoc(double lonlat)
        {
            return (Math.Sign(lonlat) * (int)Math.Floor((Math.Abs(lonlat) + (gridsize / 2)) / gridsize));
        }

        struct GPSBoxed
        {
            public GPSBoxed(GPSData data, Satellite sat)
            {
                this.data = data;
                this.sat = sat;
                if (sat == Satellite.GraceA)
                {
                    this.lonbox = GetGridLoc(data.lonA);
                    this.latbox = GetGridLoc(data.latA);
                }
                else
                {
                    this.lonbox = GetGridLoc(data.lonB);
                    this.latbox = GetGridLoc(data.latB);
                }
            }
            public GPSData data;
            public int lonbox;
            public int latbox;
            public Satellite sat;
        }

        static void Main(string[] args)
        {
            //*** READS TEXT FILES and find the bin ***///
            List<GPSBoxed> GPSlist = new List<GPSBoxed>();

            



            List<AreaBox> Binlist = new List<AreaBox>();
            StreamReader reader = new StreamReader("../../../../gracedata/&combined.txt");
            while (!reader.EndOfStream)
            {
                string s = reader.ReadLine();
                string[] parameters = s.Split(' ');
                int count = parameters.Length;
                
                DateTime time = Utils.GetTime(Convert.ToDouble(parameters[0]));
                double latA = Convert.ToDouble(parameters[1]);
                double lonA = Convert.ToDouble(parameters[2]);
                double altA = Convert.ToDouble(parameters[3]);
                double latB = Convert.ToDouble(parameters[7]);
                double lonB = Convert.ToDouble(parameters[8]);
                double altB = Convert.ToDouble(parameters[9]);
                GPSData data = new GPSData(time, latA, lonA, altA, latB, lonB, altB);
                GPSlist.Add(new GPSBoxed(data, Satellite.GraceA));
            }

            return;
        }

        static List<AreaBox> ParseLongitude(double lon)
        {
            List<AreaBox> list = new List<AreaBox>();
            double lat = 0;
            while (lat < 90)
            {
                list.Add(new AreaBox(GetLongitude(lon), GetLatitude(lat), gridsize, gridsize, Anchor.Center));
                lat += gridsize;
            }
            lat = -gridsize;
            while (lat > -90)
            {
                list.Add(new AreaBox(GetLongitude(lon), GetLatitude(lat), gridsize, gridsize, Anchor.Center));
                lat -= gridsize;
            }
            return list;
        }

        static double GetLongitude(double lon)
        {
            if (lon > 360) { return 360; }
            if (lon < 0) { return 0; }
            return lon;
        }

        static double GetLatitude(double lat)
        {
            if (lat > 90) { return 90; }
            if (lat < -90) { return -90; }
            return lat;
        }
    }

    public static class Utils
    {
        public static DateTime GetTime(double secondsJuliet)
        {
            DateTime time = new DateTime(0, DateTimeKind.Utc);
            time = time.AddYears(1999);
            time = time.AddSeconds(secondsJuliet);
            return time;
        }
    }
}
