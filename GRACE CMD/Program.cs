﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace GRACE_CMD
{
    class Program
    {
        static void Main(string[] args)
        {
            //*** READ TEXT FILES and find the bin of each point ***//

            /* 
             * gpslist = GPS data by time entered bin (in order)
             * bins = grid of bins and how often found
             */

            Dictionary<Structs.PointTime, List<Structs.GPSBoxed>> gpslist =
                new Dictionary<Structs.PointTime, List<Structs.GPSBoxed>>();
            Structs.PointTime lasttime = new Structs.PointTime();

            //Initialize grid of bins
            List<Structs.AreaBox>[,] bins = new List<Structs.AreaBox>[Structs.GPSBoxed.BinsLon, Structs.GPSBoxed.BinsLat];
            for (int i = 0; i < Structs.GPSBoxed.BinsLon; i++)
            {
                for (int j = 0; j < Structs.GPSBoxed.BinsLat; j++)
                {
                    bins[i, j] = new List<Structs.AreaBox>();
                }
            }

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
                Structs.GPSData data = new Structs.GPSData(time, latA, lonA, altA, latB, lonB, altB);
                Structs.GPSBoxed box = new Structs.GPSBoxed(data, Structs.Satellite.GraceA);

                Structs.PointTime current = new Structs.PointTime(box.boxcenter, data.time);
                
                if (lasttime.point == current.point)
                {
                    //if already inside the area
                    gpslist.First((e) =>
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
                    List<Structs.GPSBoxed> lbox = new List<Structs.GPSBoxed>();
                    lbox.Add(box);
                    gpslist.Add(current, lbox); //add to boxlist
                    bins[box.lonbox, Structs.GPSBoxed.BinLatCenter + box.latbox].Add(box.area); //add to bin
                    lasttime = current;
                }
                
            }

            return;
        }
    }
}
