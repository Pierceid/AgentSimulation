using OSPABA;
using Agents.AgentScope.ContinualAssistants;
using Simulation;

namespace Agents.AgentScope {
    //meta! id="3"
    public class AgentScope : OSPABA.Agent { 
        public AgentScope(int id, OSPABA.Simulation mySim, Agent parent) : base(id, mySim, parent) {
            Init();
        }

        override public void PrepareReplication() {
            base.PrepareReplication();
            // Setup component for the next replication
        }

        //meta! userInfo="Generated code: do not modify", tag="begin"
        private void Init() {
            new ManagerScope(SimId.ManagerScope, MySim, this);
            new OrderArrival(SimId.OrderArrival, MySim, this);

            AddOwnMessage(Mc.Init);
            AddOwnMessage(Mc.OrderEnter);
        }
        //meta! tag="end"
    }
}