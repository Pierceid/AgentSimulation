using AgentSimulation.Simulations;

namespace AgentSimulation.Observer {
    public interface IObserver {
        void Refresh(SimulationCore simulationCore);
        void Refresh(OSPABA.Simulation simulation);
    }
}
