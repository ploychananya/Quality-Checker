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

        public float peak_volume;
        public float min_volume;

        public byte freez_threshold;
        public float volume_max_threshold;
        public float volume_min_threshold;

        //public List<Vidio> video = new Vidio[video_tracks];
        //public Audio[] audio= new Audio[audio_tracks];
        public List<FreezFrame> freezframe = new List<FreezFrame>();
        public List<LoudVolume> loudness = new List<LoudVolume>();
        public List<SilenceVolume> silence = new List<SilenceVolume>();
        public List<Vidio> video = new List<Vidio>();
        public List<Audio> audio = new List<Audio>();
        



        //public double kbps_avg_video;
    }
}
