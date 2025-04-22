using AgentSimulation.Structures.Objects;
using OSPABA;
using Simulation;

namespace Agents.AgentWorkersC {
    //meta! id="151"
    public class ManagerWorkersC : OSPABA.Manager {
        public List<Worker> Workers { get; set; } = new();

        public ManagerWorkersC(int id, OSPABA.Simulation mySim, Agent myAgent) : base(id, mySim, myAgent) {
            Init();
        }

        override public void PrepareReplication() {
            base.PrepareReplication();

            if (PetriNet != null) {
                PetriNet.Clear();
            }

            Workers = ((MySimulation)MySim).WorkersC;
        }

        //meta! sender="AgentWorkers", id="156", type="Request"
        public void ProcessGetWorkerC(MessageForm message) {
            MyMessage myMessage = (MyMessage)message;
            Worker? availableWorker = Workers.FirstOrDefault(w => !w.IsBusy);

            if (availableWorker != null) {
                availableWorker.SetState(true);
                myMessage.Worker = availableWorker;
            } else {
                myMessage.Worker = null;
            }

            Response(myMessage);
        }

        //meta! userInfo="Process messages defined in code", id="0"
        public void ProcessDefault(MessageForm message) { }

        //meta! sender="AgentWorkers", id="203", type="Notice"
        public void ProcessDeassignWorkerC(MessageForm message) {
            MyMessage myMessage = (MyMessage)message;

            if (myMessage.Worker != null) {
                var match = Workers.FirstOrDefault(w => w.Id == myMessage.Worker.Id);
                match?.SetState(false);
                myMessage.Worker = null;
            }

            Notice(myMessage);
        }

        //meta! userInfo="Generated code: do not modify", tag="begin"
        public void Init() {
            Workers = ((MySimulation)MySim).WorkersA;
        }

        override public void ProcessMessage(MessageForm message) {
            switch (message.Code) {
                case Mc.DeassignWorkerC:
                    ProcessDeassignWorkerC(message);
                    break;

                case Mc.GetWorkerC:
                    ProcessGetWorkerC(message);
                    break;

                default:
                    ProcessDefault(message);
                    break;
            }
        }
        //meta! tag="end"

        public new AgentWorkersC MyAgent {
            get {
                return (AgentWorkersC)base.MyAgent;
            }
        }
    }
}