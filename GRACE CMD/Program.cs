#define SEARCHPOWER_0 //Entry Count Only
//#define SEARCHPOWER_1 //List of entries by time
//#define SEARCHPOWER_2 //Entry Count with Dated Entries

#define WRITELOG //write to log file

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Collections.Concurrent;

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
            Structs.PointTime lasttime = new Structs.PointTime();
            Structs.Anchor anchor = Structs.Anchor.Uniform;
            if (!(360 % Globals.gridsize == 0)) { anchor = Structs.Anchor.Center; }

            #if WRITELOG
                //Prepare logging
                File.Delete("output.txt");
                StreamWriter log = new StreamWriter("output.txt");
                log.AutoFlush = true;

                #if SEARCHPOWER_1
                log.WriteLine("GRACE-CMD1 SEARCHPOWER_1");
                log.WriteLine("Boxsize: {0} degrees", Globals.gridsize);
                log.WriteLine("EntryTimeUTC | BoxIDLon BoxIDLat | BoxTopLeftLon BoxTopLeftLat BoxBottomRightLon BoxBottomRightLat");
                log.WriteLine(" ");
                #endif
            #endif

            #if SEARCHPOWER_2
            //initialize grid of bins, with list of all entries in/out
            List<DateTime>[,] binentry = new List<DateTime>[Structs.CoercedBin.BinsLon, Structs.CoercedBin.BinsLat];
            for (int i = 0; i < Structs.CoercedBin.BinsLon; i++)
            {
                for (int j = 0; j < Structs.CoercedBin.BinsLat; j++)
                {
                    binentry[i, j] = new List<DateTime>();
                }
            }
            #endif
            #if SEARCHPOWER_1
            //Initialize grid of bins
            List<Structs.CoercedBin>[,] bins = new List<Structs.CoercedBin>[Structs.CoercedBin.BinsLon, Structs.CoercedBin.BinsLat];
            for (int i = 0; i < Structs.CoercedBin.BinsLon; i++)
            {
                for (int j = 0; j < Structs.CoercedBin.BinsLat; j++)
                {
                    bins[i, j] = new List<Structs.CoercedBin>();
                }
            }
            #endif
            #if SEARCHPOWER_0
            //initialize grid of bins, times intersected only (super low memory usage)
            int[,] bins = new int[Structs.CoercedBin.BinsLon, Structs.CoercedBin.BinsLat];
            for (int i = 0; i < Structs.CoercedBin.BinsLon; i++)
            {
                for (int j = 0; j < Structs.CoercedBin.BinsLat; j++)
                {
                    bins[i, j] = 0;
                }
            }
            #endif

            //Read all files
            string[] files = new string[1] { "../../../../../gracedata/2002-04-05.1579023002.latlon" };
            //string[] files = Directory.GetFiles("../../../../../gracedata/", "*.latlon", SearchOption.TopDirectoryOnly);
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
                    Structs.GPSBoxed gpsbox = new Structs.GPSBoxed(data, Structs.Satellite.GraceA, Structs.Anchor.Uniform);

                    Structs.PointTime current = new Structs.PointTime(gpsbox.bin.boxcenter, data.time);

                    if (lasttime.point != current.point)
                    {
                        //if not yet inside area (just entering)
                        #if SEARCHPOWER_2
                            #if WRITELOG
                            //add new entry
                            //lastbox = new Structs.Point(gpsbox.bin.lonbox, Structs.CoercedBin.BinLatCenter + gpsbox.bin.latbox);
                            binentry[gpsbox.bin.lonbox, Structs.CoercedBin.BinLatCenter + gpsbox.bin.latbox].Add(
                                current.time);
                            #else
                            binentry[gpsbox.bin.lonbox, Structs.CoercedBin.BinLatCenter + gpsbox.bin.latbox].Add(
                                current.time); //add to bin
                            #endif
                        #endif

                        #if SEARCHPOWER_1
                            #if WRITELOG
                                log.WriteLine("{0} | {1} {2} | {3} {4} {5} {6}", gpsbox.bin.entry.ToString(),
                                    gpsbox.bin.lonbox.ToString(), gpsbox.bin.latbox.ToString(),
                                    gpsbox.bin.box.topleft.x.ToString("F3"), gpsbox.bin.box.topleft.y.ToString("F3"),
                                    gpsbox.bin.box.bottomright.x.ToString("F3"), gpsbox.bin.box.bottomright.y.ToString("F3"));
                            #else
                            bins[gpsbox.bin.lonbox, Structs.CoercedBin.BinLatCenter + gpsbox.bin.latbox].Add(gpsbox.bin); //add to bin
                            #endif
                        #endif

                        #if SEARCHPOWER_0
                            bins[gpsbox.bin.lonbox, Structs.CoercedBin.BinLatCenter + gpsbox.bin.latbox]++; //add 1 to bin count
                        #endif
                        lasttime = current;
                    }

                }

                System.GC.Collect(); //free some memory
            }

            #if WRITELOG

                #if SEARCHPOWER_2
                Console.WriteLine("Writing log...");
                log.WriteLine("GRACE-CMD1 SEARCHPOWER_2");
                log.WriteLine("Boxsize: {0} degrees", Globals.gridsize);
                log.WriteLine("EntryCount | BoxIDLon BoxIDLat | BoxTopLeftLon BoxTopLeftLat BoxBottomRightLon BoxBottomRightLat");
                log.WriteLine("--- EntryTimeUTC");
                log.WriteLine(" ");
                for (int i = 0; i < Structs.CoercedBin.BinsLon; i++)
                {
                    for (int j = 0; j < Structs.CoercedBin.BinsLat; j++)
                    {
                        int k = j - Structs.CoercedBin.BinLatCenter;
                        Structs.Point c = Structs.CoercedBin.GetCenter(i, k);
                        Structs.Point size = Structs.CoercedBin.GetSize(c.x, c.y);
                        Structs.AreaBox box = new Structs.AreaBox(Utils.coerce(c.x - (size.x / 2), 0, 360),
                            Utils.coerce(c.x + (size.x / 2), 0, 360),
                            Utils.coerce(c.y - (size.y / 2), -90, 90),
                            Utils.coerce(c.y + (size.y / 2), -90, 90));
                        log.WriteLine("{0} | {1} {2} | {3} {4} {5} {6}", binentry[i, j].Count, i.ToString(), k.ToString(),
                            box.topleft.x.ToString("F3"), box.topleft.y.ToString("F3"), box.bottomright.x.ToString("F3"), box.bottomright.y.ToString("F3"));
                        foreach (DateTime item in binentry[i, j])
                        {
                            log.WriteLine("--- {0}", item.ToString());
                        }
                    }
                }
                #endif
                #if SEARCHPOWER_0
                Console.WriteLine("Writing log...");
                log.WriteLine("GRACE-CMD1 SEARCHPOWER_0");
                log.WriteLine("Boxsize: {0} degrees", Globals.gridsize);
                log.WriteLine("TimesEntered | BoxIDLon BoxIDLat | BoxTopLeftLon BoxTopLeftLat BoxBottomRightLon BoxBottomRightLat");
                log.WriteLine(" ");
                for (int i = 0; i < Structs.CoercedBin.BinsLon; i++)
                {
                    for (int j = 0; j < Structs.CoercedBin.BinsLat; j++)
                    {
                        int k = j - Structs.CoercedBin.BinLatCenter;
                        Structs.Point c = Structs.CoercedBin.GetCenter(i, k);
                        Structs.Point size = Structs.CoercedBin.GetSize(c.x, c.y);
                        Structs.AreaBox box = new Structs.AreaBox();
                        switch (anchor)
                        {
                            case Structs.Anchor.Center:
                                box = new Structs.AreaBox(Utils.coerce(c.x - (size.x / 2), 0, 360),
                                    Utils.coerce(c.x + (size.x / 2), 0, 360),
                                    Utils.coerce(c.y - (size.y / 2), -90, 90),
                                    Utils.coerce(c.y + (size.y / 2), -90, 90));
                                break;
                            case Structs.Anchor.Uniform:
                                box = new Structs.AreaBox(Utils.coerce(c.x, 0, 360),
                                    Utils.coerce(c.x + size.x, 0, 360),
                                    Utils.coerce(c.y, -90, 90),
                                    Utils.coerce(c.y + size.y, -90, 90));
                                break;
                            default:
                                break;
                        }
                        log.WriteLine("{0} | {1} {2} | {3} {4} {5} {6}", bins[i, j].ToString(), i.ToString(), k.ToString(),
                            box.topleft.x.ToString("F3"), box.topleft.y.ToString("F3"), box.bottomright.x.ToString("F3"), box.bottomright.y.ToString("F3"));
                    }
                }
                #endif

                log.Close();

            #endif

            Console.WriteLine("Reading complete!");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
            
            return;
        }
    }
}
