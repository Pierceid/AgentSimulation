namespace AgentSimulation.Statistics {
    public class Counter {
        public int Count { get; set; }

        public void AddSample(int count) {
            Count += count;
        }

        public int GetCounter() {
            return Count;
        }

        public void Clear() {
            Count = 0;
        }
    }
}
