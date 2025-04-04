namespace AgentSimulation.Statistics {
    public class ConfidenceInterval {
        public int Count { get; set; }
        public double Sum { get; set; }
        public double SumOfSquares { get; set; }

        public ConfidenceInterval() {
            Count = 0;
            Sum = 0;
            SumOfSquares = 0;
        }

        public void AddSample(double value) {
            Count++;
            Sum += value;
            SumOfSquares += value * value;
        }

        public (double bottom, double top) GetConfidenceInterval() {
            if (Count < 30) return (double.NaN, double.NaN);

            var mean = GetMean();
            var sigma = GetSigma();
            var variation = 1.96 * sigma / Math.Sqrt(Count);

            return (mean - variation, mean + variation);
        }

        public void Clear() {
            Count = 0;
            Sum = 0;
            SumOfSquares = 0;
        }

        public double GetMean() {
            if (Count == 0) return 0;

            return Sum / Count;
        }

        private double GetSigma() {
            if (Count < 2) return 0;

            return Math.Sqrt((SumOfSquares - (Sum * Sum) / Count) / (Count - 1));
        }
    }
}
