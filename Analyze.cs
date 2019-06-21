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
        private double rate;
        private string name;
        string strFormat;
        M_AUD_PROPS audProps;

        public void Method(string filePath, string quality)
        {
            Lib lib = new Lib();
            //Playlist
            int nIndex = -1;
            MPlaylistClass myPlaylist = new MPlaylistClass();
            
            myPlaylist.PreviewWindowSet("", 10);
            myPlaylist.PreviewEnable("", 1, 1);

            MItem pFile;
            myPlaylist.PlaylistAdd(null, filePath, "", ref nIndex, out pFile);

            //string extraProps = "video_track=-1";
            MFileClass myFile = new MFileClass();
            myFile.FileNameSet(filePath, "");
            myFile.FilePlayStart(); //it has to play for respresent the statistic.

            if (quality == "-solution")
            {
            
            }else if (quality == "-info")//Only Object Property
            {
                //string myVal;
                //((IMProps)pFile).PropsGet("file::info::subtitle.0::codec_name", out myVal);
                //Console.WriteLine(myVal);

                /*
                Console.WriteLine("     info IMProps: ");
                int nCount;
                ((IMProps)pFile).PropsGetCount("", out nCount); 
                for (int i = 0; i < nCount; i++)
                {
                    string myName;
                    string myValue;
                    int myNode;
                    ((IMProps)pFile).PropsGetByIndex("", i, out myName, out myValue, out myNode);
                    Console.WriteLine(myName + " " + myValue + " " + myNode.ToString());
                    //Console.WriteLine(myValue);
                    //((IMProps)myItem).PropsRemove(myName);
                }


                Console.WriteLine("     info MFILE not specific: ");
                lib.GetFileInfo(myFile, "");
                */
                lib.GetFileInfo(myFile, "file::info");
                /*
                Console.WriteLine("     file MFILE: ");
                string myVal;
                (myFile as IMProps).PropsGet("object::info::codec", out myVal);
                Console.WriteLine(myVal);
                */
            }
            else if (quality == "-codec")
            {
                Console.WriteLine("Codec: h263, h264");
            }
            else if (quality == "-freez")
            {
                Console.WriteLine("between frame: 100-110");
            }
            else if (quality == "-audChanel")
            {

                int nChannels;
                myPlaylist.AudioChannelsPerTrackGet(out nChannels);
                Console.WriteLine("#AudioChanel(s) : " + nChannels);

            } else if (quality == "-audTrack")
            {
                int tCount;
                myPlaylist.AudioTracksGetCount(out tCount);
                Console.WriteLine("#AudioTrack(s) : " + tCount);

            }else if (quality == "-volume")
            {
                double myVol;
                myPlaylist.PreviewAudioVolumeGet("", -1, out myVol);
                Console.WriteLine("Volume(dB) : " + myVol);
            }
            else if (quality == "-play")
            {
                myPlaylist.FilePlayStart();
            } else if (quality == "-name")
            {
                string FileName;
                myPlaylist.FileNameGet(out FileName);
                Console.WriteLine("File's name : " + FileName.ToString());
            } else if (quality == "-format")
            {
                myPlaylist.FormatAudioGet(eMFormatType.eMFT_Output, out audProps, out nIndex, out strFormat);
                Console.WriteLine("Format: " + strFormat);
            } else if (quality == "-sRate")
            {
                pFile.FileRateGet(out rate);
                Console.WriteLine("Rate : " + audProps.nSamplesPerSec + "X");
                Console.WriteLine("Sample Rate : " + audProps.nSamplesPerSec);
            }
            }
    }
    
}
