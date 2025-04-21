using AgentSimulation.Structures.Enums;
using AgentSimulation.Structures.Objects;
using OSPABA;
using Simulation;

namespace Agents.AgentWorkersB {
    //meta! id="150"
    public class ManagerWorkersB : OSPABA.Manager {
        private Queue<Worker> workers;

        public ManagerWorkersB(int id, OSPABA.Simulation mySim, Agent myAgent) : base(id, mySim, myAgent) {
            Init();
        }

        override public void PrepareReplication() {
            base.PrepareReplication();

            if (PetriNet != null) {
                PetriNet.Clear();
            }

            workers = new Queue<Worker>();

            for (int i = 0; i < ((MySimulation)MySim).WorkersBCount; i++) {
                workers.Enqueue(new Worker(i, WorkerGroup.B));
            }
        }

        //meta! sender="AgentWorkers", id="157", type="Request"
        public void ProcessGetWorkerB(MessageForm message) {
            var myMessage = (MyMessage)message;

            if (workers.Count > 0) {
                myMessage.Worker = workers.Dequeue();
            } else {
                myMessage.Worker = null;
            }

            Response(myMessage);
        }

        //meta! userInfo="Process messages defined in code", id="0"
        public void ProcessDefault(MessageForm message) { }

        //meta! userInfo="Generated code: do not modify", tag="begin"
        public void Init() { }

        override public void ProcessMessage(MessageForm message) {
            switch (message.Code) {
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
