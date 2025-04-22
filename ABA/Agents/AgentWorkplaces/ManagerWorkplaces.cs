using AgentSimulation.Structures.Objects;
using OSPABA;
using Simulation;
namespace Agents.AgentWorkplaces {
    //meta! id="39"
    public class ManagerWorkplaces : OSPABA.Manager {
        public List<Workplace> Workplaces { get; set; }

        public ManagerWorkplaces(int id, OSPABA.Simulation mySim, Agent myAgent) : base(id, mySim, myAgent) {
            Init();
        }

        override public void PrepareReplication() {
            base.PrepareReplication();
            // Setup component for the next replication

            if (PetriNet != null) {
                PetriNet.Clear();
            }

            Workplaces = new();
        }

		//meta! sender="AgentCarpentry", id="115", type="Request"
		public void ProcessGetWorkerForCutting(MessageForm message) {
            var myMessage = (MyMessage)message.CreateCopy();

            if (myMessage.Worker == null) return;


        }

		//meta! userInfo="Removed from model"
        public void ProcessDoPickling(MessageForm message) {
        }

		//meta! sender="AgentCarpentry", id="119", type="Request"
		public void ProcessGetWorkerForMounting(MessageForm message) {
        }

		//meta! userInfo="Removed from model"
		public void ProcessAssignWorkplace(MessageForm message) {
        }

		//meta! userInfo="Removed from model"
        public void ProcessDoAssembling(MessageForm message) {
        }

		//meta! sender="AgentCarpentry", id="118", type="Request"
		public void ProcessGetWorkerForAssembling(MessageForm message) {
        }

		//meta! sender="AgentCarpentry", id="40", type="Notice"
		public void ProcessInit(MessageForm message) {
        }

		//meta! sender="AgentCarpentry", id="170", type="Response"
		public void ProcessGetFreeWorkplace(MessageForm message) {
        }

		//meta! sender="AgentCarpentry", id="117", type="Request"
		public void ProcessGetWorkerForPainting(MessageForm message) {
        }

		//meta! userInfo="Removed from model"
        public void ProcessDoMounting(MessageForm message) {
        }

		//meta! userInfo="Removed from model"
        public void ProcessDoCutting(MessageForm message) {
        }

		//meta! userInfo="Removed from model"
        public void ProcessDoPainting(MessageForm message) {
        }

		//meta! sender="Pickling", id="147", type="Finish"
		public void ProcessFinishPickling(MessageForm message) {
        }

		//meta! sender="Assembling", id="67", type="Finish"
		public void ProcessFinishAssembling(MessageForm message) {
        }

		//meta! sender="Painting", id="65", type="Finish"
		public void ProcessFinishPainting(MessageForm message) {
        }

		//meta! sender="Cutting", id="63", type="Finish"
		public void ProcessFinishCutting(MessageForm message) {
        }

		//meta! sender="Mounting", id="69", type="Finish"
		public void ProcessFinishMounting(MessageForm message) {
        }

		//meta! sender="AgentCarpentry", id="120", type="Request"
		public void ProcessGetWorkerForPickling(MessageForm message) {
        }

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message) {
            switch (message.Code) {
            }
        }

		//meta! userInfo="Removed from model"
		public void ProcessGetFreeWorkplaceAgentCarpentry(MessageForm message)
		{
		}

		//meta! sender="AgentCarpentry", id="73", type="Notice"
		public void ProcessDeassignWorkplace(MessageForm message)
		{
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		public void Init()
		{
		}

		override public void ProcessMessage(MessageForm message)
		{
			switch (message.Code)
			{
			case Mc.Finish:
				switch (message.Sender.Id)
				{
				case SimId.Mounting:
					ProcessFinishMounting(message);
				break;

				case SimId.Painting:
					ProcessFinishPainting(message);
				break;

				case SimId.Assembling:
					ProcessFinishAssembling(message);
				break;

				case SimId.Cutting:
					ProcessFinishCutting(message);
				break;

				case SimId.Pickling:
					ProcessFinishPickling(message);
				break;
				}
			break;

			case Mc.Init:
				ProcessInit(message);
			break;

			case Mc.GetWorkerForPickling:
				ProcessGetWorkerForPickling(message);
			break;

			case Mc.GetFreeWorkplace:
				ProcessGetFreeWorkplace(message);
			break;

			case Mc.GetWorkerForCutting:
				ProcessGetWorkerForCutting(message);
			break;

			case Mc.GetWorkerForAssembling:
				ProcessGetWorkerForAssembling(message);
			break;

			case Mc.GetWorkerForPainting:
				ProcessGetWorkerForPainting(message);
			break;

			case Mc.DeassignWorkplace:
				ProcessDeassignWorkplace(message);
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
        public new AgentWorkplaces MyAgent {
            get {
                return (AgentWorkplaces)base.MyAgent;
            }
        }
    }
}