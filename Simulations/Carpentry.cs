using AgentSimulation.Statistics;
using AgentSimulation.Structures.Events;
using AgentSimulation.Structures.Objects;

namespace AgentSimulation.Simulations {
    public class Carpentry : EventSimulationCore<ProductionManager> {
        public ConfidenceInterval AverageOrderTime { get; set; }
        public Average AverageFinishedOrders { get; set; }
        public Average AveragePendingOrders { get; set; }
        public Average AverageUtilityA { get; set; }
        public Average AverageUtilityB { get; set; }
        public Average AverageUtilityC { get; set; }

        public Carpentry(int replicationStock, double endOfSimulationTime) : base(replicationStock, endOfSimulationTime) {
            this.Data = new ProductionManager();

            this.AverageOrderTime = new();
            this.AverageFinishedOrders = new();
            this.AveragePendingOrders = new();

            this.AverageUtilityA = new();
            this.AverageUtilityB = new();
            this.AverageUtilityC = new();
        }

        public override void BeforeSimulation() {
            base.BeforeSimulation();

            this.EventCalendar.Clear();
            this.Data.Clear();

            EventCalendar.Enqueue(new SystemEvent<ProductionManager>(this, SimulationTime), SimulationTime);
            EventCalendar.Enqueue(new OrderStartEvent(this, SimulationTime), SimulationTime);
        }

        public override void AfterSimulation() {
            if (CurrentReplication >= ReplicationStock * 0.01 && (ReplicationStock < 1000 || CurrentReplication % (ReplicationStock / 1000) == 0)) {
                Notify();
            }

            this.Data.AveragePendingOrders.AddSample(this.Data.QueueA.Count);

            this.AverageOrderTime.AddSample(this.Data.AverageOrderTime.GetAverage());
            this.AverageFinishedOrders.AddSample(this.Data.AverageFinishedOrders.GetCounter());
            this.AveragePendingOrders.AddSample(this.Data.AveragePendingOrders.GetCounter());

            this.AverageUtilityA.AddSample(this.Data.AverageUtilityA.GetUtility(this.SimulationTime));
            this.AverageUtilityB.AddSample(this.Data.AverageUtilityB.GetUtility(this.SimulationTime));
            this.AverageUtilityC.AddSample(this.Data.AverageUtilityC.GetUtility(this.SimulationTime));

            base.AfterSimulation();
        }

        public override void BeforeSimulationRun() {
            base.BeforeSimulationRun();

            this.EventCalendar.Clear();
            this.Data.Clear();
            this.AverageOrderTime.Clear();
            this.AverageFinishedOrders.Clear();
            this.AveragePendingOrders.Clear();

            this.AverageUtilityA.Clear();
            this.AverageUtilityB.Clear();
            this.AverageUtilityC.Clear();
        }

        public override void AfterSimulationRun() {
            base.AfterSimulationRun();
        }
    }
}
