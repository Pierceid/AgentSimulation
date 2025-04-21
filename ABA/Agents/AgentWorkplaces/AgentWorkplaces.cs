using OSPABA;
using Agents.AgentWorkplaces.ContinualAssistants;
using Simulation;
using Agents.AgentWorkplaces.InstantAssistants;

namespace Agents.AgentWorkplaces {
    //meta! id="39"
    public class AgentWorkplaces : OSPABA.Agent {
        public AgentWorkplaces(int id, OSPABA.Simulation mySim, Agent parent) : base(id, mySim, parent) {
            Init();
        }

        override public void PrepareReplication() {
            base.PrepareReplication();
        }

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			new ManagerWorkplaces(SimId.ManagerWorkplaces, MySim, this);
			new Assembling(SimId.Assembling, MySim, this);
			new Mounting(SimId.Mounting, MySim, this);
			new AssignWorkplace(SimId.AssignWorkplace, MySim, this);
			new Painting(SimId.Painting, MySim, this);
			new Cutting(SimId.Cutting, MySim, this);
			new Pickling(SimId.Pickling, MySim, this);
			AddOwnMessage(Mc.GetWorkerForCutting);
			AddOwnMessage(Mc.GetFreeWorkplace);
			AddOwnMessage(Mc.Init);
			AddOwnMessage(Mc.GetWorkerForPainting);
			AddOwnMessage(Mc.GetWorkerForMounting);
			AddOwnMessage(Mc.AssignWorkplace);
			AddOwnMessage(Mc.GetWorkerForAssembling);
			AddOwnMessage(Mc.GetWorkerForPickling);
		}
		//meta! tag="end"
    }
}