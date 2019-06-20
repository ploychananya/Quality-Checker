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
//birkan.herguner @gmail.com
namespace Qualıty_Checker
{
    public partial class Analyze 
    {
        //private MFileClass myfile;
        private double rate;
        private string name;
        //MFReader myReader = new MFReader();
        public void Method(string filePath, string quality)
        {
            filePath = @"E:\VideoFiles\Cannon.mp4";
            //string extraProps = "video_track=-1";
            MFileClass myFile = new MFileClass();
            //myFile.FileNameSet(filePath, extraProps);


            myFile.PropsSet("active_frc", "0");
            myFile.PropsSet("on_frame.sync", "true");
            myFile.PropsSet("file::video_track", "-1");
            myFile.PropsSet("loop", "false");
            //yFile.OnEventSafe += m_objFile_OnEventSafe;
            //myFile.OnFrameSafe -= m_objFileScan_OnFrameSafe;
            myFile.PropsSet("rate_control", "false");
            myFile.FileNameSet(filePath, "");


            //.ObjectStart(m_objPlaylist);
            //myFile.ObjectStart(myFile);
            //Console.WriteLine("between frame: 100-110");


            //myFile.FileNameSet(filePath, extraProps);

            // pull vidio file to variable;
            if (quality == "-solution")
            {
                //string file;
                //myFile.FileNameGet(out file);
                int nIndex;
                string strFormat;
                M_VID_PROPS vidProps;
                myFile.FormatVideoGet(eMFormatType.eMFT_Output, out vidProps, out nIndex, out strFormat);
                Console.WriteLine((vidProps.nAspectX).ToString() + " * " + (vidProps.nAspectY).ToString() + "\n"+" ScaleType: " + (vidProps.eScaleType).ToString() + "  bitrate: "+ (vidProps.dblRate).ToString());
                Console.WriteLine(nIndex.ToString());
                Console.WriteLine(strFormat.ToString());
                Console.WriteLine((eMFormatType.eMFT_Output).ToString());
                //myFile.FormatVideoGet();

            }
            else if (quality == "-codec")
            {
                //find solution 
                //show solution 
                Console.WriteLine("Codec: h263, h264");
            }else if (quality == "-freez")
            {
                //find solution 
                //show solution 
                Console.WriteLine("between frame: 100-110");
            }else if (quality == "-info")
            {
                
                

            }
        }
    }
}
