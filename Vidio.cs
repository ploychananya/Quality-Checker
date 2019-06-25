using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qualıty_Checker
{
    public class Vidio
    {
        public int idx;
        public int pid;
        public string codec;
        public string codec_name;
        public string codec_tag;
        public string profile;

        public int width;
        public int height;
        public int has_b_frames;

        public string pixel_ar;
        public string display_ar;
        public string key_distance;
        public double codec_frame_rate;
        public string r_frame_rate;
        public string avg_frame_rate;
        public string time_base;

        public double duration_ts;
        public double duration;
        public double bit_rate;
        public int nb_frames;

        public Metadata metadata;

    }
}
