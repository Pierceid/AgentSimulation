namespace AgentSimulation.Statistics {
    public class Utility {
        public double Sum { get; private set; }
        private double? lastStart;

        public void AddSample(double time, bool isStart) {
            if (isStart) {
                lastStart = time;
            } else if (lastStart.HasValue) {
                Sum += time - lastStart.Value;
                lastStart = null;
            }
        }

        public double GetUtility(double totalTime) {
            return Sum / totalTime;
        }

        public void Clear() {
            Sum = 0;
            lastStart = null;
        }
    }
}