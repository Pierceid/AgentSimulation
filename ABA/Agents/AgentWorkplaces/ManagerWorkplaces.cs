using AgentSimulation.Structures.Entities;
using AgentSimulation.Structures.Enums;
using OSPABA;
using Simulation;

namespace Agents.AgentWorkplaces {
    //meta! id="39"
    public class ManagerWorkplaces : OSPABA.Manager {
        public List<Workplace> Workplaces { get; set; } = new();

        public ManagerWorkplaces(int id, OSPABA.Simulation mySim, Agent myAgent) : base(id, mySim, myAgent) {
            Init();
        }

        public override void PrepareReplication() {
            base.PrepareReplication();
            PetriNet?.Clear();
        }

        public void InitWorkplaces(int workplaces) {
            Parallel.For(0, workplaces, w => {
                lock (Workplaces) {
                    Workplaces.Add(new Workplace(w));
                }
            });
        }

        public void Clear() {
            int count = Workplaces.Count;
            Workplaces.Clear();
            InitWorkplaces(count);
        }
        //meta! sender="AgentCarpentry", id="257", type="Notice"
        public void ProcessAssignWorkplace(MessageForm message) {
            MyMessage myMessage = (MyMessage)message.CreateCopy();
            Workplace? workplace = GetFreeWorkplace();

            if (workplace != null) {
                workplace.Product = myMessage.Product;
                workplace.Worker = myMessage.WorkerToRelease;
            }

            myMessage.Workplace = workplace;
            myMessage.Code = GetNextProcessCode(myMessage.Product);
            myMessage.Addressee = MySim.FindAgent(SimId.AgentCarpentry);
            Request(myMessage);
        }

        //meta! sender="AgentCarpentry", id="115", type="Response"
        public void ProcessGetWorkerForCutting(MessageForm message) {
            SendWorkerToWork(message, Mc.DoCutting);
        }

        //meta! sender="AgentCarpentry", id="119", type="Response"
        public void ProcessGetWorkerForMounting(MessageForm message) {
            SendWorkerToWork(message, Mc.DoMounting);
        }

        //meta! sender="AgentCarpentry", id="118", type="Response"
        public void ProcessGetWorkerForAssembling(MessageForm message) {
            SendWorkerToWork(message, Mc.DoAssembling);
        }

        //meta! sender="AgentCarpentry", id="117", type="Response"
        public void ProcessGetWorkerForPainting(MessageForm message) {
            SendWorkerToWork(message, Mc.DoPainting);
        }

        //meta! sender="AgentCarpentry", id="120", type="Response"
        public void ProcessGetWorkerForPickling(MessageForm message) {
            SendWorkerToWork(message, Mc.DoPickling);
        }
        //meta! sender="AgentCarpentry", id="170", type="Request"
        public void ProcessGetFreeWorkplace(MessageForm message) {
            MyMessage myMessage = (MyMessage)message.CreateCopy();
            Workplace? workplace = GetFreeWorkplace();

            if (workplace != null) {
                workplace.SetState(true);
            }

            myMessage.Workplace = workplace;
            myMessage.Code = Mc.GetFreeWorkplace;
            myMessage.Addressee = MySim.FindAgent(SimId.AgentCarpentry);
            Response(myMessage);
        }
        //meta! sender="AgentCarpentry", id="73", type="Notice"
        public void ProcessDeassignWorkplace(MessageForm message) {
            MyMessage myMessage = (MyMessage)message.CreateCopy();

            if (myMessage.Workplace != null) {
                myMessage.Workplace.SetState(false);
            }

            myMessage.Workplace = null;
        }

        private void SendWorkerToWork(MessageForm message, int processCode) {
            MyMessage myMessage = (MyMessage)message.CreateCopy();
            myMessage.Code = processCode;
            myMessage.Addressee = MySim.FindAgent(SimId.AgentCarpentry);
            Notice(myMessage);
        }

        public void ProcessInit(MessageForm message) {

        }
        //meta! userInfo="Process messages defined in code", id="0"
        public void ProcessDefault(MessageForm message) {

        }

        public void Init() {

        }

        public override void ProcessMessage(MessageForm message) {
            switch (message.Code) {
                case Mc.AssignWorkplace:
                    ProcessAssignWorkplace(message);
                    break;

                case Mc.GetWorkerForCutting:
                    ProcessGetWorkerForCutting(message);
                    break;

                case Mc.GetWorkerForPainting:
                    ProcessGetWorkerForPainting(message);
                    break;

                case Mc.DeassignWorkplace:
                    ProcessDeassignWorkplace(message);
                    break;

                case Mc.GetWorkerForPickling:
                    ProcessGetWorkerForPickling(message);
                    break;

                case Mc.GetFreeWorkplace:
                    ProcessGetFreeWorkplace(message);
                    break;

                case Mc.GetWorkerForAssembling:
                    ProcessGetWorkerForAssembling(message);
                    break;

                case Mc.GetWorkerForMounting:
                    ProcessGetWorkerForMounting(message);
                    break;

                default:
                    ProcessDefault(message);
                    break;
            }
        }

        public new AgentWorkplaces MyAgent => (AgentWorkplaces)base.MyAgent;

        private Workplace? GetFreeWorkplace() {
            return Workplaces.FirstOrDefault(w => !w.IsOccupied);
        }

        private int GetNextProcessCode(Product? product) {
            return product?.State switch {
                ProductState.Raw => Mc.GetWorkerForCutting,
                ProductState.Cut => Mc.GetWorkerForPainting,
                ProductState.Painted => Mc.GetWorkerForPickling,
                ProductState.Pickled => Mc.GetWorkerForAssembling,
                ProductState.Assembled => Mc.GetWorkerForMounting,
                _ => Mc.GetWorkerForCutting
            };
        }
    }
}