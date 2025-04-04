using AgentSimulation.Structures;

namespace AgentSimulation.Randoms.Continuous {
    public class UniformC : GeneralRandom<double> {
        private double min;
        private double max;

        public UniformC(double min, double max) {
            this.min = min;
            this.max = max;
        }

        public override double Next() {
            return this.min + (this.max - this.min) * Generator.NextDouble();
        }
    }
}
