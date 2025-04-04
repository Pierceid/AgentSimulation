using AgentSimulation.Structures;

namespace AgentSimulation.Randoms.Other {
    public class Exponential : GeneralRandom<double> {
        private double rate;

        public Exponential(double rate) {
            this.rate = rate;
        }

        public override double Next() {
            double rng = Generator.NextDouble();

            return -Math.Log(1.0 - rng) * this.rate;
        }
    }
}
