namespace AgentSimulation.Statistics {
    public class Average {
        public int Count { get; set; }
        public double Sum { get; set; }

        public Average() {
            Count = 0;
            Sum = 0;
        }

        public void AddSample(double value) {
            Count++;
            Sum += value;
        }

        public double GetAverage() {
            return Count == 0 ? 0 : Sum / Count;
        }

        public void Clear() {
            Count = 0;
            Sum = 0;
        }
    }
}
