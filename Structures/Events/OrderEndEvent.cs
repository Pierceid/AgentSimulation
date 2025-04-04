using AgentSimulation.Simulations;
using AgentSimulation.Structures.Enums;
using AgentSimulation.Structures.Objects;

namespace AgentSimulation.Structures.Events {
    public class OrderEndEvent : Event<ProductionManager> {
        public Order Order { get; }
        public Worker Worker { get; }

        public OrderEndEvent(EventSimulationCore<ProductionManager> simulationCore, double time, Order order, Worker worker) : base(simulationCore, time) {
            Order = order;
            Worker = worker;
        }

        public override void Execute() {
            if (SimulationCore.Data is not ProductionManager manager) return;

            if (Order != null) {
                Order.State = ProductState.Finished;
                Order.EndTime = Time;
                manager.AverageOrderTime.AddSample(Order.EndTime - Order.StartTime);
                manager.AverageFinishedOrders.AddSample(1);
            }
        }
    }
}
