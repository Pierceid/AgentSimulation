using AgentSimulation.Structures;

namespace AgentSimulation.Randoms.Continuous {
    public class EmpiricC : GeneralRandom<double> {
        private List<EmpiricData<double>> samples;
        private List<UniformC> generators;

        public EmpiricC(EmpiricData<double>[] samples) {
            this.samples = new(samples.Length);
            this.generators = new(samples.Length);

            double cumProb = 0.0;

            samples.ToList().ForEach(s => {
                EmpiricData<double> data = new(s.Range.First, s.Range.Second, s.Probability + cumProb);
                this.samples.Add(data);
                this.generators.Add(new(s.Range.First, s.Range.Second));
                cumProb += s.Probability;
            });
        }

        public override double Next() {
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
