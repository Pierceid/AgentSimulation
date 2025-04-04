namespace AgentSimulation.Statistics {
    public class Utility {
        public int Count { get; set; }
        public double Sum { get; set; }
        public double LastTime { get; set; }

        public Utility() {
            Count = 0;
            Sum = 0;
            LastTime = 0;
        }

        public void AddSample(double totalTtime, bool isStart) {
            Count++;

            if (isStart) {
                LastTime = totalTtime;
            } else {
                Sum += totalTtime - LastTime;
            }
        }

        public double GetUtility(double time) {
            return Sum / time;
        }


        public void Clear() {
            Count = 0;
            Sum = 0;
            LastTime = 0;
        }
    }
}
