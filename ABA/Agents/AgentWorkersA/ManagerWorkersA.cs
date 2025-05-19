using AgentSimulation.Structures;
using AgentSimulation.Structures.Enums;
using AgentSimulation.Structures.Objects;
using OSPABA;
using Simulation;

namespace Agents.AgentWorkersA {
    //meta! id="149"
    public class ManagerWorkersA : OSPABA.Manager {
        public List<Worker> Workers { get; set; } = new();

        public ManagerWorkersA(int id, OSPABA.Simulation mySim, Agent myAgent) : base(id, mySim, myAgent) {
            Init();
        }

        override public void PrepareReplication() {
            base.PrepareReplication();
            PetriNet?.Clear();
        }

        public void InitWorkers(int workersA) {
            Parallel.For(0, workersA, a => { lock (Workers) { Workers.Add(new Worker(a, WorkerGroup.A)); } });
        }

        public void Clear() {
            int workersA = Workers.Count;
            Workers.Clear();
            InitWorkers(workersA);
        }

        //meta! sender="AgentWorkers", id="159", type="Request"
        public void ProcessGetWorkerA(MessageForm message) {
            MyMessage myMessage = (MyMessage)message.CreateCopy();
            Worker? availableWorker = Workers.FirstOrDefault(w => !w.IsBusy);
            availableWorker?.SetProduct(myMessage.Product);

            if (myMessage.Product != null) {
                if (myMessage.Product.State == ProductState.Raw) {
                    myMessage.Product.WorkerToCut = availableWorker;
                } else {
                    myMessage.Product.WorkerToMount = availableWorker;
                }
            }

            Response(myMessage);
        }

        //meta! sender="AgentWorkers", id="205", type="Notice"
        public void ProcessDeassignWorkerA(MessageForm message) {
            MyMessage myMessage = (MyMessage)message.CreateCopy();
            Worker? worker = myMessage.WorkerToRelease;

            if (worker != null && worker.Group == WorkerGroup.A) {
                var match = Workers.FirstOrDefault(w => w.Id == worker.Id);
                match?.SetState(WorkerState.WAITING);
                match?.Utility.AddSample(myMessage.DeliveryTime, false);
            }
        }

        //meta! userInfo="Process messages defined in code", id="0"
        public void ProcessDefault(MessageForm message) { }

        //meta! sender="AgentWorkers", id="267", type="Notice"
        public void ProcessAssignWorkerA(MessageForm message) {
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
                case Mc.GetWorkerA:
                    ProcessGetWorkerA(message);
                    break;

                case Mc.AssignWorkerA:
                    ProcessAssignWorkerA(message);
                    break;

                case Mc.DeassignWorkerA:
                    ProcessDeassignWorkerA(message);
                    break;

                default:
                    ProcessDefault(message);
                    break;
            }
        }
        //meta! tag="end"

        public double GetAverageUtility() {
            return Workers.Average(w => w.Utility.GetUtility(Constants.SIMULATION_TIME));
        }

        public new AgentWorkersA MyAgent {
            get {
                return (AgentWorkersA)base.MyAgent;
            }
        }
    }
}