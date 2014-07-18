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
        public AreaBox(double xmin, double xmax, double ymin, double ymax)
        {
            this.topleft = new Point(xmin, ymin);
            this.topright = new Point(xmax, ymin);
            this.bottomleft = new Point(xmin, ymax);
            this.bottomright = new Point(xmax, ymax);
            this.center = new Point((xmax - xmin) / 2, (ymax - ymin) / 2);
            this.anchortype = Anchor.TopLeft;
            this.width = xmax - xmin;
            this.height = ymax - ymin;
            this.x = xmin;
            this.y = ymin;
            return;
        }

        public Point topleft, topright, bottomleft, bottomright, center;
        public double width, height, x, y;
        public Anchor anchortype;
    }

    class Program
    {
        static readonly double gridsize = 10; //degrees

        static int GetGridLoc(double n)
        {
            return (int)Math.Floor((n / gridsize) - 0.5d) + 1;
        }

        static Point GetCoord(double boxlon, double boxlat, int boxlatcenter)
        {
            double lon = gridsize * boxlon;
            double lat = gridsize * (boxlat);
            return new Point(lon, lat);
        }
        static Point GetSize(double boxlon, double boxlat)
        {
            //gridsize - extra space that was cut
            double w = gridsize - (boxlon - coerce(boxlon, 0, 360));
            double h = gridsize - (boxlat - coerce(boxlat, -90, 90));
            return new Point(w, h);
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
            //*** COUNT BINS ***//

            int binslon = (int)Math.Ceiling(360 / gridsize) + 1; //x (0 to 360)  +1 accounts for centering
            int binslat = (int)Math.Ceiling(180 / gridsize) + 1; //y (-90 to 90) +1 accounts for centering
            int binlatcenter = (binslat - 1) / 2;
            Point lastcenter = new Point();

            //*** INITIALIZE BINS ***//
            List<AreaBox>[,] bins = new List<AreaBox>[binslon, binslat];
            for (int i = 0; i < binslon; i++)
            {
                for (int j = 0; j < binslat; j++)
                {
                    bins[i, j] = new List<AreaBox>();
                }
            }

            //*** READS TEXT FILES and find the bin ***//
            List<GPSBoxed> GPSlist = new List<GPSBoxed>();
            StreamReader reader = new StreamReader("../../../../gracedata/2002-04-05.1579023002.latlon");
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
                GPSBoxed box = new GPSBoxed(data, Satellite.GraceA);

                //remove area if already in it
                Point boxcenter = GetCoord(box.lonbox, box.latbox, binlatcenter);
                if ((lastcenter.x == boxcenter.x) && (lastcenter.y == boxcenter.y)) { continue; }
                GPSlist.Add(box);
                lastcenter = boxcenter;

                if (box.sat == Satellite.GraceA)
                {
                    Point boxsize = GetSize(box.lonbox, box.latbox);
                    AreaBox area = new AreaBox(coerce(boxcenter.x - (boxsize.x / 2), 0, 360), coerce(boxcenter.x + (boxsize.x / 2), 0, 360),
                        coerce(boxcenter.y + (boxsize.y / 2), -90, 90), coerce(boxcenter.x + (boxsize.y / 2), -90, 90));
                    bins[box.lonbox, binlatcenter + box.latbox].Add(area);
                }
            }

            return;
        }

        static double coerce(double value, double min, double max)
        {
            return Math.Max(min, Math.Min(max, value));
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
