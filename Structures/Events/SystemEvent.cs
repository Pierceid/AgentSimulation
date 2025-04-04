using AgentSimulation.Simulations;

namespace AgentSimulation.Structures.Events {
    public class SystemEvent<T>(EventSimulationCore<T> simulationCore, double time) : Event<T>(simulationCore, time) where T : class, new() {
        public override void Execute() {
            Thread.Sleep((int)SimulationCore.UpdateTime);

            Time = SimulationCore.SimulationTime + (SimulationCore.Speed / SimulationCore.UpdateTime);
            SimulationCore.EventCalendar.Enqueue(this, Time);

            if (SimulationCore.Speed != double.MaxValue) {
                SimulationCore.Notify();
            }
        }
    }
}
