using OSPABA;
using Simulation;

namespace AgentSimulation.ABA.Simulation {
    public class CarpentryManager : OSPABA.Manager {
        public CarpentryManager(int id, OSPABA.Simulation mySim, Agent myAgent) : base(id, mySim, myAgent) {
        }

        public override void ProcessMessage(MessageForm message) {
            throw new NotImplementedException();
        }

        public MySimulation MySimulation { get { return (MySimulation)base.MySim; } }

        public CarpentryAgent CarpentryAgent { get { return (CarpentryAgent)base.MyAgent; } }
    }
}
