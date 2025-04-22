using AgentSimulation.Structures.Objects;
using OSPABA;
using Simulation;

namespace Agents.AgentWorkersB {
    //meta! id="150"
    public class ManagerWorkersB : OSPABA.Manager {
        private List<Worker> workers = new();

        public ManagerWorkersB(int id, OSPABA.Simulation mySim, Agent myAgent) : base(id, mySim, myAgent) {
            Init();
        }

        override public void PrepareReplication() {
            base.PrepareReplication();

            if (PetriNet != null) {
                PetriNet.Clear();
            }

            workers = ((MySimulation)MySim).WorkersB;
        }

        //meta! sender="AgentWorkers", id="157", type="Request"
        public void ProcessGetWorkerB(MessageForm message) {
            MyMessage myMessage = (MyMessage)message;
            Worker? availableWorker = workers.FirstOrDefault(w => !w.IsBusy);

            if (availableWorker != null) {
                availableWorker.SetState(true);
                myMessage.Worker = availableWorker;
            } else {
                myMessage.Worker = null;
            }

            Response(myMessage);
        }

        //meta! sender="AgentWorkers", id="204", type="Notice"
        public void ProcessDeassignWorkerB(MessageForm message) {
            MyMessage myMessage = (MyMessage)message;

            if (myMessage.Worker != null) {
                var match = workers.FirstOrDefault(w => w.Id == myMessage.Worker.Id);
                match?.SetState(false);
                myMessage.Worker = null;
            }

            Notice(myMessage);
        }

        //meta! userInfo="Process messages defined in code", id="0"
        public void ProcessDefault(MessageForm message) {
            // Optional: Log unhandled codes
        }

        //meta! userInfo="Generated code: do not modify", tag="begin"
        public void Init() {
            workers = ((MySimulation)MySim).WorkersA;
        }

        override public void ProcessMessage(MessageForm message) {
            switch (message.Code) {
                case Mc.DeassignWorkerB:
                    ProcessDeassignWorkerB(message);
                    break;

                case Mc.GetWorkerB:
                    ProcessGetWorkerB(message);
                    break;

                default:
                    ProcessDefault(message);
                    break;
            }
        }
        //meta! tag="end"

        public new AgentWorkersB MyAgent {
            get {
                return (AgentWorkersB)base.MyAgent;
            }
        }
    }
}
