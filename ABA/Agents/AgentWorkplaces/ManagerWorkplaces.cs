using AgentSimulation.Structures.Enums;
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
            StartWorkWithAssistant(message, SimId.Cutting, Mc.Start);
        }

        // Mounting
		//meta! sender="AgentCarpentry", id="119", type="Request"
		public void ProcessGetWorkerForMounting(MessageForm message) {
            StartWorkWithAssistant(message, SimId.Mounting, Mc.Start);
        }

        // Assembling
		//meta! sender="AgentCarpentry", id="118", type="Request"
		public void ProcessGetWorkerForAssembling(MessageForm message) {
            StartWorkWithAssistant(message, SimId.Assembling, Mc.Start);
        }

        // Painting
		//meta! sender="AgentCarpentry", id="117", type="Request"
		public void ProcessGetWorkerForPainting(MessageForm message) {
            StartWorkWithAssistant(message, SimId.Painting, Mc.Start);
        }

        // Pickling
		//meta! sender="AgentCarpentry", id="120", type="Request"
		public void ProcessGetWorkerForPickling(MessageForm message) {
            StartWorkWithAssistant(message, SimId.Pickling, Mc.Start);
        }

		//meta! sender="AgentCarpentry", id="170", type="Response"
		public void ProcessGetFreeWorkplace(MessageForm message) {
            MyMessage myMessage = (MyMessage)message;
            Workplace? workplace = GetFreeWorkplace();

            if (workplace != null) {
                workplace.SetState(true);
            }

            myMessage.Workplace = workplace;
        }

		//meta! userInfo="Removed from model"
        public void ProcessFinishCutting(MessageForm message) {
            MyMessage myMessage = (MyMessage)message;

            if (myMessage.Product != null) {
                myMessage.Product.State = ProductState.Cut;
            }

            FreeWorkplaceAndNotify(myMessage);
        }

		//meta! userInfo="Removed from model"
        public void ProcessFinishPainting(MessageForm message) {
            MyMessage myMessage = (MyMessage)message;

            if (myMessage.Product != null) {
                myMessage.Product.State = ProductState.Painted;
            }

            FreeWorkplaceAndNotify(myMessage);
        }

		//meta! userInfo="Removed from model"
        public void ProcessFinishAssembling(MessageForm message) {
            MyMessage myMessage = (MyMessage)message;

            if (myMessage.Product != null) {
                myMessage.Product.State = ProductState.Assembled;
            }

            FreeWorkplaceAndNotify(myMessage);
        }

		//meta! userInfo="Removed from model"
        public void ProcessFinishMounting(MessageForm message) {
            MyMessage myMessage = (MyMessage)message;

            if (myMessage.Product != null) {
                myMessage.Product.State = ProductState.Mounted;
            }

            FreeWorkplaceAndNotify(myMessage);
        }

		//meta! userInfo="Removed from model"
        public void ProcessFinishPickling(MessageForm message) {
            MyMessage myMessage = (MyMessage)message;

            if (myMessage.Product != null) {
                myMessage.Product.State = ProductState.Pickled;
            }

            FreeWorkplaceAndNotify(myMessage);
        }

		//meta! sender="AgentCarpentry", id="73", type="Notice"
		public void ProcessDeassignWorkplace(MessageForm message) {
            MyMessage myMessage = (MyMessage)message;

            if (myMessage.Workplace != null) {
                myMessage.Workplace.SetState(false);
            }

            myMessage.Workplace = null;
        }

		//meta! userInfo="Removed from model"
        public void ProcessInit(MessageForm message) {
        }

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message) {
        }

		//meta! userInfo="Generated code: do not modify", tag="begin"
		public void Init()
		{
		}

		override public void ProcessMessage(MessageForm message)
		{
			switch (message.Code)
			{
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

        private void StartWorkWithAssistant(MessageForm message, int assistantId, int commandCode) {
            MyMessage myMessage = (MyMessage)message.CreateCopy();

            if (myMessage.Worker == null) {
                return;
            }

            myMessage.Addressee = MyAgent.FindAssistant(assistantId);
            myMessage.Code = commandCode;

            StartContinualAssistant(myMessage);
        }

        private void FreeWorkplaceAndNotify(MyMessage message) {
            message.Code = Mc.DeassignWorkplace;
            message.Addressee = MySim.FindAgent(SimId.AgentCarpentry);
            Notice(message);
        }
    }
}