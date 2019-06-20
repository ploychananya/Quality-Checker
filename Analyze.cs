using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using MPLATFORMLib;

namespace Qualıty_Checker
{
    public partial class Analyze : Form
    {
        private MFile myfile;


        public void Method(string filePath, string quality)
        {
            myfile = new MFile();
            string extraProps = "vidio_track=-1";
            myfile.FileNameSet(filePath, extraProps);
            
            //myFile.FileNameSet(filePath, extraProps);

            // pull vidio file to variable;
            if (quality == "-solution")
            {
                myfile.FilePlayStart();// to start playback
                //find solution 
                //show solution 
                Console.WriteLine("Solution : 720p");
            }else if (quality == "-codec")
            {
                //find solution 
                //show solution 
                Console.WriteLine("Codec: h263, h264");
            }else if (quality == "-freez")
            {
                //find solution 
                //show solution 
                Console.WriteLine("between frame: 100-110");
            }
        }
    }
}
