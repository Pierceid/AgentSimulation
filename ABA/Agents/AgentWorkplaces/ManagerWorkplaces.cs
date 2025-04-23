using AgentSimulation.Structures.Objects;
using OSPABA;
using Simulation;

namespace Agents.AgentWorkplaces {
    //meta! id="39"
    public class ManagerWorkplaces : OSPABA.Manager {
        public List<Workplace> Workplaces { get; set; } = new();

        public ManagerWorkplaces(int id, OSPABA.Simulation mySim, Agent myAgent) : base(id, mySim, myAgent) {
            Init();
        }

        override public void PrepareReplication() {
            base.PrepareReplication();

            PetriNet?.Clear();
        }

        public void InitWorkplaces(int workplaces) {
            Parallel.For(0, workplaces, w => { lock (Workplaces) { Workplaces.Add(new Workplace(w)); } });
        }

        public void Clear() {
            int count = Workplaces.Count;
            Workplaces.Clear();
            InitWorkplaces(count);
        }

        // Cutting
        //meta! sender="AgentCarpentry", id="115", type="Request"
        public void ProcessGetWorkerForCutting(MessageForm message) {
            SendWorkerToWork(message, Mc.DoCutting);
        }

        // Mounting
        //meta! sender="AgentCarpentry", id="119", type="Request"
        public void ProcessGetWorkerForMounting(MessageForm message) {
            SendWorkerToWork(message, Mc.DoMounting);
        }

        // Assembling
        //meta! sender="AgentCarpentry", id="118", type="Request"
        public void ProcessGetWorkerForAssembling(MessageForm message) {
            SendWorkerToWork(message, Mc.DoAssembling);
        }

        // Painting
        //meta! sender="AgentCarpentry", id="117", type="Request"
        public void ProcessGetWorkerForPainting(MessageForm message) {
            SendWorkerToWork(message, Mc.DoPainting);
        }

        // Pickling
        //meta! sender="AgentCarpentry", id="120", type="Request"
        public void ProcessGetWorkerForPickling(MessageForm message) {
            SendWorkerToWork(message, Mc.DoPickling);
        }

        //meta! sender="AgentCarpentry", id="170", type="Response"
        public void ProcessGetFreeWorkplace(MessageForm message) {
            MyMessage myMessage = (MyMessage)message.CreateCopy();
            Workplace? workplace = GetFreeWorkplace();

            if (workplace != null) {
                workplace.SetState(true);
            }

            myMessage.Workplace = workplace;
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


        //meta! userInfo="Removed from model"
        public void ProcessInit(MessageForm message) {
        }

        //meta! userInfo="Process messages defined in code", id="0"
        public void ProcessDefault(MessageForm message) {
        }

        //meta! userInfo="Generated code: do not modify", tag="begin"
        public void Init() {
        }

        override public void ProcessMessage(MessageForm message) {
            switch (message.Code) {
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
        //meta! tag="end"

        public new AgentWorkplaces MyAgent => (AgentWorkplaces)base.MyAgent;

        private Workplace? GetFreeWorkplace() {
            return Workplaces.FirstOrDefault(w => !w.IsOccupied);
        }
    }
}