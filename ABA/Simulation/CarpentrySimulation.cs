using Agents.AgentModel;
using Agents.AgentScope;
using AgentSimulation.Generators;
using AgentSimulation.Statistics;
using Simulation;

namespace AgentSimulation.ABA.Simulation {
    public class CarpentrySimulation : OSPABA.Simulation {
        public RandomGenerators Generators { get; set; }
        public ConfidenceInterval AverageOrderTime { get; set; }
        public Average AverageFinishedOrders { get; set; }
        public Average AveragePendingOrders { get; set; }
        public Average AverageUtilityA { get; set; }
        public Average AverageUtilityB { get; set; }
        public Average AverageUtilityC { get; set; }
        public CarpentryAgent AgentCarpentry { get; set; }

        public CarpentrySimulation() {
            var agentModel = new AgentModel(SimId.AgentModel, this, null);
            new AgentScope(SimId.AgentScope, this, agentModel);
            this.AgentCarpentry = new CarpentryAgent(SimId.AgentCarpentry, this, agentModel);

            this.Generators = new();

            this.AverageOrderTime = new();
            this.AverageFinishedOrders = new();
            this.AveragePendingOrders = new();

            this.AverageUtilityA = new();
            this.AverageUtilityB = new();
            this.AverageUtilityC = new();
        }

        public override void PrepareSimulation() {
            base.PrepareSimulation();
        }

        public override void PrepareReplication() {
            base.PrepareReplication();
            Clear();
        }

        public override void ReplicationFinished() {
            base.ReplicationFinished();
        }

        public override void SimulationFinished() {
            base.SimulationFinished();
        }

        private void Clear() {
            this.AverageOrderTime.Clear();
            this.AverageFinishedOrders.Clear();
            this.AveragePendingOrders.Clear();

            this.AverageUtilityA.Clear();
            this.AverageUtilityB.Clear();
            this.AverageUtilityC.Clear();
        }
    }
}
