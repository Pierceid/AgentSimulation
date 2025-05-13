using System.IO;

namespace AgentSimulation.Structures {
    public class Constants {
        public const int REPLICATION_COUNT = 100;
        public const int SIMULATION_TIME = 249 * 8 * 60 * 60;
        public static string IMAGE_PATH = Path.GetFullPath(Path.Combine("..", "..", "..", "Images"));
    }
}
