using OSPABA;
using Agents.AgentWarehouse.ContinualAssistants;
using Simulation;

namespace Agents.AgentWarehouse {
    //meta! id="77"
    public class AgentWarehouse : OSPABA.Agent {
        public AgentWarehouse(int id, OSPABA.Simulation mySim, Agent parent) : base(id, mySim, parent) {
            Init();
        }

        override public void PrepareReplication() {
            base.PrepareReplication();
        }

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			new ManagerWarehouse(SimId.ManagerWarehouse, MySim, this);
			new Preparing(SimId.Preparing, MySim, this);
			AddOwnMessage(Mc.Init);
			AddOwnMessage(Mc.DoPreparing);
		}
		//meta! tag="end"
    }
}