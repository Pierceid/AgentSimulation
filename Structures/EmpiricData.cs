namespace AgentSimulation.Structures {
    public class EmpiricData<T>(T min, T max, double probability) {
        public Pair<T, T> Range { get; set; } = new(min, max);
        public double Probability { get; set; } = probability;
    }
}
