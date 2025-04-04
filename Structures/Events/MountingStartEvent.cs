using AgentSimulation.Simulations;
using AgentSimulation.Structures.Objects;

namespace AgentSimulation.Structures.Events {
    public class MountingStartEvent : Event<ProductionManager> {
        public Order Order { get; }
        public Worker Worker { get; }

        public MountingStartEvent(EventSimulationCore<ProductionManager> simulationCore, double time, Order order, Worker worker) : base(simulationCore, time) {
            Order = order;
            Worker = worker;
        }

        public override void Execute() {
            if (SimulationCore.Data is not ProductionManager manager) return;

            double movingTime = 0.0;

            if (Worker.Workplace == null) {
                movingTime += SimulationCore.Generators.WorkerMoveToStorageTime.Next();
            } else if (Worker.Workplace != Order.Workplace) {
                movingTime += SimulationCore.Generators.WorkerMoveBetweenStationsTime.Next();
            }

            Worker.Workplace = Order.Workplace;
            Worker.SetOrder(Order);
            manager.AverageUtilityC.AddSample(Time, true);

            double mountingTime = SimulationCore.Generators.WardrobeMountingTime.Next();
            double nextEventTime = Time + movingTime + mountingTime;

            SimulationCore.EventCalendar.Enqueue(new MountingEndEvent(SimulationCore, nextEventTime, Order, Worker), nextEventTime);
        }
    }
}
