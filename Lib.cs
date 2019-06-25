using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPLATFORMLib;
using System.Linq;
using System.Runtime.InteropServices;

using System.Reflection;

namespace Qualıty_Checker
{
    public class Lib
    {
     
        public Info info = new Info();
        public Metadata metadataObj = new Metadata();

        unsafe byte GetPixelValue(uint* startOfFrame, int width, int x, int y) // 100 pıxel 
        {
            uint* pixel = startOfFrame + (y * width) + x;
            byte* bw = (byte*)pixel;
            return *bw;
        }
        unsafe void SetPixelValue(uint* startOfFrame, int width, int x, int y, byte Value) // 100 pıxel 
        {

            uint* pixel = startOfFrame + (y * width) + x;
            byte* blue = (byte*)pixel;
            byte* green = ((byte*)pixel) + 1;
            byte* red = ((byte*)pixel) + 2;
            *blue = Value;
            *green = Value;
            *red = Value;
        }
        public float[] kernel =
{
            -1.0f, -1.0f, -1.0f,-1.0f, -1.0f, -1.0f,
            -1.0f,-1.0f, -1.0f,9.0f, -1.0f, -1.0f,
            -1.0f, -1.0f, -1.0f,-1.0f, -1.0f, -1.0f,

        };

        public float GetKernelValue(int x, int y)
        {
            return kernel[y * 3 + x];
        }

        public void M_objReader_OnFrameSafe(string bsChannelID, object pMFrame)//**********************************
        {

            MFrame clonedFrame;

            M_AV_PROPS avProps;
            (pMFrame as IMFrame).FrameAVPropsGet(out avProps);
            int frameWidth = avProps.vidProps.nWidth;
            int frameHeight = Math.Abs(avProps.vidProps.nHeight);
            int pcbSize;
            long framePointer;
            long framePointerCloned;
            byte valuePixel;
            (pMFrame as IMFrame).FrameClone(out clonedFrame, eMFrameClone.eMFC_Full_ForceCPU, eMFCC.eMFCC_ARGB32);

            (pMFrame as IMFrame).FrameVideoGetBytes(out pcbSize, out framePointer);

            unsafe
            {
                //convert original frame to bw
                uint* videoData = (uint*)framePointer;
                byte* alpha;
                byte* red;
                byte* green;
                byte* blue;
                byte grayScale;

                for (int y = 0; y < frameHeight; y++)
                {
                    for (int x = 0; x < frameWidth; x++)
                    {


                        blue = (byte*)videoData;
                        green = ((byte*)videoData) + 1;
                        red = ((byte*)videoData) + 2;
                        alpha = ((byte*)videoData) + 3;
                        grayScale = (byte)((*blue * 0.1) + (*green * 0.6f) + (*red * 0.3f));
                        SetPixelValue((uint*)framePointer, frameWidth, x, y, grayScale);
                        videoData++;

                    }
                }



                //(pMFrame as IMFrame).FrameClone(out clonedFrame, eMFrameClone.eMFC_Full_ForceCPU, eMFCC.eMFCC_ARGB32);

                byte threshold = 20;

                clonedFrame.FrameVideoGetBytes(out pcbSize, out framePointerCloned);
                videoData = (uint*)framePointerCloned;

                /*SIMPLE!! EDGE DETECTION 
                for (int y = 1; y < frameHeight; y++)
                {
                    for (int x = 1; x < frameWidth; x++)
                    {

                        //byte currentValue = GetPixelValue((uint*)framePointer, frameWidth, x, y);
                        byte diffHoriZontal= (byte)Math.Abs(GetPixelValue((uint*)framePointer, frameWidth, x - 1, y) - GetPixelValue((uint*)framePointer, frameWidth, x, y));
                        byte diffVirtical = (byte)Math.Abs(GetPixelValue((uint*)framePointer, frameWidth, x, y-1 ) - GetPixelValue((uint*)framePointer, frameWidth, x, y));
                        if(diffHoriZontal > threshold || diffVirtical > threshold)
                            SetPixelValue((uint*)framePointerCloned, frameWidth, x, y, 255);
                        else
                            SetPixelValue((uint*)framePointerCloned, frameWidth, x, y, 0);
                        //byte pixelValueOfOriginal = GetPixelValue((uint*)framePointer, frameWidth, x, y);
                        //byte pixelValueOfCloned = GetPixelValue((uint*)framePointerCloned, frameWidth, x, y);


                    }
                }
                */


                //SHARPEN&BLUR
                for (int y = 1; y < frameHeight - 1; y++)
                {
                    for (int x = 1; x < frameWidth - 1; x++)
                    {
                        float sumKernel = 0;


                        for (int yy = -1; yy < 2; yy++)
                            for (int xx = -1; xx < 2; xx++)
                            {


                                sumKernel += GetPixelValue((uint*)framePointer, frameWidth, x + xx, y + yy) * GetKernelValue(xx + 1, yy + 1);


                            }

                        //sumKernel = GetPixelValue((uint*)framePointer, frameWidth, x , y);

                        if (sumKernel > 255)
                            sumKernel = 255;
                        if (sumKernel < 0)
                            sumKernel = 0;

                        //byte diffHoriZontal = (byte)Math.Abs(GetPixelValue((uint*)framePointer, frameWidth, x - 1, y) - GetPixelValue((uint*)framePointer, frameWidth, x, y));
                        //byte diffVirtical = (byte)Math.Abs(GetPixelValue((uint*)framePointer, frameWidth, x, y - 1) - GetPixelValue((uint*)framePointer, frameWidth, x, y));
                        //if (diffHoriZontal > threshold || diffVirtical > threshold)
                        SetPixelValue((uint*)framePointerCloned, frameWidth, x, y, (byte)sumKernel);
                        //else
                        //SetPixelValue((uint*)framePointerCloned, frameWidth, x, y, 0);
                        //byte pixelValueOfOriginal = GetPixelValue((uint*)framePointer, frameWidth, x, y);
                        //byte pixelValueOfCloned = GetPixelValue((uint*)framePointerCloned, frameWidth, x, y);


                    }
                }

            }
            //preview.ReceiverPutFrame(bsChannelID, clonedFrame);

            Marshal.ReleaseComObject(pMFrame);
            Marshal.ReleaseComObject(clonedFrame);
            GC.Collect();
        }

        public Info GetFileInfo(MFileClass m_objMFReader, string propertyNode)
        {
            
            info.metadata = metadataObj;
            
            
            
            //get how many vidio and audio
            //info.video[].metadata = metadataObj;            88888888888888888888888


            //string json = "\"info\":{";
            
            int nCount = 0;
            try
            {
                // get a number of properties
                m_objMFReader.PropsGetCount(propertyNode, out nCount);
            }
            catch (Exception) { }
            for (int i = 0; i < nCount; i++)
            {
                int startIndex = 12;
                int length = 2;
                string sName;
                string sValue;
                int bNode = 0;
                m_objMFReader.PropsGetByIndex(propertyNode, i, out sName, out sValue, out bNode);
                // bNode flag indicates whether there are internal properties
                // to collect a full node name we should separated it with "::", e.g. "info::video.0"
                string sRelName = propertyNode.Length > 0 ? propertyNode + "::" + sName : sName;

              
                if (bNode != 0)
                {
                    GetFileInfo(m_objMFReader,sRelName); // call the method recursively in case of sub-nodes
                }
                else
                {
                    string sub_name = sRelName.Substring(startIndex, sRelName.Length - 12);



                    //insert to json 
                    if (sub_name == "ts_programs")
                    {
                        info.ts_programs = Int32.Parse(sValue);
                    }
                    else if (sub_name == "subtitle_track")
                    {
                        info.subtitle_track = Int32.Parse(sValue);
                    }
                    else if (sub_name == "path")
                    {
                        info.path = sValue;
                    }
                    else if (sub_name == "audio_tracks") // create array of audio
                    {

                        info.audio_tracks = Int32.Parse(sValue);
                        Audio audioObj = new Audio();
                        for (int j = 0; j < Int32.Parse(sValue); j++)
                        {
                            info.audio[j] = audioObj;
                            info.audio[j].metadata = metadataObj;
                        }

                    }
                    else if (sub_name == "audio_track.used")
                    {
                        info.audio_track_used = Int32.Parse(sValue);
                    }
                    else if (sub_name == "video_tracks") // creat array of vidio
                    {
                        info.video_tracks = Int32.Parse(sValue);
                        Vidio vidioObj = new Vidio();
                        for (int j = 0; j < Int32.Parse(sValue); j++)
                        {
                            info.video[j] = vidioObj;
                            info.video[j].metadata = metadataObj;
                        }
                    }
                    else if (sub_name == "video_track.used")
                    {
                        info.video_track_used = Int32.Parse(sValue);
                    }
                    else if (sub_name == "subtitle_tracks")
                    {
                        info.subtitle_tracks = Int32.Parse(sValue);
                    }
                    else if (sub_name == "subtitle_track.used")
                    {
                        if (sValue != "")
                            info.subtitle_track_used = Int32.Parse(sValue);
                    }
                    else if (sub_name == "context_type")
                    {
                        info.context_type = sValue;
                    }
                    else if (sub_name == "streams")
                    {
                        info.streams = Int32.Parse(sValue);
                    }
                    else if (sub_name == "format")
                    {
                        info.format = sValue;
                    }
                    else if (sub_name == "format_name")
                    {
                        info.format_name = sValue;
                    }
                    else if (sub_name == "start_time")
                    {
                        info.start_time = Double.Parse(sValue);
                    }
                    else if (sub_name == "duration")
                    {
                        info.duration = Double.Parse(sValue);
                    }
                    else if (sub_name == "size")
                    {
                        info.size = Int32.Parse(sValue);
                    }
                    else if (sub_name == "bitrate")
                    {
                        info.bitrate = Int32.Parse(sValue);
                    }
                    else if (sub_name == "bitrate_kbps")
                    {
                        info.bitrate_kbps = Double.Parse(sValue);
                    }
                    else if (sub_name == "metadata.major_brand")
                    {
                        info.metadata.major_brand = sValue;
                    }
                    else if (sub_name == "metadata.minor_version")
                    {
                        info.metadata.minor_version = sValue;
                    }
                    else if (sub_name == "metadata.compatible_brands")
                    {
                        info.metadata.compatible_brands = sValue;
                    }
                    else if (sub_name == "metadata.encoder")
                    {
                        info.metadata.encoder = sValue;
                    }
                    else if (sub_name == "kbps_avg_video")
                    {
                        info.kbps_avg_video = Double.Parse(sValue);
                    }
                    else if (sub_name == "kbps_avg")
                    {
                        info.kbps_avg = Double.Parse(sValue);
                    }
                    else if (sub_name == "kbps_avg_audio")
                    {
                        info.kbps_avg_audio = Double.Parse(sValue);
                    }
                    else if (sub_name.Substring(0, 5) == "video")
                    {
                        //Console.WriteLine(sub_name.Substring(6, 1));
                        int index_vidio = Int32.Parse(sub_name.Substring(6, 1));
                        //int index_vidio = 0;
                        if (sub_name.Substring(9, sub_name.Length - 9) == "idx")
                        {
                            info.video[index_vidio].idx = Int32.Parse(sValue);
                        }
                        else if (sub_name.Substring(9, sub_name.Length - 9) == "pid")
                        {
                            info.video[index_vidio].pid = Int32.Parse(sValue);
                        }
                        else if (sub_name.Substring(9, sub_name.Length - 9) == "codec")
                        {
                            info.video[index_vidio].codec = sValue;
                        }
                        else if (sub_name.Substring(9, sub_name.Length - 9) == "codec_name")
                        {
                            info.video[index_vidio].codec_name = sValue;
                        }
                        else if (sub_name.Substring(9, sub_name.Length - 9) == "codec_tag")
                        {
                            info.video[index_vidio].codec_tag = sValue;
                        }
                        else if (sub_name.Substring(9, sub_name.Length - 9) == "profile")
                        {
                            info.video[index_vidio].profile = sValue;
                        }
                        else if (sub_name.Substring(9, sub_name.Length - 9) == "width")
                        {
                            info.video[index_vidio].width = Int32.Parse(sValue);
                        }
                        else if (sub_name.Substring(9, sub_name.Length - 9) == "height")
                        {
                            info.video[index_vidio].height = Int32.Parse(sValue);
                        }
                        else if (sub_name.Substring(9, sub_name.Length - 9) == "has_b_frames")
                        {
                            info.video[index_vidio].has_b_frames = Int32.Parse(sValue);
                        }
                        else if (sub_name.Substring(9, sub_name.Length - 9) == "pixel_ar")
                        {
                            info.video[index_vidio].pixel_ar = sValue;
                        }
                        else if (sub_name.Substring(9, sub_name.Length - 9) == "display_ar")
                        {
                            info.video[index_vidio].display_ar = sValue;
                        }
                        else if (sub_name.Substring(9, sub_name.Length - 9) == "key_distance")
                        {
                            info.video[index_vidio].key_distance = sValue;
                        }
                        else if (sub_name.Substring(9, sub_name.Length - 9) == "codec_frame_rate")
                        {
                            info.video[index_vidio].codec_frame_rate = Double.Parse(sValue);
                        }
                        else if (sub_name.Substring(9, sub_name.Length - 9) == "r_frame_rate")
                        {
                            info.video[index_vidio].r_frame_rate = sValue;
                        }
                        else if (sub_name.Substring(9, sub_name.Length - 9) == "avg_frame_rate")
                        {
                            info.video[index_vidio].avg_frame_rate = sValue;
                        }
                        else if (sub_name.Substring(9, sub_name.Length - 9) == "time_base")
                        {
                            info.video[index_vidio].time_base = sValue;
                        }
                        else if (sub_name.Substring(9, sub_name.Length - 9) == "has_b_frames")
                        {
                            info.video[index_vidio].has_b_frames = Int32.Parse(sValue);
                        }
                        else if (sub_name.Substring(9, sub_name.Length - 9) == "duration_ts")
                        {
                            info.video[index_vidio].duration_ts = Int32.Parse(sValue);
                        }
                        else if (sub_name.Substring(9, sub_name.Length - 9) == "duration")
                        {
                            info.video[index_vidio].duration = Double.Parse(sValue);
                        }
                        else if (sub_name.Substring(9, sub_name.Length - 9) == "bit_rate")
                        {
                            info.video[index_vidio].bit_rate = int.Parse(sValue);
                        }
                        else if (sub_name.Substring(9, sub_name.Length - 9) == "nb_frames")
                        {
                            info.video[index_vidio].nb_frames = int.Parse(sValue);
                        }
                        else if (sub_name.Substring(9, sub_name.Length - 9) == "metadata.language")
                        {
                            info.video[index_vidio].metadata.language = sValue;
                        }
                        else if (sub_name.Substring(9, sub_name.Length - 9) == "metadata.handler_name")
                        {
                            info.video[index_vidio].metadata.handler_name = sValue;
                        }

                    }
                    else if (sub_name.Substring(0, 5) == "audio")
                    {
                        //Console.WriteLine(sub_name.Substring(9, sub_name.Length - 9));
                        int index_audio = Int32.Parse(sub_name.Substring(6, 1));
                        if (sub_name.Substring(9, sub_name.Length - 9) == "idx")
                        {
                            info.audio[index_audio].idx = Int32.Parse(sValue);
                        }
                        else if (sub_name.Substring(9, sub_name.Length - 9) == "pid")
                        {
                            info.audio[index_audio].pid = Int32.Parse(sValue);
                        }
                        else if (sub_name.Substring(9, sub_name.Length - 9) == "codec")
                        {
                            info.audio[index_audio].codec = sValue;
                        }
                        else if (sub_name.Substring(9, sub_name.Length - 9) == "codec_name")
                        {
                            info.audio[index_audio].codec_name = sValue;
                        }
                        else if (sub_name.Substring(9, sub_name.Length - 9) == "codec_tag")
                        {
                            info.audio[index_audio].codec_tag = sValue;
                        }
                        else if (sub_name.Substring(9, sub_name.Length - 9) == "profile")
                        {
                            info.audio[index_audio].profile = sValue;
                        }
                        else if (sub_name.Substring(9, sub_name.Length - 9) == "width")
                        {
                            info.audio[index_audio].width = Int32.Parse(sValue);
                        }
                        else if (sub_name.Substring(9, sub_name.Length - 9) == "height")
                        {
                            info.audio[index_audio].height = Int32.Parse(sValue);
                        }
                        else if (sub_name.Substring(9, sub_name.Length - 9) == "has_b_frames")
                        {
                            info.audio[index_audio].has_b_frames = Int32.Parse(sValue);
                        }
                        else if (sub_name.Substring(9, sub_name.Length - 9) == "pixel_ar")
                        {
                            info.audio[index_audio].pixel_ar = sValue;
                        }
                        else if (sub_name.Substring(9, sub_name.Length - 9) == "display_ar")
                        {
                            info.audio[index_audio].display_ar = sValue;
                        }
                        else if (sub_name.Substring(9, sub_name.Length - 9) == "key_distance")
                        {
                            info.audio[index_audio].key_distance = sValue;
                        }
                        else if (sub_name.Substring(9, sub_name.Length - 9) == "codec_frame_rate")
                        {
                            info.audio[index_audio].codec_frame_rate = Int32.Parse(sValue);
                        }
                        else if (sub_name.Substring(9, sub_name.Length - 9) == "r_frame_rate")
                        {
                            info.audio[index_audio].r_frame_rate = sValue;
                        }
                        else if (sub_name.Substring(9, sub_name.Length - 9) == "avg_frame_rate")
                        {
                            info.audio[index_audio].avg_frame_rate = sValue;
                        }
                        else if (sub_name.Substring(9, sub_name.Length - 9) == "time_base")
                        {
                            info.audio[index_audio].time_base = sValue;
                        }
                        else if (sub_name.Substring(9, sub_name.Length - 9) == "has_b_frames")
                        {
                            info.audio[index_audio].has_b_frames = Int32.Parse(sValue);
                        }
                        else if (sub_name.Substring(9, sub_name.Length - 9) == "duration_ts")
                        {
                            info.audio[index_audio].duration_ts = Int32.Parse(sValue);
                        }
                        else if (sub_name.Substring(9, sub_name.Length - 9) == "duration")
                        {
                            info.audio[index_audio].duration = Double.Parse(sValue);
                        }
                        else if (sub_name.Substring(9, sub_name.Length - 9) == "bit_rate")
                        {
                            info.audio[index_audio].bit_rate = Int32.Parse(sValue);
                        }
                        else if (sub_name.Substring(9, sub_name.Length - 9) == "nb_frames")
                        {
                            info.audio[index_audio].nb_frames = Int32.Parse(sValue);
                        }
                        else if (sub_name.Substring(9, sub_name.Length - 9) == "metadata.language")
                        {
                            info.audio[index_audio].metadata.language = sValue;
                        }
                        else if (sub_name.Substring(9, sub_name.Length - 9) == "metadata.handler_name")
                        {
                            info.audio[index_audio].metadata.handler_name = sValue;
                        }
                        else if (sub_name.Substring(9, sub_name.Length - 9) == "format")
                        {
                            info.audio[index_audio].format = sValue;
                        }
                        else if (sub_name.Substring(9, sub_name.Length - 9) == "sample_rate")
                        {
                            info.audio[index_audio].sample_rate = Int32.Parse(sValue);
                        }
                        else if (sub_name.Substring(9, sub_name.Length - 9) == "channels")
                        {
                            info.audio[index_audio].channels = Int32.Parse(sValue);
                        }
                        else if (sub_name.Substring(9, sub_name.Length - 9) == "bits")
                        {
                            info.audio[index_audio].bits = Int32.Parse(sValue);
                        }
                    }
                
                }

            }
            return info;
            //return json;
        }
       

    }
}
