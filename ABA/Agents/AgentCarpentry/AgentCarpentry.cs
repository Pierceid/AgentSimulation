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
			AddOwnMessage(Mc.GetWorkerToPaint);
			AddOwnMessage(Mc.ProcessOrder);
			AddOwnMessage(Mc.DoAssembling);
			AddOwnMessage(Mc.GetWorkerForAssembling);
			AddOwnMessage(Mc.MoveToWorkplace);
			AddOwnMessage(Mc.DoPrepare);
			AddOwnMessage(Mc.Init);
			AddOwnMessage(Mc.DoMounting);
			AddOwnMessage(Mc.DoCutting);
			AddOwnMessage(Mc.DoPaint);
			AddOwnMessage(Mc.DoPreparing);
			AddOwnMessage(Mc.DoAssemble);
			AddOwnMessage(Mc.MoveToStorage);
			AddOwnMessage(Mc.GetWorkerForPickling);
			AddOwnMessage(Mc.GetWorkerToCut);
			AddOwnMessage(Mc.GetWorkerForCutting);
			AddOwnMessage(Mc.DoPickling);
			AddOwnMessage(Mc.GetWorkerToMount);
			AddOwnMessage(Mc.DoCut);
			AddOwnMessage(Mc.GetWorkerForMounting);
			AddOwnMessage(Mc.DoMount);
			AddOwnMessage(Mc.GetWorkerToAssemble);
			AddOwnMessage(Mc.GetWorkerForPainting);
			AddOwnMessage(Mc.GetFreeWorkplace);
			AddOwnMessage(Mc.GetWorkerToPickle);
			AddOwnMessage(Mc.DoPainting);
			AddOwnMessage(Mc.DoPickle);
		}
		//meta! tag="end"
    }
}