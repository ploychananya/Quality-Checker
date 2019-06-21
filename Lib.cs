using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPLATFORMLib;

namespace Qualıty_Checker
{
    public class Lib
    {
        public void GetFileInfo(MFileClass m_objMFReader, string propertyNode)
        {
            //string fileInfo = "";
            int nCount = 0;
            try
            {
                // get a number of properties
                m_objMFReader.PropsGetCount(propertyNode, out nCount);
            }
            catch (Exception) { }
            for (int i = 0; i < nCount; i++)
            {
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
                    Console.WriteLine(sRelName + " = " + sValue + Environment.NewLine);
                    //fileInfo += sRelName + " = " + sValue + Environment.NewLine;
                }

            }
            //return fileInfo;
        }
    }
}
