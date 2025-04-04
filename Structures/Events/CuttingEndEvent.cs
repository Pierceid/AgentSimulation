using AgentSimulation.Simulations;
using AgentSimulation.Structures.Enums;
using AgentSimulation.Structures.Objects;

namespace AgentSimulation.Structures.Events {
    public class CuttingEndEvent : Event<ProductionManager> {
        public Order Order { get; }
        public Worker Worker { get; }

        public CuttingEndEvent(EventSimulationCore<ProductionManager> simulationCore, double time, Order order, Worker worker) : base(simulationCore, time) {
            Order = order;
            Worker = worker;
        }

        public override void Execute() {
            if (SimulationCore.Data is not ProductionManager manager) return;

            Order.State = ProductState.Cut;
            manager.QueueC.AddLast(Order);

            Worker.SetOrder(null);
            manager.AverageUtilityA.AddSample(Time, false);

            List<Worker> availableWorkersC = manager.GetAvailableWorkers(ProductState.Cut);
            List<Worker> availableWorkersA = manager.GetAvailableWorkers(ProductState.Raw);

            if (availableWorkersC.Count > 0 && manager.QueueC.Count > 0) {
                Worker nextWorker = availableWorkersC.First();
                Order nextOrder = manager.QueueC.First();
                manager.QueueC.RemoveFirst();

                SimulationCore.EventCalendar.Enqueue(new PaintingStartEvent(SimulationCore, Time, nextOrder, nextWorker), Time);
            }

            if (availableWorkersA.Count > 0 && manager.QueueA.Count > 0) {
                Worker nextWorker = availableWorkersA.First();
                Order nextOrder = manager.QueueA.First();
                manager.QueueA.RemoveFirst();

                SimulationCore.EventCalendar.Enqueue(new CuttingStartEvent(SimulationCore, Time, nextOrder, nextWorker), Time);
            }
        }
    }
}
