using OSPABA;
using Simulation;

namespace Agents.AgentCarpentry {
    //meta! id="4"
    public class AgentCarpentry : OSPABA.Agent {
        public AgentCarpentry(int id, OSPABA.Simulation mySim, Agent parent) : base(id, mySim, parent) {
            Init();
        }

        override public void PrepareReplication() {
            base.PrepareReplication();
        }

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			new ManagerCarpentry(SimId.ManagerCarpentry, MySim, this);
			AddOwnMessage(Mc.GetWorkerForCutting);
			AddOwnMessage(Mc.DoPickling);
			AddOwnMessage(Mc.GetWorkerForMounting);
			AddOwnMessage(Mc.ProcessOrder);
			AddOwnMessage(Mc.DoAssembling);
			AddOwnMessage(Mc.GetWorkerForAssembling);
			AddOwnMessage(Mc.MoveToWorkplace);
			AddOwnMessage(Mc.GetWorkerForPainting);
			AddOwnMessage(Mc.GetFreeWorkplace);
			AddOwnMessage(Mc.Init);
			AddOwnMessage(Mc.DoMounting);
			AddOwnMessage(Mc.DoCutting);
			AddOwnMessage(Mc.DoPainting);
			AddOwnMessage(Mc.DoPreparing);
			AddOwnMessage(Mc.MoveToStorage);
			AddOwnMessage(Mc.GetWorkerForPickling);
		}
		//meta! tag="end"
    }
}