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
        bool _freez = true;
        public MFrame clonedFrame1;
        public MFrame clonedFrame2;
        public long framePointerCloned1;
        public long framePointerCloned2;
        public List<int> startFreez = new List<int>();
        public List<int> freezTotal = new List<int>();

        public int _freezFramesCount = 0;
        public int frameCount = 0;

        public int indexPrintList = 0;

        private MFileClass m_objReader = new MFileClass();

        private MRendererClass m_objRenderer = new MRendererClass();



        //second preview
        private MPreviewClass preview = new MPreviewClass();



        public Medialook(string filePath)
        {
            TryToOpenFile(filePath);
            //InitializeComponent();
        }

        private void TryToOpenFile(string pathToFile)
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


        public void M_objReader_OnFrameSafe(string bsChannelID, object pMFrame)//**********************************
        {

            //public MFrame clonedFrame1;
            //public MFrame clonedFrame2;

            M_AV_PROPS avProps;
            (pMFrame as IMFrame).FrameAVPropsGet(out avProps);
            int frameWidth = avProps.vidProps.nWidth;
            int frameHeight = Math.Abs(avProps.vidProps.nHeight);
            int pcbSize;
            long framePointer;
            /*
            public MFrame clonedFrame1;
            public MFrame clonedFrame2;
            public long framePointerCloned1;
            public long framePointerCloned2;
            */

            byte valuePixel;

            //int frameCount = 0;
            //int B_frameCount = 0;
            //int W_frameCount = 0;

            //int startIndexFreezFrame;
            //int _freezFramesCount = 0;

            //List<int> _freezRange = new List<int>();
            //(pMFrame as IMFrame).FrameClone(out clonedFrame, eMFrameClone.eMFC_Full_ForceCPU, eMFCC.eMFCC_ARGB32);

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

                for (int y = 0; y < frameHeight / 2; y++)
                {
                    for (int x = 0; x < frameWidth; x++)
                    {

                        blue = (byte*)videoData;
                        green = ((byte*)videoData) + 1;
                        red = ((byte*)videoData) + 2;
                        alpha = ((byte*)videoData) + 3;
                        grayScale = (byte)((*blue * 0.1f) + (*green * 0.6f) + (*red * 0.3f));
                        SetPixelValue((uint*)framePointer, frameWidth, x, y, grayScale); //set it to greyscale
                        videoData++;

                    }
                }
                // FREEZ FRAME DETECTION
                frameCount++; //frame count 

                byte threshold = 20;




                if (frameCount % 2 == 1)
                {
                    if (frameCount == 1)
                    {
                        (pMFrame as IMFrame).FrameClone(out clonedFrame1, eMFrameClone.eMFC_Full_ForceCPU, eMFCC.eMFCC_ARGB32); //set gray and use it in clone 
                    }

                    clonedFrame1.FrameVideoGetBytes(out pcbSize, out framePointerCloned1);
                    videoData = (uint*)framePointerCloned1;

                }
                else if (frameCount % 2 == 0)
                {

                    (pMFrame as IMFrame).FrameClone(out clonedFrame2, eMFrameClone.eMFC_Full_ForceCPU, eMFCC.eMFCC_ARGB32); //set gray and use it in clone 
                    clonedFrame2.FrameVideoGetBytes(out pcbSize, out framePointerCloned2);
                    videoData = (uint*)framePointerCloned2;


                    for (int y = 0; y < frameHeight / 2; y++)
                    {
                        for (int x = 0; x < frameWidth; x++)
                        {


                            //Console.WriteLine("in");
                            byte PixelValueClone1 = (byte)Math.Abs(GetPixelValue((uint*)framePointerCloned1, frameWidth, x, y));
                            byte PixelValueClone2 = (byte)Math.Abs(GetPixelValue((uint*)framePointerCloned2, frameWidth, x, y));


                            if ((PixelValueClone1 -PixelValueClone2) > threshold)
                            {
                                if (_freezFramesCount != 0)
                                {
                                    freezTotal.Add(_freezFramesCount);
                                    _freezFramesCount = 0;
                                }
                                _freez = false;
                                break;
                            }



                            //GetPixelValue((uint*)framePointer, frameWidth, x - 1, y);

                        }

                        if (!_freez)
                        {
                            break;
                        }
                    }

                    if (_freez)
                    {

                        //add first index to list
                        if (_freezFramesCount == 0)
                        {
                            startFreez.Add(frameCount);
                            Console.WriteLine("START AT :    "+frameCount);//print index when it add 
                        }
                            
                        //framFreezcount
                        _freezFramesCount++;
                        Console.WriteLine(_freezFramesCount);

                    }

                    Marshal.ReleaseComObject(clonedFrame1);

                    (clonedFrame2 as IMFrame).FrameClone(out clonedFrame1, eMFrameClone.eMFC_Full_ForceCPU, eMFCC.eMFCC_ARGB32); //swap clon2 to clone 1


                    _freez = true; //set to can calculate again

                    //Marshal.ReleaseComObject(clonedFrame1);
                    Marshal.ReleaseComObject(clonedFrame2); // wait for next clone

                    //Console.WriteLine("Freez Frame(s): " + _freezFramesCount);









                }



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

                /*
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
                }*/

            }
            //preview.ReceiverPutFrame(bsChannelID, clonedFrame);
            //Marshal.ReleaseComObject(clonedFrame1);
            Marshal.ReleaseComObject(pMFrame);
            //Marshal.ReleaseComObject(clonedFrame1);
            //Marshal.ReleaseComObject(clonedFrame2);
            GC.Collect();

            int i = 0;

            for (i = indexPrintList; i < freezTotal.Count; i++)
            {

                Console.WriteLine("Start at Frame : " + startFreez[i] + "   *****************    To Frame : " + ((int)(freezTotal[i])+ (int)(startFreez[i])) + " **********  Total(s) : " + (int)(freezTotal[i]) + "   Frame(s)");
                
            }
            indexPrintList = i;
            
           

           
        }


    }
}
