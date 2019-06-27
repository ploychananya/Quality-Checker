using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using MPLATFORMLib;
//birkan.herguner@gmail.com
using System.Windows.Forms;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Linq;
using System.Runtime.InteropServices;








using System.Data;
using System.Diagnostics;
using System.Threading;

namespace Qualıty_Checker
{
    partial class  Medialook : Form
    {
        Analyze analyze = new Analyze();
        Lib lib = new Lib();
        public Info info = new Info();
        bool _freez = true;
        public MFrame clonedFrame;
        public MFrame previousFrame;
        long previousFrameFramePointer;
        public long framePointerCloned1;
        public long framePointerCloned2;
        public List<int> startFreez = new List<int>();
        public List<int> freezTotal = new List<int>();
        public int _freezFramesCount = 0;
        public int frameCount = 0; // so start at frame 1
        public byte threshold = 60;
        public FreezFrame ObjFreezFrame;
        public int index_freezFrameInfo=-1;
        public string _reportName;
        public int indexFreezInfo = 0;
        public MFileClass m_objReader = new MFileClass();
        private MRendererClass m_objRenderer = new MRendererClass();
        private MPreviewClass preview = new MPreviewClass();
        public Medialook(string filePath, Info infoFromAnalyze, string reportName)
        {
            _reportName = reportName;
            info = infoFromAnalyze;
            TryToOpenFile(filePath);
            info.freez_Ththreshold = threshold;
        }

        public void TryToOpenFile(string pathToFile)
        {
            try
            {
                
                m_objReader.FileNameSet(pathToFile, "loop=true");
                m_objReader.PropsSet("object::on_frame.sync", "true");
                m_objReader.PropsSet("object::on_frame.data", "true");
                m_objReader.OnFrameSafe += M_objReader_OnFrameSafe;
                m_objReader.FilePlayStart();
            }
            catch (Exception ex)
            {
                MessageBox.Show(  pathToFile + Environment.NewLine + ex.Message);
                return;
            }  
        }
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
        /*
        private float[] kernel =
        {
            0.0625f, 0.03f, 0.062f, 0.0625f, 0.03f, 0.062f,
            0.125f, 0.25f, 0.125f, 0.125f, 0.25f, 0.125f,
           0.0625f, 0.03f, 0.062f, 0.0625f, 0.03f, 0.062f,

        };*/

        private float[] kernel =
{
            -1.0f, -1.0f, -1.0f,-1.0f, -1.0f, -1.0f,
            -1.0f,-1.0f, -1.0f,9.0f, -1.0f, -1.0f,
            -1.0f, -1.0f, -1.0f,-1.0f, -1.0f, -1.0f,

        };

        private float GetKernelValue(int x, int y)
        {
            return kernel[y * 3 + x];
        }
        public void GrayScaleFrame(long FramePointer, int frameWidth, int frameHeight)
        {
            unsafe
            {
                uint* videoData = (uint*)FramePointer;
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
                        grayScale = (byte)((*blue * 0.1f) + (*green * 0.6f) + (*red * 0.3f));
                        SetPixelValue((uint*)FramePointer, frameWidth, x, y, grayScale); //set it to greyscale
                        videoData++;

                    }
                }
            }
        }
        public void M_objReader_OnFrameSafe(string bsChannelID, object pMFrame)//**********************************
        {
            frameCount++;
            int nb_frames = (int)(info.duration * info.video[0].codec_frame_rate);
            if (frameCount<=nb_frames) {

                M_AV_PROPS avProps;
                (pMFrame as IMFrame).FrameAVPropsGet(out avProps);
                int frameWidth = avProps.vidProps.nWidth;
                int frameHeight = Math.Abs(avProps.vidProps.nHeight);
                int pcbSize;
                long currentFrameFramePointer;
                MFrame currentFrame; 
                (pMFrame as IMFrame).FrameClone(out currentFrame, eMFrameClone.eMFC_Full_ForceCPU, eMFCC.eMFCC_ARGB32);
                currentFrame.FrameVideoGetBytes(out pcbSize, out currentFrameFramePointer);

                unsafe
                {

                    GrayScaleFrame(currentFrameFramePointer, frameWidth, frameHeight);
    
                    // FREEZ FRAME DETECTION
                    if (previousFrame == null)
                    {
                        (currentFrame as IMFrame).FrameClone(out previousFrame, eMFrameClone.eMFC_Full_ForceCPU, eMFCC.eMFCC_ARGB32); //set gray and use it in clone
                        Marshal.ReleaseComObject(pMFrame);
                        Marshal.ReleaseComObject(currentFrame);
                        GC.Collect();
                        return;
                    }
                    previousFrame.FrameVideoGetBytes(out pcbSize, out previousFrameFramePointer);
                    _freez = true;
                    for (int y = 0; y < frameHeight; y++)
                    {
                        for (int x = 0; x < frameWidth; x++)
                        {
                            _freez = ComparePixel(currentFrameFramePointer, previousFrameFramePointer,threshold, frameWidth,x,y);
                            if ( !_freez || frameCount == nb_frames)
                            {
                                if (_freezFramesCount != 0)
                                {
                                    freezTotal.Add(_freezFramesCount);
                                    //info.freezframe[indexFreezInfo].final_frame = ((int)(freezTotal[indexFreezInfo]) + (int)(startFreez[indexFreezInfo]));
                                    //indexFreezInfo++;
                                }
                                    break;
                            }
                        }
                        if (!_freez)
                        {
                            info.freezframe[indexFreezInfo].final_frame = ((int)(freezTotal[indexFreezInfo]) + (int)(startFreez[indexFreezInfo]));
                            indexFreezInfo++;
                            break;
                        }
                    }
                    if (_freez)
                    {
                        if (_freezFramesCount == 0)
                        {
                            ObjFreezFrame = new FreezFrame();
                            info.freezframe.Add(ObjFreezFrame); //if not it will clone same values 
                            startFreez.Add(frameCount - 1);
                            info.freezframe[indexFreezInfo].start_frame = startFreez[indexFreezInfo];
                        }
                            
                           
                            _freezFramesCount++;
                    }
                    Marshal.ReleaseComObject(previousFrame);
                    (currentFrame as IMFrame).FrameClone(out previousFrame, eMFrameClone.eMFC_Full_ForceCPU, eMFCC.eMFCC_ARGB32); //swap clon2 to clone 1
                    Marshal.ReleaseComObject(currentFrame);
                    Marshal.ReleaseComObject(pMFrame);
                }
            }
            else
            {/*
                for (int i = 0; i < freezTotal.Count; i++)
                {
                    //ObjFreezFrame = new FreezFrame();
                    //info.freezframe.Add(ObjFreezFrame); //if not it will clone same values

                    //info.freezframe[i].start_frame = startFreez[i];

                    //info.freezframe[i].final_frame = ((int)(freezTotal[i]) + (int)(startFreez[i]));

                    Console.WriteLine("Start at Frame : " + startFreez[i] + "   *****************    To Frame : " + ((int)(freezTotal[i]) + (int)(startFreez[i])) + " **********  Total(s) : " + (int)(freezTotal[i]) + "   Frame(s)");

                }*/
                System.Xml.Serialization.XmlSerializer writer =
                new System.Xml.Serialization.XmlSerializer(typeof(Info));
                var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//" + _reportName +".xml";
                System.IO.FileStream file = System.IO.File.Create(path);
                writer.Serialize(file, info);
                file.Close();

                frameCount = 0;
                startFreez.Clear();
                freezTotal.Clear();
                Application.ExitThread();
            }
        }

        public bool ComparePixel(long currentFrameFramePointer, long previousFrameFramePointer,int threshold, int frameWidth, int x, int y)
        {
            unsafe
            {
                byte PixelValueCurrent = (byte)(GetPixelValue((uint*)currentFrameFramePointer, frameWidth, x, y));
                byte PixelValuePrevious = (byte)(GetPixelValue((uint*)previousFrameFramePointer, frameWidth, x, y));
                if (Math.Abs(PixelValueCurrent - PixelValuePrevious) > threshold) return false;
                else return true;
            }
        }
    }
}