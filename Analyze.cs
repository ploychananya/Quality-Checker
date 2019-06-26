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
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace Qualıty_Checker
{
    public partial class Analyze
    {
        private double rate;
        private string name;
        string info;
        string strFormat;
        M_AUD_PROPS audProps;



        public bool calculation = true;

        

        public void Method(string filePath, string quality)
        {
            Lib lib = new Lib();
            Info info = new Info();


            //while (calculation)
            //{
            //TryToOpenFile(filePath);


            //}
            //Console.WriteLine("finish Calculate");
            /////////////////////////////////////////////
            ///

            /*
            while (calculation)
            {
             */

            //m_objReader = new MFileClass();
            //m_objRenderer = new MRendererClass();
            //MPreviewClass preview = new MPreviewClass();
            //m_objReader.FileNameSet(filePath, "loop=true");
            //m_objReader.PropsSet("object::on_frame.sync", "true");
            //m_objReader.PropsSet("object::on_frame.data", "true");
            //m_objReader.FilePlayStart();
            //m_objReader.OnFrameSafe += M_objReader_OnFrameSafe;

            //}

            //////////////////////////////////////////////

            //MFrame myFrame = new MFrame() ;
            //MF_FRAME_INFO FrameInfo;

            MFileClass myFile = new MFileClass();

            myFile.FileNameSet(filePath, "");
            myFile.FilePlayStart(); //it has to play for respresent the statistic.
            info = lib.GetFileInfo(myFile, "file::info");
            //myFile.FileFrameGet(10.00, 1.0, out myFrame);

            //Book overview = new Book();
            //overview.title = "Serialization Overview";
            System.Xml.Serialization.XmlSerializer writer =
                new System.Xml.Serialization.XmlSerializer(typeof(Info));

            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//Result.xml";
            System.IO.FileStream file = System.IO.File.Create(path);

            writer.Serialize(file, info);
            file.Close();
            





            if (quality == "-solution")
            {

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

            }
            else if (quality == "-audTrack")
            {

            }
            else if (quality == "-volume")
            {

            }
            else if (quality == "-play")
            {

            }
            else if (quality == "-name")
            {

            }
            else if (quality == "-format")
            {

            }
            else if (quality == "-sRate")
            {

            }
        }


    }
        

}
