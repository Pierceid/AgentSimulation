using AgentSimulation.Flyweight;
using AgentSimulation.Simulations;
using AgentSimulation.Structures.Enums;
using AgentSimulation.Structures.Objects;

namespace AgentSimulation.Structures.Events {
    public class OrderStartEvent : Event<ProductionManager> {
        private OrderFlyweight orderFlyweight;

        public OrderStartEvent(EventSimulationCore<ProductionManager> simulationCore, double time) : base(simulationCore, time) {
            orderFlyweight = new OrderFlyweight();
        }

        public override void Execute() {
            if (SimulationCore.Data is not ProductionManager manager) return;

            var rng = SimulationCore.Generators.RNG.Next();
            ProductType orderType = rng < 0.5 ? ProductType.Table : rng < 0.65 ? ProductType.Chair : ProductType.Wardrobe;

            Product order = orderFlyweight.GetOrder(orderType, Time);
            manager.Orders.Add(order);

            if (manager.Orders.Count > 500) {
                manager.Orders.RemoveAll(o => o.State == ProductState.Finished);
            }

            Workplace workPlace = manager.GetOrCreateWorkplace();
            order.Workplace = workPlace;
            workPlace.AssignOrder(order);

            List<Worker> availableWorkersA = manager.GetAvailableWorkers(ProductState.Raw);

            if (availableWorkersA.Count > 0) {
                Worker nextWorker = availableWorkersA.First();
                Product nextOrder;

                if (manager.QueueA.Count > 0) {
                    nextOrder = manager.QueueA.First();
                    manager.QueueA.RemoveFirst();
                } else {
                    nextOrder = order;
                }

                if (nextOrder != order) {
                    manager.QueueA.AddLast(order);
                }

                SimulationCore.EventCalendar.Enqueue(new CuttingStartEvent(SimulationCore, Time, nextOrder, nextWorker), Time);
            } else {
                manager.QueueA.AddLast(order);
            }

            double nextArrivalTime = Time + SimulationCore.Generators.OrderArrivalTime.Next();

            SimulationCore.EventCalendar.Enqueue(new OrderStartEvent(SimulationCore, nextArrivalTime), nextArrivalTime);
        }
    }
}
