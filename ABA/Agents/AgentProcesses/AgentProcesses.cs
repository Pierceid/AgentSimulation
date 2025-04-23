using OSPABA;
using Agents.AgentProcesses.ContinualAssistants;
using Simulation;

namespace Agents.AgentProcesses {
    //meta! id="77"
    public class AgentProcesses : OSPABA.Agent {
        public AgentProcesses(int id, OSPABA.Simulation mySim, Agent parent) : base(id, mySim, parent) {
            Init();
        }

        override public void PrepareReplication() {
            base.PrepareReplication();
            // Setup component for the next replication
        }

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			new ManagerProcesses(SimId.ManagerProcesses, MySim, this);
			new Mounting(SimId.Mounting, MySim, this);
			new Preparing(SimId.Preparing, MySim, this);
			new Pickling(SimId.Pickling, MySim, this);
			new Painting(SimId.Painting, MySim, this);
			new Assembling(SimId.Assembling, MySim, this);
			new Cutting(SimId.Cutting, MySim, this);
			AddOwnMessage(Mc.DoPickling);
			AddOwnMessage(Mc.DoMounting);
			AddOwnMessage(Mc.DoCutting);
			AddOwnMessage(Mc.DoPainting);
			AddOwnMessage(Mc.DoPreparing);
			AddOwnMessage(Mc.DoAssembling);
		}
		//meta! tag="end"
    }
}