using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRACE_CMD
{
    /// <summary>
    /// Structures class
    /// </summary>
    public class Structs
    {
        /// <summary>
        /// GPS Data including time, in a specific boxed area
        /// </summary>
        public struct GPSBoxed
        {
            /// <summary>
            /// Initialize a GPS Data coordinate to a boxed location
            /// </summary>
            /// <param name="data">GPSData information from satellite</param>
            /// <param name="sat">Satellite enumeration</param>
            public GPSBoxed(GPSData data, Satellite sat)
            {
                this.data = data;
                this.bin = new CoercedBin(data, sat);
            }

            public CoercedBin bin; //The bounds of the resulting box
            public GPSData data; //GPS data information
        }
        
        /// <summary>
        /// A rectangle coerced be a certain size and within coerced positions
        /// Uses gridsize to determine size
        /// </summary>
        public struct CoercedBin
        {
            public CoercedBin(GPSData data, Satellite sat)
            {
                this.entry = data.time;
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
                this.boxcenter = GetCenter(lonbox, latbox);
                this.boxsize = GetSize(boxcenter.x, boxcenter.y);
                AreaBox box = new AreaBox(Utils.coerce(boxcenter.x - (boxsize.x / 2), 0, 360), Utils.coerce(boxcenter.x + (boxsize.x / 2), 0, 360),
                    Utils.coerce(boxcenter.y - (boxsize.y / 2), -90, 90), Utils.coerce(boxcenter.y + (boxsize.y / 2), -90, 90));
                this.box = box;
            }

            public int lonbox; //X-axis box location
            public int latbox; //Y-axis box location
            public Point boxcenter; //Center of box in degrees
            public Point boxsize; //Size of box as (width, height)
            public AreaBox box; //Underlying box
            public DateTime entry; //Time of entry
            public Satellite sat; //Satellite
            
            /// <summary>
            /// Get integer location of grid from degree value
            /// </summary>
            /// <param name="n">Degrees</param>
            /// <returns>Grid location, as an integer</returns>
            /// <remarks>Grid location MAY BE NEGATIVE!</remarks>
            public static int GetGridLoc(double n)
            {
                return Math.Sign(n) * ((int)Math.Floor((Math.Abs(n) / Globals.gridsize) - 0.5d) + 1);
            }
            /// <summary>
            /// Get the center coordinate from a box location
            /// </summary>
            /// <param name="boxlon">X value of box location</param>
            /// <param name="boxlat">Y value of box location</param>
            /// <returns></returns>
            public static Point GetCenter(int boxlon, int boxlat)
            {
                double lon = Globals.gridsize * (double)boxlon;
                double lat = Globals.gridsize * (double)boxlat;
                return new Point(lon, lat);
            }
            /// <summary>
            /// Get size of the box from a longitude and latitude
            /// </summary>
            /// <param name="boxlon">Longitude of box in degrees</param>
            /// <param name="boxlat">Latitude of box in degrees</param>
            /// <returns></returns>
            public static Point GetSize(double boxlon, double boxlat)
            {
                //Globals.gridsize - extra space that was cut
                double w = Globals.gridsize - (boxlon - Utils.coerce(boxlon, 0, 360));
                double h = Globals.gridsize - (boxlat - Utils.coerce(boxlat, -90, 90));
                return new Point(w, h);
            }
            /// <summary>
            /// Count of bins on longitude axis
            /// </summary>
            public static int BinsLon { get { return (int)Math.Ceiling(360 / Globals.gridsize) + 1; } }
            /// <summary>
            /// Count of bins on latitude axis
            /// </summary>
            public static int BinsLat { get { return (int)Math.Ceiling(180 / Globals.gridsize) + 1; } }
            /// <summary>
            /// Center bin on latitude axis
            /// </summary>
            public static int BinLatCenter { get { return (BinsLat - 1) / 2; } }
        }

        /// <summary>
        /// A fully defined rectangle
        /// </summary>
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

            /// <summary>
            /// Anchor of box
            /// </summary>
            public enum Anchor { TopLeft, Center }

            public Point topleft, topright, bottomleft, bottomright, center;
            public double width, height, x, y;
            public Anchor anchortype;
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

            public DateTime time; ///Date and time
            public double latA, lonA, altA, latB, lonB, altB;
        }

        /// <summary>
        /// Satellite enumeration
        /// </summary>
        public enum Satellite { GraceA, GraceB }

        /// <summary>
        /// A point with 2 dimensions
        /// </summary>
        public struct Point
        {
            /// <summary>
            /// Initialize a point structure
            /// </summary>
            /// <param name="x">X-value</param>
            /// <param name="y">Y-value</param>
            public Point(double x, double y)
            {
                this.x = x;
                this.y = y;
            }
            public double x, y;
            /// <summary>
            /// Test for object equality
            /// </summary>
            /// <param name="obj">Object</param>
            /// <returns>True is equal</returns>
            public override bool Equals(object obj)
            {
                return ((Point)obj == this);
            }
            /// <summary>
            /// Get object's hash code
            /// </summary>
            /// <returns>Integer hash code</returns>
            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
            /// <summary>
            /// Test for inequality
            /// </summary>
            /// <param name="a">Point A</param>
            /// <param name="b">Point B</param>
            /// <returns>True is not equal</returns>
            public static bool operator !=(Point a, Point b)
            {
                if ((a.x == b.x) && (a.y == b.y))
                { return false; }
                return true;
            }
            /// <summary>
            /// Test of equality
            /// </summary>
            /// <param name="a">Point A</param>
            /// <param name="b">Point B</param>
            /// <returns>True if equal</returns>
            public static bool operator ==(Point a, Point b)
            {
                if ((a.x == b.x) && (a.y == b.y))
                { return true; }
                return false;
            }
        }

        /// <summary>
        /// A location associated with a time
        /// </summary>
        public struct PointTime
        {
            /// <summary>
            /// Initialize a PointTime class from a point and time
            /// </summary>
            /// <param name="point">Point as x,y</param>
            /// <param name="time">Time as DateTime</param>
            public PointTime(Point point, DateTime time)
            {
                this.point = point;
                this.time = time;
            }
            public Point point;
            public DateTime time;

            /// <summary>
            /// Test for equality
            /// </summary>
            /// <param name="obj">Object to test</param>
            /// <returns>True if equal</returns>
            public override bool Equals(object obj)
            {
                return ((PointTime)obj == this);
            }
            /// <summary>
            /// Get object hash code
            /// </summary>
            /// <returns>Integer hash code</returns>
            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
            /// <summary>
            /// Test for inequality
            /// </summary>
            /// <param name="a">PointTime A</param>
            /// <param name="b">PointTime B</param>
            /// <returns>True if not equal</returns>
            public static bool operator !=(PointTime a, PointTime b)
            {
                if ((a.point == b.point) && (a.time == b.time))
                { return false; }
                return true;
            }
            /// <summary>
            /// Test for equality
            /// </summary>
            /// <param name="a">PointTime A</param>
            /// <param name="b">PointTime B</param>
            /// <returns>True if equal</returns>
            public static bool operator ==(PointTime a, PointTime b)
            {
                if ((a.point == b.point) && (a.time == b.time))
                { return true; }
                return false;
            }
        }
    }
}
