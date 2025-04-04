using AgentSimulation.Simulations;
using AgentSimulation.Structures.Enums;
using AgentSimulation.Structures.Objects;

namespace AgentSimulation.Structures.Events {
    public class PaintingStartEvent : Event<ProductionManager> {
        public Order Order { get; }
        public Worker Worker { get; }

        public PaintingStartEvent(EventSimulationCore<ProductionManager> simulationCore, double time, Order order, Worker worker) : base(simulationCore, time) {
            Order = order;
            Worker = worker;
        }

        public override void Execute() {
            if (SimulationCore.Data is not ProductionManager manager) return;

            if (manager.QueueD.Count > 0) {
                manager.QueueC.AddFirst(Order);

                Order nextOrder = manager.QueueD.First();
                manager.QueueD.RemoveFirst();

                SimulationCore.EventCalendar.Enqueue(new MountingStartEvent(SimulationCore, Time, nextOrder, Worker), Time); return;
            }

            double movingTime = 0.0;

            if (Worker.Workplace == null) {
                movingTime += SimulationCore.Generators.WorkerMoveToStorageTime.Next();
            } else if (Worker.Workplace != Order.Workplace) {
                movingTime += SimulationCore.Generators.WorkerMoveBetweenStationsTime.Next();
            }

            Worker.Workplace = Order.Workplace;
            Worker.SetOrder(Order);
            manager.AverageUtilityC.AddSample(Time, true);

            double paintingTime = Order.Type switch {
                ProductType.Chair => SimulationCore.Generators.ChairPaintingTime.Next(),
                ProductType.Table => SimulationCore.Generators.TablePaintingTime.Next(),
                ProductType.Wardrobe => SimulationCore.Generators.WardrobePaintingTime.Next(),
                _ => 0.0
            };

            double nextEventTime = Time + movingTime + paintingTime;

            SimulationCore.EventCalendar.Enqueue(new PaintingEndEvent(SimulationCore, nextEventTime, Order, Worker), nextEventTime);
        }
    }
}
