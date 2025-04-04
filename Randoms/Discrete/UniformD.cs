using AgentSimulation.Structures;

namespace AgentSimulation.Randoms.Discrete {
    public class UniformD : GeneralRandom<int> {
        private int min;
        private int max;

        public UniformD(int min, int max) {
            this.min = min;
            this.max = max;
        }

        public override int Next() {
            return Generator.Next(this.min, this.max);
        }
    }
}
