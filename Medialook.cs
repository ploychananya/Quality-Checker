using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using MPLATFORMLib;
//birkan.herguner@gmail.com
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Data;
using System.Diagnostics;
using System.Threading;
namespace Qualıty_Checker
{
    partial class Medialook : Form
    {
        Analyze analyze = new Analyze();
        Lib lib = new Lib();
        public Info info = new Info();
        M_AV_PROPS avProps;
        bool _freez = true;
        public MFrame clonedFrame;
        public MFrame previousFrame;
        long previousFrameFramePointer;
        public long framePointerCloned1;
        public long framePointerCloned2;
        public List<int> startFreez = new List<int>();
        public List<int> freezTotal = new List<int>();
        public List<int[]> audioLoudRange = new List<int[]>();
        public List<int[]> audioSilenRange = new List<int[]>();
        public int index_loud = 0;
        public int index_silen = 0;
        public double frameRate;
        public LoudVolume ObjLoud = new LoudVolume();
        public SilenceVolume ObjSilence = new SilenceVolume();
        SilenceVolume ObjSilence_;
        LoudVolume ObjLoud_;


        public int _freezFramesCount = 0;
        public int frameCount = 0; // so start at frame 1
        public byte threshold = 60;
        public FreezFrame ObjFreezFrame;
        public int index_freezFrameInfo = -1;
        public string _reportName;
        public int indexFreezInfo = 0;
        public MFileClass m_objReader = new MFileClass();
        private MRendererClass m_objRenderer = new MRendererClass();
        private MPreviewClass preview = new MPreviewClass();
        public float audioVuMax = -90;
        public float audioVuMin = 0;
        public float audioMaxthreshold = -20; // more than this very lound
        public float audioMinthreshold = -30; // less than this mean silence

        public Medialook(string filePath, Info infoFromAnalyze, string reportName)
        {
            _reportName = reportName;
            info = infoFromAnalyze;
            TryToOpenFile(filePath);
            frameRate = info.video[0].codec_frame_rate;
            info.freez_threshold = threshold;
            info.volume_max_threshold = audioMaxthreshold;
            info.volume_min_threshold = audioMinthreshold;
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
                MessageBox.Show(pathToFile + Environment.NewLine + ex.Message);
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
        private int _audioLoudCount;
        private int _audioSilenceCount;

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
        public void M_objReader_OnFrameSafe(string bsChannelID, object pMFrame)
        {
            frameCount++;
            int nb_frames = (int)(info.duration * info.video[0].codec_frame_rate);
            if (frameCount <= nb_frames) {
                //M_AV_PROPS avProps;
                (pMFrame as IMFrame).FrameAVPropsGet(out avProps);
                int numberOfChannels = avProps.audProps.nChannels;
                float[] outputChannelsVUmeterArr = avProps.ancData.audOutput.arrVUMeter;
                float[] outputChannelsRMS = avProps.ancData.audOutput.arrRMS;
                float[] outputChannelsVUPeaks = avProps.ancData.audOutput.arrVUPeaks;

                int entrance_count = 0;
                for (int i = 0; i < numberOfChannels; i++)
                {
                    
                    // Get the VU level value for the current audio channel
                    if (outputChannelsVUPeaks[i] > audioVuMax) //find MaxVu
                        audioVuMax = outputChannelsVUPeaks[i];
                    if (outputChannelsVUmeterArr[i] < audioVuMin) //find MinVu
                        audioVuMin = outputChannelsVUmeterArr[i];

                    if (outputChannelsVUmeterArr[i] > audioMaxthreshold && entrance_count == 0)//loud
                    {
                        if (_audioLoudCount == 0) //add first index
                        {
                            if (_audioSilenceCount != 0)
                            {//if silene frame in previous frame add Silencecount to the list 
                                //Console.WriteLine("Silence frame total"+ (_audioSilenceCount + audioSilenRange[index_silen][0] - 1));
                                audioSilenRange[index_silen][1] = ((_audioSilenceCount + audioSilenRange[index_silen][0]) - 1);//add frame count to audioSilenceRange
                                _audioSilenceCount = 0;
                                index_silen++;
                            }

                            audioLoudRange.Add(new int[2]); //index , value
                            audioLoudRange[index_loud][0] =  (frameCount - 1);

                        }
                        _audioLoudCount++;
                        entrance_count++;
                        //break;
                    }
                    else if (outputChannelsVUmeterArr[i] < audioMinthreshold && entrance_count == 0)
                    {
                        if (_audioSilenceCount == 0) //add first index
                        {
                            if (_audioLoudCount != 0)
                            {//if silene frame in previous frame add Silencecount to the list 
                                Console.WriteLine("Loud frame total" + (_audioLoudCount + audioLoudRange[index_loud][0] - 1));
                                audioLoudRange[index_loud][1] = ((_audioLoudCount+audioLoudRange[index_loud][0])-1);
                                _audioLoudCount = 0;
                                index_loud++;
                            }
                                                        //add frame count to audioSilenceRange
                            audioSilenRange.Add(new int[2]); //index , value
                            audioSilenRange[index_silen][0] = (frameCount - 1);
                            //index_silen++;


                        }
                        _audioSilenceCount ++;
                        entrance_count++;
                        //break;
                    }
                    else if(entrance_count == 0)
                    {
   
                        if (_audioLoudCount != 0)
                        {//if silene frame in previous frame add Silencecount to the list 
                            //Console.WriteLine("Loud frame total" + (_audioLoudCount + audioLoudRange[index_loud][0] - 1));
                            audioLoudRange[index_loud][1] = ((_audioLoudCount + audioLoudRange[index_loud][0]) - 1);
                            _audioLoudCount = 0;
                            index_loud++;
                            //entrance_count++;
                        }else if (_audioSilenceCount != 0)
                        {
                            //Console.WriteLine("Silence frame total" + (_audioSilenceCount + audioSilenRange[index_silen][0] - 1));
                            audioSilenRange[index_silen][1] = ((_audioSilenceCount + audioSilenRange[index_silen][0]) - 1);//add frame count to audioSilenceRange
                            _audioSilenceCount = 0;
                            index_silen++;
                            ///entrance_count++;
                        }
                        entrance_count++;
                    }

                    if (frameCount == nb_frames )
                    {
                
                        if (_audioLoudCount != 0)
                        {//if silene frame in previous frame add Silencecount to the list 
                            //Console.WriteLine("Loud frame total" + (_audioLoudCount + audioLoudRange[index_loud][0] - 1));
                            audioLoudRange[index_loud][1] = ((_audioLoudCount + audioLoudRange[index_loud][0]) - 1);
                            _audioLoudCount = 0;
                            index_loud++;
                        }
                        else if (_audioSilenceCount != 0)
                        {
                            //Console.WriteLine("Silence frame total" + (_audioSilenceCount + audioSilenRange[index_silen][0] - 1));
                            audioSilenRange[index_silen][1] = ((_audioSilenceCount + audioSilenRange[index_silen][0]) - 1);//add frame count to audioSilenceRange
                            _audioSilenceCount = 0;
                            index_silen++;
                        }
                        
                    }
                 
                }

                    //avProps.ancData.audOriginal.
                    int frameWidth = avProps.vidProps.nWidth;
                    int frameHeight = Math.Abs(avProps.vidProps.nHeight);
                    int pcbSize;
                    long currentFrameFramePointer;
                    MFrame currentFrame;
                    (pMFrame as IMFrame).FrameClone(out currentFrame, eMFrameClone.eMFC_Full_ForceCPU, eMFCC.eMFCC_ARGB32);
                    currentFrame.FrameVideoGetBytes(out pcbSize, out currentFrameFramePointer);

                    // FREEZ FRAME DETECTION
                    unsafe
                    {
                        GrayScaleFrame(currentFrameFramePointer, frameWidth, frameHeight);
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
                                _freez = ComparePixel(currentFrameFramePointer, previousFrameFramePointer, threshold, frameWidth, x, y);
                                if (!_freez || frameCount == nb_frames)
                                {
                                    if (_freezFramesCount != 0) freezTotal.Add(_freezFramesCount);
                                    _freezFramesCount = 0;
                                    break;
                                }
                            }
                            if (!_freez) break;
                        }
                        if (_freez)
                        {
                            if (_freezFramesCount == 0) startFreez.Add(frameCount - 1);
                            _freezFramesCount++;
                        }
                        Marshal.ReleaseComObject(previousFrame);
                        (currentFrame as IMFrame).FrameClone(out previousFrame, eMFrameClone.eMFC_Full_ForceCPU, eMFCC.eMFCC_ARGB32); //swap clon2 to clone 1
                        Marshal.ReleaseComObject(currentFrame);
                        Marshal.ReleaseComObject(pMFrame);
                    }
                }
            else{
                    InsertVolumeInfo(info, audioLoudRange, "loud");
                    InsertVolumeInfo(info, audioSilenRange, "silence");
                    InsertFreezFrameInfo(info, startFreez, freezTotal);
                    info.peak_volume = audioVuMax;
                    info.min_volume = audioVuMin; //insert volume info

                    System.Xml.Serialization.XmlSerializer writer =
                    new System.Xml.Serialization.XmlSerializer(typeof(Info));
                    var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//" + _reportName + ".xml";
                    System.IO.FileStream file = System.IO.File.Create(path);
                    writer.Serialize(file, info);
                    file.Close();

                    frameCount = 0;
                    startFreez.Clear();
                    freezTotal.Clear();
                    Application.ExitThread();
                }
        }
        public void InsertFreezFrameInfo(Info info,List<int> startFreez,List<int> freezTotal)
        {
            for (int i = 0; i < freezTotal.Count; i++)
            {
                ObjFreezFrame = new FreezFrame();
                info.freezframe.Add(ObjFreezFrame); //if not it will clone same values
                info.freezframe[i].start_frame = startFreez[i]; 
                info.freezframe[i].final_frame = ((int)(freezTotal[i]) + (int)(startFreez[i])) - 1;
                //Console.WriteLine("Start at Frame : " + startFreez[i] + "   *****************    To Frame : " + (((int)(freezTotal[i]) + (int)(startFreez[i])) - 1) + " **********  Total(s) : " + (int)(freezTotal[i]) + "   Frame(s)");  
            }
        }

        public void InsertVolumeInfo(Info info, List<int[]> Volume, string c)
        {
            for (int i = 0; i < Volume.Count; i++)
            {
                
                
                if (c=="silence")
                {
                    ObjSilence_ = new SilenceVolume();
                    info.silence.Add(ObjSilence_); //if not it will clone same values
                    info.silence[i].start_frame = Volume[i][0];
                    //Console.WriteLine("Silence final frame = " + Volume[i][1]);
                    info.silence[i].final_frame = Volume[i][1];
                    info.silence[i].duration_secs = Math.Abs((Volume[i][1] - Volume[i][0] + 1) / frameRate); 

                }
                else if (c=="loud")
                {
                    ObjLoud_ = new LoudVolume();
                    info.loudness.Add(ObjLoud_); //if not it will clone same values
                    info.loudness[i].start_frame = Volume[i][0];
                    //Console.WriteLine("Loud final frame = " + Volume[i][1]);
                    info.loudness[i].final_frame = Volume[i][1];
                    info.loudness[i].duration_secs = Math.Abs((Volume[i][1] - Volume[i][0] + 1) / frameRate);

                }
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