using System;
using MPLATFORMLib;
//birkan.herguner@gmail.com
namespace Qualıty_Checker
{
    public partial class Analyze 
    {
        private double rate;
        private string name;
        string info;
        string strFormat;
        M_AUD_PROPS audProps;

        public void Method(string filePath, string quality)
        {
            Lib lib = new Lib();

            MFileClass myFile = new MFileClass();
            myFile.FileNameSet(filePath, "");
            myFile.FilePlayStart(); //it has to play for respresent the statistic.
            info = lib.GetFileInfo(myFile, "file::info");
            Console.WriteLine(info);
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

            } else if (quality == "-audTrack")
            {
               
            }else if (quality == "-volume")
            {

            }
            else if (quality == "-play")
            {
               
            } else if (quality == "-name")
            {
               
            } else if (quality == "-format")
            {
               
            } else if (quality == "-sRate")
            {
               
            }
        }
    }
    
}
