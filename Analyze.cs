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
    public class Analyze
    {
        public Info info;
        private string infoCode = "file::info";
        public Info GetAllInfo(string filePath)
        {
            Lib lib = new Lib();
            info = new Info();
            MFileClass myFile = new MFileClass();
            myFile.FileNameSet(filePath, "");
            myFile.FilePlayStart(); //it has to play for respresent the statistic.
            info = lib.GetFileInfo(myFile, infoCode);
            return info;
         
        }


    }
        

}
