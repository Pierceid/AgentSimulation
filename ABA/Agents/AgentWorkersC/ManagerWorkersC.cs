using AgentSimulation.Structures;
using AgentSimulation.Structures.Entities;
using AgentSimulation.Structures.Enums;
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
            PetriNet?.Clear();
            Clear();
        }

        public void InitWorkers(int workersC) {
            lock (Workers) {
                for (int i = 0; i < workersC; i++) {
                    Workers.Add(new Worker(i, WorkerGroup.C));
                }
            }
        }

        public void Clear() {
            int count = Workers.Count;
            Workers.Clear();
            InitWorkers(count);
        }

        //meta! sender="AgentWorkers", id="156", type="Request"
        public void ProcessGetWorkerC(MessageForm message) {
            MyMessage myMessage = (MyMessage)message.CreateCopy();
            Worker? availableWorker = Workers.FirstOrDefault(w => !w.IsBusy);
            availableWorker?.SetProduct(myMessage.Product);

            if (myMessage.Product != null) {
                if (myMessage.Product.State == ProductState.Cut) {
                    myMessage.Product.WorkerToPaint = availableWorker;
                } else if (myMessage.Product.State == ProductState.Painted && myMessage.Product.IsPickled) {
                    myMessage.Product.WorkerToPickle = availableWorker;
                } else {
                    myMessage.Product.WorkerToMount = availableWorker;
                }
            }

            Response(myMessage);
        }

        //meta! sender="AgentWorkers", id="203", type="Notice"
        public void ProcessDeassignWorkerC(MessageForm message) {
            MyMessage myMessage = (MyMessage)message.CreateCopy();
            Worker? worker = myMessage.WorkerToRelease;

            if (worker != null && worker.Group == WorkerGroup.C) {
                var match = Workers.FirstOrDefault(w => w.Id == worker.Id);
                match?.SetState(WorkerState.WAITING);
                match?.Utility.AddSample(myMessage.DeliveryTime, false);
            }
        }

        //meta! userInfo="Process messages defined in code", id="0"
        public void ProcessDefault(MessageForm message) { }

        //meta! sender="AgentWorkers", id="266", type="Notice"
        public void ProcessAssignWorkerC(MessageForm message) {
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
                case Mc.GetWorkerC:
                    ProcessGetWorkerC(message);
                    break;

                case Mc.AssignWorkerC:
                    ProcessAssignWorkerC(message);
                    break;

                case Mc.DeassignWorkerC:
                    ProcessDeassignWorkerC(message);
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

        public new AgentWorkersC MyAgent => (AgentWorkersC)base.MyAgent;
    }
}