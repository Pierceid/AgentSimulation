using AgentSimulation.Structures;
using System.IO;

namespace AgentSimulation.Utilities {
    public static class Util {
        public static string FormatRange(object min, object max) {
            return $"<{min.ToString()?.Replace(',', '.')},{max.ToString()?.Replace(',', '.')})";
        }

        public static string FormatTime(double time) {
            int daySeconds = (int)(time % 28800);
            int days = (int)(time / 28800);
            int hours = (daySeconds / 3600) % 8 + 6;
            int minutes = (daySeconds % 3600) / 60;
            int seconds = daySeconds % 60;

            return $"{days:D2}d {hours:D2}h {minutes:D2}m {seconds:D2}s";
        }

        public static string GetFilePath(string fileName) {
            return Path.GetFullPath(Path.Combine(Constants.IMAGE_PATH, fileName));
        }
    }
}
