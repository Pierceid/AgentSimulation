using OSPABA;
using Simulation;
namespace Agents.AgentModel {
    //meta! id="2"
    public class ManagerModel : OSPABA.Manager {
        public ManagerModel(int id, OSPABA.Simulation mySim, Agent myAgent) : base(id, mySim, myAgent) {
            Init();
        }

        override public void PrepareReplication() {
            base.PrepareReplication();
            // Setup component for the next replication

            if (PetriNet != null) {
                PetriNet.Clear();
            }
        }

        //meta! sender="AgentCarpentry", id="12", type="Response"
        public void ProcessProcessOrder(MessageForm message) {
        }

        //meta! sender="AgentScope", id="23", type="Notice"
        public void ProcessOrderExit(MessageForm message) {
        }

        //meta! userInfo="Process messages defined in code", id="0"
        public void ProcessDefault(MessageForm message) {
            switch (message.Code) {
            }
        }

        //meta! userInfo="Generated code: do not modify", tag="begin"
        public void Init() {
        }

        override public void ProcessMessage(MessageForm message) {
            switch (message.Code) {
                case Mc.OrderExit:
                    ProcessOrderExit(message);
                    break;

                case Mc.ProcessOrder:
                    ProcessProcessOrder(message);
                    break;

                default:
                    ProcessDefault(message);
                    break;
            }
        }
        //meta! tag="end"
        public new AgentModel MyAgent {
            get {
                return (AgentModel)base.MyAgent;
            }
        }
    }
}