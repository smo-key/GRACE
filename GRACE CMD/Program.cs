//#define SEARCHPOWER_0 //low memory & low latency
//#define SEARCHPOWER_1 //low latency
#define SEARCHPOWER_2 //WARNING!  Slow and mem-draining!


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace GRACE_CMD
{

    /// <summary>
    /// Main program
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            /* 
             * gpslist = GPS data by time entered bin (in order)
             * bins = grid of bins and how often entered
             */

            Console.WriteLine("Initializing...");

            #if SEARCHPOWER_2
            //initialize ordered GPS data
            Dictionary<Structs.PointTime, List<Structs.GPSBoxed>> gpslist =
                new Dictionary<Structs.PointTime, List<Structs.GPSBoxed>>();
            #endif
            Structs.PointTime lasttime = new Structs.PointTime();

            //Initialize grid of bins
            #if SEARCHPOWER_1 || SEARCHPOWER_2
            List<Structs.AreaBox>[,] bins = new List<Structs.AreaBox>[Structs.GPSBoxed.BinsLon, Structs.GPSBoxed.BinsLat];
            for (int i = 0; i < Structs.GPSBoxed.BinsLon; i++)
            {
                for (int j = 0; j < Structs.GPSBoxed.BinsLat; j++)
                {
                    bins[i, j] = new List<Structs.AreaBox>();
                }
            }
            #endif
            #if SEARCHPOWER_0
            //areabox, times intersected (super low memory usage)
            long[,] bins = new long[Structs.GPSBoxed.BinsLon, Structs.GPSBoxed.BinsLat];
            for (int i = 0; i < Structs.GPSBoxed.BinsLon; i++)
            {
                for (int j = 0; j < Structs.GPSBoxed.BinsLat; j++)
                {
                    bins[i, j] = 0L;
                }
            }
            #endif

            //Read all files
            string[] files = System.IO.Directory.GetFiles("../../../../gracedata/", "*.latlon", SearchOption.TopDirectoryOnly);
            int filen = 1; //current file number

            foreach (string file in files)
            {
                //*** READ TEXT FILE and find the bin of each point ***//
                Console.Clear();
                Console.WriteLine("Reading {0} [file {1} out of {2}]", Path.GetFileName(file), filen.ToString(), files.Length.ToString());
                filen++;
                StreamReader reader = new StreamReader(file);
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
                    Structs.GPSData data = new Structs.GPSData(time, latA, lonA, altA, latB, lonB, altB);
                    Structs.GPSBoxed box = new Structs.GPSBoxed(data, Structs.Satellite.GraceA);

                    Structs.PointTime current = new Structs.PointTime(box.boxcenter, data.time);

                    if (lasttime.point == current.point)
                    {
                        //if already inside the area
                        #if SEARCHPOWER_2
                        //gpslist.ElementAt(gpslist.Count - 1).Value.Add(box);
                        //gpslist.Last().Value.Add(box);
                        gpslist.First((e) =>
                        {
                            if (e.Key == lasttime)
                            {
                                return true;
                            }
                            return false;
                        }).Value.Add(box);
                        #endif
                    }
                    else
                    {
                        //if not yet inside area
                        #if SEARCHPOWER_2
                        List<Structs.GPSBoxed> lbox = new List<Structs.GPSBoxed>();
                        lbox.Add(box);
                        gpslist.Add(current, lbox); //add to boxlist
                        #endif
                        #if SEARCHPOWER_1
                        bins[box.lonbox, Structs.GPSBoxed.BinLatCenter + box.latbox].Add(box.area); //add to bin
                        #endif
                        #if SEARCHPOWER_0
                        bins[box.lonbox, Structs.GPSBoxed.BinLatCenter + box.latbox]++; //add 1 to bin count
                        #endif
                        lasttime = current;
                    }

                }

                System.GC.Collect(); //free some memory
            }

            Console.WriteLine("Reading complete!");

            return;
        }
    }
}
