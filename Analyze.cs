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
        string strFormat;
        M_AUD_PROPS audProps;
        //MFReader myReader = new MFReader();
        public void Method(string filePath, string quality)
        {
            //Playlist
            int nIndex = -1;
            MPlaylistClass myPlaylist = new MPlaylistClass();
            
            myPlaylist.PreviewWindowSet("", 10);
            myPlaylist.PreviewEnable("", 1, 1);
            //myPlaylist.ObjectStart(new object());
            MItem pFile;
            myPlaylist.PlaylistAdd(null, filePath, "", ref nIndex, out pFile);

            /*
            MPlaylistClass myPlaylist = new MPlaylistClass();
            myPlaylist.PreviewWindowSet("", 10);
            myPlaylist.PreviewEnable("", 1, 1);
            myPlaylist.ObjectStart(new object());
            int nIndex = -1;
            MItem pFile;
            myPlaylist.PlaylistAdd(null, filePath, "", ref nIndex, out pFile);
            myPlaylist.FilePlayStart();
            int nCount = 2;
            //((IMProps)pFile).PropsGetCount("info", out nCount);

            for (int i = 0; i < nCount; i++)
            {
                string myName;
                string myValue;
                int myNode;
                ((IMProps)pFile).PropsGetByIndex("info", i, out myName, out myValue, out myNode);

                Console.WriteLine(myName+" "+myValue+ " "+ myNode);
                ((IMProps)pFile).PropsRemove(myName);
            }
            */



            


            //string myVal;
            //((IMProps)pFile).PropsGet("object", out myVal);
            //Console.WriteLine(myVal);
            
            //Playlist


            //MFILECLASS
            //MFileClass file = new MFileClass();
            //IMAudioTrack track;
            //Create new virtual track as full clone of 0 track
            //file.AudioTrackAdd(0, "", "NewTrack", out track);
            //Create new virtual track as clone of 1,2,4 channels of 2 track
            //file.AudioTrackAdd(2, "1,2,4", "NewTrack", out track);



            //MITEMCLASS


            //myPlaylist.FilePlayStart();
            //yPlaylist.FormatAudioGet(eMFormatType.eMFT_Output, out audProps, out nIndex, out strFormat);

            //int myIndex = -1;

            //MItem myTargetItem;
            //myPlaylist.PlaylistCommandAdd("mute", "true", myItem, ref myIndex, out myTargetItem);

            //MItem myTargetItem;
            //myPlaylist.PlaylistCommandAdd("mute", "true", myItem, ref nIndex, out myTargetItem);

            //int nIndex;
            //string strFormat;
            //M_AUD_PROPS audProps;
            //MPlaylistClass myPlaylist = new MPlaylistClass();

            //audProps.nSamplesPerSec = 48000;
            //myPlaylist.FormatAudioSet(eMFormatType.eMFT_Convert, ref audProps);
            //filePath = @"/c/Users/ployrue/Desktop";
            //string extraProps = "video_track=-1";

            //MItemClass myFile = new MItemClass();
            //myFile.FileNameSet(filePath, extraProps);


            //myFile.PropsSet("active_frc", "0");
            //myFile.PropsSet("on_frame.sync", "true");
            //myFile.PropsSet("file::video_track", "-1");
            //myFile.PropsSet("loop", "false");

            //yFile.OnEventSafe += m_objFile_OnEventSafe;
            //myFile.OnFrameSafe -= m_objFileScan_OnFrameSafe;

            //myFile.PropsSet("rate_control", "false");
            //myFile.FileNameSet(filePath, "");


            //.ObjectStart(m_objPlaylist);
            //myFile.ObjectStart(myFile);
            //Console.WriteLine("between frame: 100-110");


            //myFile.FileNameSet(filePath, extraProps);

            // pull vidio file to variable;
            if (quality == "-solution")
            {
                //string file;
                //myFile.FileNameGet(out file);

                //int nIndex;
                //string strFormat;
                //M_VID_PROPS vidProps;
                //myFile.FormatVideoGet(eMFormatType.eMFT_Output, out vidProps, out nIndex, out strFormat);
                //Console.WriteLine((vidProps.nAspectX).ToString() + " * " + (vidProps.nAspectY).ToString() + "\n"+" ScaleType: " + (vidProps.eScaleType).ToString() + "  bitrate: "+ (vidProps.dblRate).ToString());
                //Console.WriteLine(nIndex.ToString());
                //Console.WriteLine(strFormat.ToString());
                //Console.WriteLine((eMFormatType.eMFT_Output).ToString());


                //work
                //////////////////////////////////////////////////////////////////////////////
                /*MPlaylistClass myPlaylist = new MPlaylistClass();
                //myPlaylist.PreviewWindowSet("", panelPreview.Handle.ToInt32());
                myPlaylist.PreviewWindowSet("", 2);
                myPlaylist.PreviewEnable("", 1, 1);
                myPlaylist.ObjectStart(new object());
                //Insert clip at the end of playlist
                int nIndex = -1;
                MItem pFile;
                string strPath = @"c:/Users/ployrue/Desktop/123.mp3";
                myPlaylist.PlaylistAdd(null, strPath, "", ref nIndex, out pFile);
                myPlaylist.FilePlayStart();
                */
                //////////////////////////////////////////////////////////////////////////////

                //MPlaylist myPlaylist = new MPlaylist();
                //myPlaylist.PreviewWindowSet("", panelPreview.Handle.ToInt32());
                //myPlaylist.PreviewEnable("", 1, 1);
                //myPlaylist.ObjectStart(new object());
                //Insert clip at the end of playlist
                //int nIndex = -1;
                //MItem pFile;
                //string strPath = @"C:/Users/ployrue/Desktop/123.mp3";
                //myPlaylist.PlaylistAdd(null, strPath, "", ref nIndex, out pFile);
                //myPlaylist.FilePlayStart();
                //myPlaylist.PlaylistBackgroundGet();
                //myFile.FormatVideoGet();

            }else if (quality == "-info")
            {

                int nCount;
                ((IMProps)pFile).PropsGetCount("", out nCount); //Only Object Property
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
            }
            else if (quality == "-codec")
            {
                //find solution 
                //show solution 
                Console.WriteLine("Codec: h263, h264");
            }
            else if (quality == "-freez")
            {
                //find solution 
                //show solution 
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
                //MPlaylistClass myPlaylist = new MPlaylistClass();
                //myPlaylist.PreviewWindowSet("", panelPreview.Handle.ToInt32());

                //myPlaylist.PreviewWindowSet("", 10);
                //myPlaylist.PreviewEnable("", 1, 1);
                //myPlaylist.ObjectStart(new object());

                //Insert clip at the end of playlist

                //w int nIndex = -1;
                //w MItem pFile;

                //string strPath = @"c:/Users/ployrue/Desktop/123.mp3";

                //w myPlaylist.PlaylistAdd(null, filePath, "", ref nIndex, out pFile);

                myPlaylist.FilePlayStart();
            } else if (quality == "-name")
            {
                string FileName;
                myPlaylist.FileNameGet(out FileName);
                Console.WriteLine("File's name : " + FileName.ToString());
            } else if (quality == "-format")
            {
                //int nIndex;
                //string strFormat;
                //M_AUD_PROPS audProps;
                //MPlaylistClass myPlaylist = new MPlaylistClass();
                //eMFormatType autype;
                myPlaylist.FormatAudioGet(eMFormatType.eMFT_Output, out audProps, out nIndex, out strFormat);

                //audProps.nSamplesPerSec = 48000;
                //myPlaylist.FormatAudioSet(eMFormatType.eMFT_Convert, ref audProps);
                Console.WriteLine("Format: " + strFormat);
                //Console.WriteLine("Format2: " + myItem.FileRateGet()
            } else if (quality == "-sRate")
            {
                //string strFormat;
                //M_AUD_PROPS audProps;
                //MPlaylistClass myPlaylist = new MPlaylistClass();

                //myPlaylist.FormatAudioGet(eMFormatType.eMFT_Output, out audProps, out nIndex, out strFormat);

                //audProps.nSamplesPerSec = 48000;
                //myPlaylist.FormatAudioSet(eMFormatType.eMFT_Convert, ref audProps);
                
                pFile.FileRateGet(out rate);
                Console.WriteLine("Rate : " + audProps.nSamplesPerSec + "X");
                Console.WriteLine("Sample Rate : " + audProps.nSamplesPerSec);
            }
            }
    }
}
