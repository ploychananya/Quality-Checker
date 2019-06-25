using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qualıty_Checker
{
    public class Info
    {
        public int ts_programs;
        public int subtitle_track;
        public int subtitle_tracks;
        public int audio_tracks; 
        public int audio_track_used;
        public int video_tracks;
        public int video_track_used;
        public int subtitle_track_used;

        public string path;

        public string context_type;
        public int streams;
        public string format;
        public string format_name;
        public double duration;

        public double start_time;
        public double size;
        public double bitrate;
        public double bitrate_kbps;
        public Metadata metadata;
        public double kbps_avg_video;
        public double kbps_avg;
        public double kbps_avg_audio;

        public Vidio[] video = new Vidio[30];
        public Audio[] audio= new Audio[30];

        //public double kbps_avg_video;
    }
}
