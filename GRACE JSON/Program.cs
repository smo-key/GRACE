using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using GRACEdata;

namespace GRACE_JSON
{
    class Program
    {
        static string jsonfile = "../../../../output/monthdata.json";
        static double gridsize = 3.0d;
        static int filen = 0;
        static int filescount = 0;

        static void Main(string[] args)
        {
            //Goal: [lat, lon, value (0-1), ...]
            /*var data = [
                [
                'seriesA', [ latitude, longitude, magnitude, latitude, longitude, magnitude, ... ]
                ],
                [
                'seriesB', [ latitude, longitude, magnitude, latitude, longitude, magnitude, ... ]
                ]
            ];*/

            Console.WriteLine("GRACE Data Automated JSON Converter");
            Console.Write("\r\nType binsize (degrees): ");
            gridsize = Convert.ToDouble(Console.ReadLine());
            Console.Write("Continuing will DELETE all currently created data! [y/n] ");
            if (Console.ReadKey().Key != ConsoleKey.Y) { return; }

            //Prepare JSON
            Console.WriteLine("\r\nInitializing...");
            File.Delete(jsonfile);
            StreamWriter json = new StreamWriter(jsonfile);
            json.AutoFlush = true;
            json.Write("[\r\n");

            //** SET SETTINGS **//
            Globals.gridsize = gridsize;
            Structs.Anchor anchor = Structs.Anchor.Uniform;
            if (!(360 % Globals.gridsize == 0)) { anchor = Structs.Anchor.Center; }

            //** GET LIST OF YEAR / MONTH **//
            List<string> ym = new List<string>();
            string[] list = Directory.GetFiles("../../../../../gracedata/groundtrack/", "*.latlon", SearchOption.TopDirectoryOnly);
            foreach (string file in list)
            {
                FileInfo fi = new FileInfo(file);
                if (!ym.Contains(fi.Name.Substring(0, 7)))
                {
                    ym.Add(fi.Name.Substring(0, 7));
                }
            }
            filen = 0;
            filescount = list.Length;

            int max = FindMax();
            bool firstd = true;

            foreach (string f in ym)
            {
                //** GET FILE COUNT **//
                string[] files = Directory.GetFiles("../../../../../gracedata/groundtrack/", f + "*.latlon", SearchOption.TopDirectoryOnly);

                //** READ ALL FILES **//
                int[,] bins = GetData(files);

                if (!firstd) { json.Write(",\r\n"); } else { firstd = false; }

                json.WriteLine("    [");
                json.Write("\"" + f + "\", [");
                bool first = true;

                for (int i = 0; i < Structs.CoercedBin.BinsLon; i++)
                {
                    for (int j = 0; j < Structs.CoercedBin.BinsLat; j++)
                    {
                        int k = j - Structs.CoercedBin.BinLatCenter;
                        Structs.Point c = Structs.CoercedBin.GetCenter(i, k);

                        
                        System.Drawing.Color col = GRACEMap.Utils.BlueToRedScale(bins[i, j], max, 3.5d);
                        double value = (double)col.A / 255.0d;

                        if (Math.Abs(c.y) != 90)
                        {
                            if (!first) { json.Write(","); } else { first = false; }
                            json.Write("{0},{1},{2}", c.y.ToString(), c.x.ToString(), value.ToString("F3"));
                        }

                        //log.WriteLine("{0} | {1} {2} | {3} {4} {5} {6}", bins[i, j].ToString(), i.ToString(), k.ToString(),
                        //    box.topleft.x.ToString("F3"), box.topleft.y.ToString("F3"), box.bottomright.x.ToString("F3"), box.bottomright.y.ToString("F3"));
                    }
                }

                json.Write("]\r\n" +
                           "    ]");
            }

            json.Write("\r\n]");
            json.Close();
            Console.WriteLine("\r\nWrite completed!");
            Console.WriteLine("Press enter to continue...");
            Console.ReadKey(true);
        }

        public static int FindMax()
        {
            //** SET SETTINGS **//
            Globals.gridsize = gridsize;

            string maxestext = "../../../../output/maxmonth.txt";

            //*DOES MAXES.TXT EXIST*//

            if (!File.Exists(maxestext))
            {
                FileStream stream = File.Create(maxestext);
                stream.Close();
            }
            int maximum = 0;

            //*READ MAXES.TXT*//
            string[] lines = System.IO.File.ReadAllLines(maxestext);

            //*CHECK IF BINSIZE EXISTS*//
            foreach (string line in lines)
            {
                if (Convert.ToDouble(line.Split(' ')[0]) == Globals.gridsize)
                {
                    return Convert.ToInt32(line.Split(' ')[1]);
                }
            }

            //** GET LIST OF YEAR / MONTH **//
            List<string> ym = new List<string>();
            string[] list = Directory.GetFiles("../../../../../gracedata/groundtrack/", "*.latlon", SearchOption.TopDirectoryOnly);
            foreach (string file in list)
            {
                FileInfo fi = new FileInfo(file);
                if (!ym.Contains(fi.Name.Substring(0, 7)))
                {
                    ym.Add(fi.Name.Substring(0, 7));
                }
            }

            filen = 0;
            filescount = list.Length;

            foreach (string f in ym)
            {
                //** GET FILE COUNT **//
                string[] files = Directory.GetFiles("../../../../../gracedata/groundtrack/", f + "*.latlon", SearchOption.TopDirectoryOnly);

                //** READ ALL FILES **//
                int[,] bins = GetData(files);

                //** GET RANGE **//
                Console.WriteLine("Parsing range...");
                int max = 0;
                for (int a = 0; a < Structs.CoercedBin.BinsLon; a++)
                {
                    for (int b = 0; b < Structs.CoercedBin.BinsLat + 1; b++)
                    {
                        if (bins[a, b] > max) { max = bins[a, b]; }
                    }
                }
                if (maximum < max) { maximum = max; }
            }

            //WRITE MAX AND BINSIZE TO MAXES.TXT
            StreamWriter writer = new StreamWriter(maxestext);
            writer.WriteLine(String.Format("{0} {1}", Globals.gridsize, maximum));
            writer.Flush();
            writer.Close();
            return maximum; //largest single bin from largest month
        }

        private static int[,] GetData(string[] files)
        {
            Structs.PointTime lasttime = new Structs.PointTime();

            //** INITIALIZE BIN COUNTS **//
            int[,] bins = new int[Structs.CoercedBin.BinsLon, Structs.CoercedBin.BinsLat + 1];
            for (int i = 0; i < Structs.CoercedBin.BinsLon; i++)
            {
                for (int j = 0; j < Structs.CoercedBin.BinsLat + 1; j++)
                {
                    bins[i, j] = 0;
                }
            }

            //** READ ALL FILES **//
            foreach (string file in files)
            {
                Console.Clear();
                Console.Write(string.Format("Reading {0} [file {1} out of {2}]...", Path.GetFileName(file), filen, filescount));
                filen++;
                StreamReader reader = new StreamReader(file);
                while (!reader.EndOfStream)
                {
                    string s = reader.ReadLine();
                    string[] parameters = s.Split(' ');
                    int count = parameters.Length;

                    DateTime time = GRACEdata.Utils.GetTime(Convert.ToDouble(parameters[0]));
                    double latA = Convert.ToDouble(parameters[1]);
                    double lonA = Convert.ToDouble(parameters[2]);
                    double altA = Convert.ToDouble(parameters[3]);
                    double latB = Convert.ToDouble(parameters[7]);
                    double lonB = Convert.ToDouble(parameters[8]);
                    double altB = Convert.ToDouble(parameters[9]);
                    Structs.GPSData data = new Structs.GPSData(time, latA, lonA, altA, latB, lonB, altB);
                    Structs.GPSBoxed gpsbox = new Structs.GPSBoxed(data, Structs.Satellite.GraceA, Structs.Anchor.Uniform);

                    Structs.PointTime current = new Structs.PointTime(gpsbox.bin.boxcenter, data.time);

                    if (lasttime.point != current.point)
                    {
                        //if just entered bin
                        bins[gpsbox.bin.lonbox, Structs.CoercedBin.BinLatCenter + gpsbox.bin.latbox]++; //add 1 to bin count
                        lasttime = current;
                    }

                }

                System.GC.Collect(); //free some memory
            }

            return bins;
        }
    }
}
