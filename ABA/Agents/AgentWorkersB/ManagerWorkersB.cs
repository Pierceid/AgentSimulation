using AgentSimulation.Structures.Enums;
using AgentSimulation.Structures.Objects;
using OSPABA;
using Simulation;

namespace Agents.AgentWorkersB {
    //meta! id="150"
    public class ManagerWorkersB : OSPABA.Manager {
        public List<Worker> Workers { get; set; } = new();

        public ManagerWorkersB(int id, OSPABA.Simulation mySim, Agent myAgent) : base(id, mySim, myAgent) {
            Init();
        }

        override public void PrepareReplication() {
            base.PrepareReplication();
            PetriNet?.Clear();
        }

        public void InitWorkers(int workersB) {
            Parallel.For(0, workersB, b => { lock (Workers) { Workers.Add(new Worker(b, WorkerGroup.B)); } });
        }

        public void Clear() {
            int count = Workers.Count;
            Workers.Clear();
            InitWorkers(count);
        }

        //meta! sender="AgentWorkers", id="157", type="Request"
        public void ProcessGetWorkerB(MessageForm message) {
            MyMessage myMessage = (MyMessage)message.CreateCopy();
            Worker? availableWorker = Workers.FirstOrDefault(w => !w.IsBusy);
            myMessage.Worker = availableWorker; availableWorker?.SetState(true);
            myMessage.Worker = availableWorker;
            Response(myMessage);
        }

        //meta! sender="AgentWorkers", id="204", type="Notice"
        public void ProcessDeassignWorkerB(MessageForm message) {
            MyMessage myMessage = (MyMessage)message;

            if (myMessage.Worker != null) {
                var match = Workers.FirstOrDefault(w => w.Id == myMessage.Worker.Id);
                match?.SetState(false);
            }
        }

        //meta! userInfo="Process messages defined in code", id="0"
        public void ProcessDefault(MessageForm message) { }

        //meta! sender="AgentWorkers", id="268", type="Notice"
        public void ProcessAssignWorkerB(MessageForm message) {
            MyMessage myMessage = (MyMessage)message;

            if (myMessage.Worker != null) {
                var match = Workers.FirstOrDefault(w => w.Id == myMessage.Worker.Id);
                match?.SetProduct(myMessage.Product);
                match?.SetWorkplace(myMessage.Workplace);
            }
        }

        //meta! userInfo="Generated code: do not modify", tag="begin"
        public void Init() {
        }

        override public void ProcessMessage(MessageForm message) {
            switch (message.Code) {
                case Mc.DeassignWorkerB:
                    ProcessDeassignWorkerB(message);
                    break;

                case Mc.AssignWorkerB:
                    ProcessAssignWorkerB(message);
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

        public new AgentWorkersB MyAgent => (AgentWorkersB)base.MyAgent;
    }
}