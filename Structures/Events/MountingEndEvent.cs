using AgentSimulation.Simulations;
using AgentSimulation.Structures.Enums;
using AgentSimulation.Structures.Objects;

namespace AgentSimulation.Structures.Events {
    public class MountingEndEvent : Event<ProductionManager> {
        public Product Order { get; }
        public Worker Worker { get; }

        public MountingEndEvent(EventSimulationCore<ProductionManager> simulationCore, double time, Product order, Worker worker) : base(simulationCore, time) {
            Order = order;
            Worker = worker;
        }

        public override void Execute() {
            if (SimulationCore.Data is not ProductionManager manager) return;

            Order.State = ProductState.Mounted;

            manager.Workplaces.FirstOrDefault(wp => wp.Order == Order)?.SetState(false);

            Worker.SetProduct(null);
            manager.AverageUtilityC.AddSample(Time, false);

            SimulationCore.EventCalendar.Enqueue(new OrderEndEvent(SimulationCore, Time, Order, Worker), Time);

            List<Worker> availableWorkersC = manager.GetAvailableWorkers(ProductState.Assembled);

            if (manager.QueueD.Count > 0 && availableWorkersC.Count > 0) {
                Worker nextWorker = availableWorkersC.First();
                Product nextOrder = manager.QueueD.First();
                manager.QueueD.RemoveFirst();

                SimulationCore.EventCalendar.Enqueue(new MountingStartEvent(SimulationCore, Time, nextOrder, nextWorker), Time);
            } else if (manager.QueueC.Count > 0 && availableWorkersC.Count > 0) {
                Worker nextWorker = availableWorkersC.First();
                Product nextOrder = manager.QueueC.First();
                manager.QueueC.RemoveFirst();

                SimulationCore.EventCalendar.Enqueue(new PaintingStartEvent(SimulationCore, Time, nextOrder, nextWorker), Time);
            }
        }
    }
}
