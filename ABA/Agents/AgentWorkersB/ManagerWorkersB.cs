using AgentSimulation.Structures;
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
            availableWorker?.SetIsBusy(true);
            availableWorker?.SetProduct(myMessage.Product);

            if (myMessage.Product != null) {
                myMessage.Product.WorkerToAssemble = availableWorker;
            }

            Response(myMessage);
        }

        //meta! sender="AgentWorkers", id="204", type="Notice"
        public void ProcessDeassignWorkerB(MessageForm message) {
            MyMessage myMessage = (MyMessage)message.CreateCopy();
            Worker? worker = myMessage.WorkerToRelease;

            if (worker != null && worker.Group == WorkerGroup.B) {
                worker.SetState(WorkerState.WAITING);
                var match = Workers.FirstOrDefault(w => w.Id == worker.Id);
                match?.SetState(worker.State);
                match?.Utility.AddSample(myMessage.DeliveryTime, false);
            }
        }

        //meta! userInfo="Process messages defined in code", id="0"
        public void ProcessDefault(MessageForm message) { }

        //meta! sender="AgentWorkers", id="268", type="Notice"
        public void ProcessAssignWorkerB(MessageForm message) {
            MyMessage myMessage = (MyMessage)message;
            var assignedWorker = myMessage.GetAssignedWorker();

            if (assignedWorker != null) {
                var match = Workers.FirstOrDefault(w => w.Id == assignedWorker.Id);
                match?.SetProduct(myMessage.Product);
                match?.SetWorkplace(myMessage.Workplace);
                match?.Utility.AddSample(myMessage.DeliveryTime, true);
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

        public double GetAverageUtility() {
            double time = 0.0;
            Workers.ForEach(w => time += w.Utility.GetUtility(Constants.SIMULATION_TIME));
            time /= Workers.Count;
            return time;
        }

        public new AgentWorkersB MyAgent => (AgentWorkersB)base.MyAgent;
    }
}