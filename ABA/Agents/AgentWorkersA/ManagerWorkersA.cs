using AgentSimulation.Structures.Enums;
using AgentSimulation.Structures.Objects;
using OSPABA;
using Simulation;

namespace Agents.AgentWorkersA {
    //meta! id="149"
    public class ManagerWorkersA : OSPABA.Manager {
        private Queue<Worker> availableWorkers;

        public ManagerWorkersA(int id, OSPABA.Simulation mySim, Agent myAgent) : base(id, mySim, myAgent) {
            Init();
        }

        override public void PrepareReplication() {
            base.PrepareReplication();

            if (PetriNet != null) {
                PetriNet.Clear();
            }

            // Reinitialize available workers at the beginning of each replication
            availableWorkers = new Queue<Worker>();

            for (int i = 0; i < ((MySimulation)MySim).WorkersACount; i++) {
                availableWorkers.Enqueue(new Worker(i, WorkerGroup.A));
            }
        }

        //meta! sender="AgentWorkers", id="159", type="Request"
        public void ProcessGetWorkerA(MessageForm message) {
            var myMessage = (MyMessage)message;

            if (availableWorkers.Count > 0) {
                myMessage.Worker = availableWorkers.Dequeue();
            } else {
                myMessage.Worker = null;
            }

            Response(myMessage);
        }

        //meta! userInfo="Process messages defined in code", id="0"
        public void ProcessDefault(MessageForm message) {

        }

        //meta! userInfo="Generated code: do not modify", tag="begin"
        public void Init() {
        }

        override public void ProcessMessage(MessageForm message) {
            switch (message.Code) {
                case Mc.GetWorkerA:
                    ProcessGetWorkerA(message);
                    break;

                default:
                    ProcessDefault(message);
                    break;
            }
        }
        //meta! tag="end"

        public new AgentWorkersA MyAgent {
            get {
                return (AgentWorkersA)base.MyAgent;
            }
        }
    }
}
