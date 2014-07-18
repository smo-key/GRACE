using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRACE_CMD
{
    public class Structs
    {
        /// <summary>
        /// GPS Data including time, in a specific boxed area
        /// </summary>
        public struct GPSBoxed
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
                AreaBox box = new AreaBox(Utils.coerce(boxcenter.x - (boxsize.x / 2), 0, 360), Utils.coerce(boxcenter.x + (boxsize.x / 2), 0, 360),
                        Utils.coerce(boxcenter.y - (boxsize.y / 2), -90, 90), Utils.coerce(boxcenter.y + (boxsize.y / 2), -90, 90));
                this.area = box;
            }

            static int GetGridLoc(double n)
            {
                return Math.Sign(n) * ((int)Math.Floor((Math.Abs(n) / Globals.gridsize) - 0.5d) + 1);
            }
            static Point GetCoord(double boxlon, double boxlat)
            {
                double lon = Globals.gridsize * boxlon;
                double lat = Globals.gridsize * boxlat;
                return new Point(lon, lat);
            }
            static Point GetSize(double boxlon, double boxlat)
            {
                //Globals.gridsize - extra space that was cut
                double w = Globals.gridsize - (boxlon - Utils.coerce(boxlon, 0, 360));
                double h = Globals.gridsize - (boxlat - Utils.coerce(boxlat, -90, 90));
                return new Point(w, h);
            }
            public static int BinsLon { get { return (int)Math.Ceiling(360 / Globals.gridsize) + 1; } }
            public static int BinsLat { get { return (int)Math.Ceiling(180 / Globals.gridsize) + 1; } }
            public static int BinLatCenter { get { return (BinsLat - 1) / 2; } }

            public GPSData data;
            public AreaBox area;
            public int lonbox;
            public int latbox;
            public Point boxcenter;
            public Point boxsize;
            public Satellite sat;
        }

        public struct GPSData
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

        public enum Satellite { GraceA, GraceB }

        public struct Point
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

        public enum Anchor { TopLeft, Center }

        public struct AreaBox
        {
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

        public struct PointTime
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
            public static bool operator ==(PointTime a, PointTime b)
            {
                if ((a.point == b.point) && (a.time == b.time))
                { return true; }
                return false;
            }
        }
    }
}
