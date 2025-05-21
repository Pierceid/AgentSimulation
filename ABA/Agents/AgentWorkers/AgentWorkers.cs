using OSPABA;
using Simulation;

namespace Agents.AgentWorkers {
    //meta! id="28"
    public class AgentWorkers : OSPABA.Agent {
        public AgentWorkers(int id, OSPABA.Simulation mySim, Agent parent) : base(id, mySim, parent) {
            Init();
        }

        override public void PrepareReplication() {
            base.PrepareReplication();
        }

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			new ManagerWorkers(SimId.ManagerWorkers, MySim, this);
			AddOwnMessage(Mc.GetWorkerToPaint);
			AddOwnMessage(Mc.GetWorkerToMount);
			AddOwnMessage(Mc.GetWorkerC);
			AddOwnMessage(Mc.GetWorkerB);
			AddOwnMessage(Mc.GetWorkerA);
			AddOwnMessage(Mc.AssignWorkerA);
			AddOwnMessage(Mc.AssignWorkerB);
			AddOwnMessage(Mc.AssignWorkerC);
			AddOwnMessage(Mc.GetWorkerToAssemble);
			AddOwnMessage(Mc.DeassignWorkerA);
			AddOwnMessage(Mc.GetWorkerToPickle);
			AddOwnMessage(Mc.DeassignWorkerC);
			AddOwnMessage(Mc.DeassignWorkerB);
			AddOwnMessage(Mc.GetWorkerToCut);
			AddOwnMessage(Mc.GetWorkerToCheck);
		}
		//meta! tag="end"
    }
}