using OSPABA;

namespace AgentSimulation.ABA.Simulation {
    public class CarpentryManager : OSPABA.Manager {
        public CarpentryManager(int id, OSPABA.Simulation mySim, Agent myAgent) : base(id, mySim, myAgent) {
        }

        public override void ProcessMessage(MessageForm message) {
            throw new NotImplementedException();
        }

        public CarpentrySimulation CarpentrySimulation { get { return (CarpentrySimulation)base.MySim; } }

        public CarpentryAgent CarpentryAgent { get { return (CarpentryAgent)base.MyAgent; } }
    }
}
