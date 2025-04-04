using AgentSimulation.Structures;

namespace AgentSimulation.Randoms.Other {
    public class Triangular : GeneralRandom<double> {
        private double min;
        private double max;
        private double mode;

        public Triangular(double min, double max, double mode) {
            this.min = min;
            this.max = max;
            this.mode = mode;
        }

        public override double Next() {
            double rng = Generator.NextDouble();

            return rng < (this.mode - this.min) / (this.max - this.min)
                ? this.min + Math.Sqrt((this.max - this.min) * (this.mode - this.min) * rng)
                : this.max - Math.Sqrt((this.max - this.min) * (this.max - this.mode) * (1.0 - rng));
        }
    }
}
