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
        private MFileClass myfile;
        private double rate;
        private string name;
        //MFReader myReader = new MFReader();
        public void Method(string filePath, string quality)
        {
            myfile = new MFileClass();

            //filePath = "@" + filePath;
            string extraProps = "";
            myfile.FileNameSet(filePath, extraProps);
            
            
            //myFile.FileNameSet(filePath, extraProps);

            // pull vidio file to variable;
            if (quality == "-solution")
            {
     
                //Console.WriteLine("Solution : 720p");

                MPlaylistClass myPlaylist = new MPlaylistClass();
                
                //myPlaylist.ObjectStart(new object());
                //Insert clip at the end of playlist
                //int nIndex = -1;
                //MItem pFile;
                string strPath = @"c:\Users\cruenpitak\Desktop\1.mp3";
                //myPlaylist.PlaylistAdd(null, strPath, "", ref nIndex, out pFile);
                //Console.WriteLine(pFile);


                int nIndex;
                string strFormat;
                M_VID_PROPS vidProps;
                //MPlaylistClass myPlaylist = new MPlaylistClass();
                myPlaylist.FormatVideoGet(eMFormatType.eMFT_Output, out vidProps, out nIndex, out strFormat);
                vidProps.eScaleType = eMScaleType.eMST_LetterBox;
                myPlaylist.FormatVideoSet(eMFormatType.eMFT_Convert, ref vidProps);
                Console.WriteLine(vidProps.eVideoFormat);
                Console.WriteLine(strFormat);
                //myPlaylist.
                //myPlaylist.FilePlayStart();
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
