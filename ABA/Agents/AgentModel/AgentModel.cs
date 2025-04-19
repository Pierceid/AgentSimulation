using OSPABA;
using Simulation;

namespace Agents.AgentModel {
    //meta! id="2"
    public class AgentModel : OSPABA.Agent {
        public AgentModel(int id, OSPABA.Simulation mySim, Agent parent) :
            base(id, mySim, parent) {
            Init();
        }

        override public void PrepareReplication() {
            base.PrepareReplication();
            Init();
        }

        //meta! userInfo="Generated code: do not modify", tag="begin"
        private void Init() {
            new ManagerModel(SimId.ManagerModel, MySim, this);
            AddOwnMessage(Mc.ProcessOrder);
            AddOwnMessage(Mc.OrderExit);
        }
        //meta! tag="end"
    }
}