using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRACEdata;

namespace GRACE_JSON
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("GRACE Data Automated JSON Converter");
            Console.Write("Continuing will DELETE all currently created data! [y/n] ");
            if (Console.ReadKey().ToString() != "y") { return; }

            Structs.PointTime lasttime = new Structs.PointTime();
            Structs.Anchor anchor = Structs.Anchor.Uniform;
            if (!(360 % Globals.gridsize == 0)) { anchor = Structs.Anchor.Center; }



        }
    }
}
