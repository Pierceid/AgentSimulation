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
        private void Init() {
            new ManagerWorkers(SimId.ManagerWorkers, MySim, this);
            AddOwnMessage(Mc.GetWorkerForCutting);
            AddOwnMessage(Mc.GetWorkerForPainting);
            AddOwnMessage(Mc.Init);
            AddOwnMessage(Mc.GetWorkerForMounting);
            AddOwnMessage(Mc.GetWorkerC);
            AddOwnMessage(Mc.GetWorkerB);
            AddOwnMessage(Mc.GetWorkerA);
            AddOwnMessage(Mc.DeassignWorker);
            AddOwnMessage(Mc.GetWorkerForAssembling);
            AddOwnMessage(Mc.GetWorkerForPickling);
        }
        //meta! tag="end"
    }
}