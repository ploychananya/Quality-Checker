using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Qualıty_Checker
{
    public class Program
    {
        static  void Main(string[] args)
        {
            Analyze analyze = new Analyze();
            string filePath ;
            string reportName;
            while (true)
            {
                string input = Console.ReadLine();
                string[] filePaths = input.Split(' ');
                if (filePaths[0] == "qc" && filePaths[1] != null) // have to protect when filepath does not works.
                {
                    filePath= filePaths[1];
                    reportName = filePaths[2];
                    Info info = analyze.GetAllInfo(filePath); //print each analyze quality
                    Application.Run(new Medialook(filePath,info,reportName));
                }else if(filePaths[0] == "close")
                {
                    break;
                }else
                {
                    Console.WriteLine("Please Try Again!");
                }
            }
        }
    }
}
