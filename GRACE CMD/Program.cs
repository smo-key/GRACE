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
        public override bool Equals(object obj)
        {
            return ((Point)obj == this);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public static bool operator !=(Point a, Point b)
        {
            if ((a.x == b.x) && (a.y == b.y))
            { return false; }
            return true;
        }

        public static bool operator ==(Point a, Point b)
        {
            if ((a.x == b.x) && (a.y == b.y))
            { return true; }
            return false;
        }
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

        /// <summary>
        /// GPS Data including time, in a specific boxed area
        /// </summary>
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
                this.boxcenter = GetCoord(lonbox, latbox);
                this.boxsize = GetSize(lonbox, latbox);
                this.area = new AreaBox(coerce(boxcenter.x - (boxsize.x / 2), 0, 360), coerce(boxcenter.x + (boxsize.x / 2), 0, 360),
                        coerce(boxcenter.y + (boxsize.y / 2), -90, 90), coerce(boxcenter.x + (boxsize.y / 2), -90, 90));
            }

            static int GetGridLoc(double n)
            {
                return Math.Sign(n) * ((int)Math.Floor((Math.Abs(n) / gridsize) - 0.5d) + 1);
            }
            static Point GetCoord(double boxlon, double boxlat)
            {
                double lon = gridsize * boxlon;
                double lat = gridsize * boxlat;
                return new Point(lon, lat);
            }
            static Point GetSize(double boxlon, double boxlat)
            {
                //gridsize - extra space that was cut
                double w = gridsize - (boxlon - coerce(boxlon, 0, 360));
                double h = gridsize - (boxlat - coerce(boxlat, -90, 90));
                return new Point(w, h);
            }
            public static int BinsLon { get { return (int)Math.Ceiling(360 / gridsize) + 1; } }
            public static int BinsLat { get { return (int)Math.Ceiling(180 / gridsize) + 1; } }
            public static int BinLatCenter { get { return (BinsLat - 1) / 2; } }
            
            public GPSData data;
            public AreaBox area;
            public int lonbox;
            public int latbox;
            public Point boxcenter;
            public Point boxsize;
            public Satellite sat;
        }

        struct PointTime
        {
            public PointTime(Point point, DateTime time)
            {
                this.point = point;
                this.time = time;
            }
            public Point point;
            public DateTime time;

            public override bool Equals(object obj)
            {
                return ((PointTime)obj == this);
            }
            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
            public static bool operator !=(PointTime a, PointTime b)
            {
                if ((a.point == b.point) && (a.time == b.time))
                { return false; }
                return true;
            }
            public static bool operator == (PointTime a, PointTime b)
            {
                if ((a.point == b.point) && (a.time == b.time))
                { return true; }
                return false;
            }
        }

        static void Main(string[] args)
        {
            //*** COUNT BINS ***//

            PointTime lasttime = new PointTime();

            //*** INITIALIZE BINS ***//

            /* 
             * GOAL
             * List<GPSBoxed> shows location over time for time plots
             * 
             * Point = center of bin location
             * DateTime = time of entry
             * GPSBoxed = same bin location with time data and points
             * 
             * Dictionary<Point, GPSBoxed>
             * 
             */

            //*** READS TEXT FILES and find the bin ***//

            Dictionary<PointTime, List<GPSBoxed>> boxlist =
                new Dictionary<PointTime, List<GPSBoxed>>();

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

                PointTime current = new PointTime(box.boxcenter, data.time);
                
                if (lasttime.point == current.point)
                {
                    //if already inside the area
                    boxlist.First((e) =>
                    {
                        if (e.Key == lasttime)
                        {
                            return true;
                        }
                        return false;
                    }).Value.Add(box);
                }
                else
                {
                    //if not yet inside area
                    List<GPSBoxed> lbox = new List<GPSBoxed>();
                    lbox.Add(box);
                    boxlist.Add(current, lbox);
                    lasttime = current;
                }
                
            }

            return;
        }

        static double coerce(double value, double min, double max)
        {
            return Math.Max(min, Math.Min(max, value));
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
