using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Qualıty_Checker
{
    //main class 
    
    public class Program
    {
        //        static void Main(string[] args)
        
        //[STAThread]
        
        static  void Main(string[] args)
        {
            Analyze analyze = new Analyze();
            string filePath ;
            while (true)
            {
                //var analyze = new Analyze();

                string input = Console.ReadLine();
                string[] qualitys = input.Split(' ');
                int qualitySize = qualitys.Length;

                if (qualitys[0] == "qc" && qualitys[1] != null) // have to protect when filepath does not works.
                {
                    int index = 2;
                    filePath= qualitys[1];
                  
                    //Application.Run(new Medialook(filePath));
                    while (index < qualitySize)
                    {
                        analyze.Method(filePath,qualitys[index]); //print each analyze quality
                        Application.Run(new Medialook(filePath));
                        index++;
                    }
                }else if(qualitys[0] == "close")
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
