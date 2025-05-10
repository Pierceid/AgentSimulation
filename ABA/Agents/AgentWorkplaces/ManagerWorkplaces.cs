using AgentSimulation.Structures.Enums;
using AgentSimulation.Structures.Objects;
using OSPABA;
using Simulation;
using System.Windows;

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

        public void ProcessAssignWorkplace(MessageForm message) {
            MessageBox.Show("ProcessAssignWorkplace()");
            MyMessage myMessage = (MyMessage)message.CreateCopy();
            Workplace? workplace = GetFreeWorkplace();

            if (workplace != null) {
                workplace.Product = myMessage.Product;
                workplace.Worker = myMessage.Worker;
            }

            myMessage.Workplace = workplace;
            myMessage.Code = GetNextProcessCode(myMessage.Product);
            myMessage.Addressee = MySim.FindAgent(SimId.AgentCarpentry);
            Request(myMessage);
        }

        // Cutting
        public void ProcessGetWorkerForCutting(MessageForm message) {
            MessageBox.Show("ProcessGetWorkerForCutting()");
            SendWorkerToWork(message, Mc.DoCutting);
        }

        // Mounting
        public void ProcessGetWorkerForMounting(MessageForm message) {
            MessageBox.Show("ProcessGetWorkerForMounting()");
            SendWorkerToWork(message, Mc.DoMounting);
        }

        // Assembling
        public void ProcessGetWorkerForAssembling(MessageForm message) {
            MessageBox.Show("ProcessGetWorkerForAssembling()");
            SendWorkerToWork(message, Mc.DoAssembling);
        }

        // Painting
        public void ProcessGetWorkerForPainting(MessageForm message) {
            MessageBox.Show("ProcessGetWorkerForPainting()");
            SendWorkerToWork(message, Mc.DoPainting);
        }

        // Pickling
        public void ProcessGetWorkerForPickling(MessageForm message) {
            MessageBox.Show("ProcessGetWorkerForPickling()");
            SendWorkerToWork(message, Mc.DoPickling);
        }

        public void ProcessGetFreeWorkplace(MessageForm message) {
            MessageBox.Show("ProcessGetFreeWorkplace()");
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

        public void ProcessDeassignWorkplace(MessageForm message) {
            MessageBox.Show("ProcessDeassignWorkplace()");
            MyMessage myMessage = (MyMessage)message.CreateCopy();

            if (myMessage.Workplace != null) {
                myMessage.Workplace.SetState(false);
            }

            myMessage.Workplace = null;
        }

        private void SendWorkerToWork(MessageForm message, int processCode) {
            MessageBox.Show($"SendWorkerToWork() - ProcessCode: {processCode}");
            MyMessage myMessage = (MyMessage)message.CreateCopy();
            myMessage.Code = processCode;
            myMessage.Addressee = MySim.FindAgent(SimId.AgentCarpentry);
            Notice(myMessage);
        }

        public void ProcessInit(MessageForm message) {

        }

        public void ProcessDefault(MessageForm message) {

        }

        public void Init() {

        }

        public override void ProcessMessage(MessageForm message) {
            MessageBox.Show($"ProcessMessage() - Code: {message.Code}");
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
            MessageBox.Show($"GetNextProcessCode() - State: {product?.State}");
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
