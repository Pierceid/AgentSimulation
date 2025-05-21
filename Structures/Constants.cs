using System.IO;

namespace AgentSimulation.Structures {
    public class Constants {
        public const int REPLICATION_COUNT = 500;
        public const int SIMULATION_TIME = 249 * 8 * 60 * 60;
        public const int FPS = 21;
        public const int IMAGE_SIZE = 48;
        public const int ANIMATION_WIDTH = 780;
        public const int ANIMATION_HEIGHT = 700;
        public static string CONFIG_PATH = Path.GetFullPath(Path.Combine("..", "..", "..", "Files"));
        public static string IMAGE_PATH = Path.GetFullPath(Path.Combine("..", "..", "..", "Files", "Images"));
    }
}
