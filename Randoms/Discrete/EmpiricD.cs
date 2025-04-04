using AgentSimulation.Structures;

namespace AgentSimulation.Randoms.Discrete {
    public class EmpiricD : GeneralRandom<int> {
        private List<EmpiricData<int>> samples;
        private List<UniformD> generators;

        public EmpiricD(EmpiricData<int>[] samples) {
            this.samples = new(samples.Length);
            this.generators = new(samples.Length);

            double cumProb = 0.0;

            samples.ToList().ForEach(s => {
                EmpiricData<int> data = new(s.Range.First, s.Range.Second, s.Probability + cumProb);
                this.samples.Add(data);
                this.generators.Add(new(s.Range.First, s.Range.Second));
                cumProb += s.Probability;
            });
        }

        public override int Next() {
            double rng = Generator.NextDouble();

            for (int i = 0; i < this.samples.Count; i++) {
                if (rng < this.samples[i].Probability) {
                    return this.generators[i].Next();
                }
            }

            throw new ArgumentException("Invalid probability distribution");
        }
    }
}
