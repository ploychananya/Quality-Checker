using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qualıty_Checker
{
    public class Analyze
    {
        public void Method(string filePath, string quality)
        {
            // pull vidio file to variable;
            if (quality == "-solution")
            {
                //find solution 
                //show solution 
                Console.WriteLine("Solution : 720p");
            }else if (quality == "-codec")
            {
                //find solution 
                //show solution 
                Console.WriteLine("Codec: h263, h264");
            }else if (quality == "-freez")
            {
                //find solution 
                //show solution 
                Console.WriteLine("between frame: 100-110");
            }
        }
    }
}
