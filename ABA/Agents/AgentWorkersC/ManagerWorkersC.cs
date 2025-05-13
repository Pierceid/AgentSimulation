using AgentSimulation.Structures;
using AgentSimulation.Structures.Enums;
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
            PetriNet?.Clear();
        }

        public void InitWorkers(int workersC) {
            Parallel.For(0, workersC, c => { lock (Workers) { Workers.Add(new Worker(c, WorkerGroup.C)); } });
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
            availableWorker?.SetState(true);
            myMessage.Worker = availableWorker;
            Response(myMessage);
        }

        //meta! sender="AgentWorkers", id="203", type="Notice"
        public void ProcessDeassignWorkerC(MessageForm message) {
            MyMessage myMessage = (MyMessage)message;

            if (myMessage.Worker != null) {
                var match = Workers.FirstOrDefault(w => w.Id == myMessage.Worker.Id);
                match?.SetState(false);
                match?.Utility.AddSample(myMessage.DeliveryTime, false);
            }
        }

        //meta! userInfo="Process messages defined in code", id="0"
        public void ProcessDefault(MessageForm message) { }

        //meta! sender="AgentWorkers", id="266", type="Notice"
        public void ProcessAssignWorkerC(MessageForm message) {
            MyMessage myMessage = (MyMessage)message;

            if (myMessage.Worker != null) {
                var match = Workers.FirstOrDefault(w => w.Id == myMessage.Worker.Id);
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
            double time = 0.0;
            Workers.ForEach(w => time += w.Utility.GetUtility(Constants.SIMULATION_TIME));
            time /= Workers.Count;
            return time;
        }

        public new AgentWorkersC MyAgent => (AgentWorkersC)base.MyAgent;
    }
}