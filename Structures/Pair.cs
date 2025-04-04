namespace AgentSimulation.Structures {
    public class Pair<T, U>(T first, U second) {
        public T First { get; set; } = first;
        public U Second { get; set; } = second;
    }
}