using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qualıty_Checker
{
    class ColorManager : EventArgs
    {
        public ColorManager(ColorHandler.ARGB argb, ColorHandler.HSV HSV)
        {
            ARGB = argb;
            this.HSV = HSV;
        }

        public ColorHandler.ARGB ARGB { get; private set; }

        public ColorHandler.HSV HSV { get; private set; }
    }
}
}
