using AgentSimulation.Simulations;
using AgentSimulation.Structures.Enums;
using AgentSimulation.Structures.Objects;

namespace AgentSimulation.Structures.Events {
    public class PaintingEndEvent : Event<ProductionManager> {
        public Product Order { get; }
        public Worker Worker { get; }

        public PaintingEndEvent(EventSimulationCore<ProductionManager> simulationCore, double time, Product order, Worker worker) : base(simulationCore, time) {
            Order = order;
            Worker = worker;
        }

        public override void Execute() {
            if (SimulationCore.Data is not ProductionManager manager) return;

            Order.State = ProductState.Painted;
            manager.QueueB.AddLast(Order);

            Worker.SetProduct(null);
            manager.AverageUtilityC.AddSample(Time, false);

            List<Worker> availableWorkersB = manager.GetAvailableWorkers(ProductState.Painted);
            List<Worker> availableWorkersC = manager.GetAvailableWorkers(ProductState.Assembled);

            if (availableWorkersB.Count > 0 && manager.QueueB.Count > 0) {
                Worker nextWorker = availableWorkersB.First();
                Product nextOrder = manager.QueueB.First();
                manager.QueueB.RemoveFirst();

                SimulationCore.EventCalendar.Enqueue(new AssemblyStartEvent(SimulationCore, Time, nextOrder, nextWorker), Time);
            }

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
