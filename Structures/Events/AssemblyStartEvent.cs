using AgentSimulation.Simulations;
using AgentSimulation.Structures.Enums;
using AgentSimulation.Structures.Objects;

namespace AgentSimulation.Structures.Events {
    public class AssemblyStartEvent : Event<ProductionManager> {
        public Order Order { get; }
        public Worker Worker { get; }

        public AssemblyStartEvent(EventSimulationCore<ProductionManager> simulationCore, double time, Order order, Worker worker) : base(simulationCore, time) {
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
            manager.AverageUtilityB.AddSample(Time, true);

            double assemblyTime = Order.Type switch {
                ProductType.Chair => SimulationCore.Generators.ChairAssemblyTime.Next(),
                ProductType.Table => SimulationCore.Generators.TableAssemblyTime.Next(),
                ProductType.Wardrobe => SimulationCore.Generators.WardrobeAssemblyTime.Next(),
                _ => 0.0
            };

            double nextEventTime = Time + movingTime + assemblyTime;

            SimulationCore.EventCalendar.Enqueue(new AssemblyEndEvent(SimulationCore, nextEventTime, Order, Worker), nextEventTime);
        }
    }
}
