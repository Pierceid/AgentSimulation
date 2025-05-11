using OSPABA;
using Simulation;

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
			AddOwnMessage(Mc.GetWorkerForCutting);
			AddOwnMessage(Mc.GetFreeWorkplace);
			AddOwnMessage(Mc.GetWorkerForPainting);
			AddOwnMessage(Mc.GetWorkerForMounting);
			AddOwnMessage(Mc.AssignWorkplace);
			AddOwnMessage(Mc.GetWorkerForAssembling);
			AddOwnMessage(Mc.GetWorkerForPickling);
			AddOwnMessage(Mc.DeassignWorkplace);
		}
		//meta! tag="end"
    }
}