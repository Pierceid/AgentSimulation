using AgentSimulation.Simulations;

namespace AgentSimulation.Structures.Events {
    public abstract class Event<T>(EventSimulationCore<T> simulationCore, double time) where T : class, new() {
        public EventSimulationCore<T> SimulationCore { get; set; } = simulationCore;
        public double Time { get; set; } = time;

        public abstract void Execute();
    }
}
