using OSPABA;
using Simulation;

namespace Agents.AgentMovement {
    //meta! id="43"
    public class ManagerMovement : OSPABA.Manager {
        public ManagerMovement(int id, OSPABA.Simulation mySim, Agent myAgent) : base(id, mySim, myAgent) {
            Init();
        }

        override public void PrepareReplication() {
            base.PrepareReplication();
            if (PetriNet != null) {
                PetriNet.Clear();
            }
        }

		//meta! sender="AgentCarpentry", id="46", type="Notice"
		public void ProcessInit(MessageForm message) {

        }

		//meta! sender="MovingToWorkplace", id="102", type="Finish"
		public void ProcessFinishMovingToWorkplace(MessageForm message) {
            // Reply back to AgentCarpentry that movement finished
            message.Code = Mc.MoveToWorkplace;
            Response(message);
        }

		//meta! sender="MovingToStorage", id="111", type="Finish"
		public void ProcessFinishMovingToStorage(MessageForm message) {
            message.Code = Mc.MoveToStorage;
            Response(message);
        }

		//meta! sender="AgentCarpentry", id="55", type="Request"
		public void ProcessMoveToWorkplace(MessageForm message) {
            message.Addressee = MySim.FindAgent(SimId.MovingToWorkplace);
            message.Code = Mc.Start;
            StartContinualAssistant(message);
        }

		//meta! sender="AgentCarpentry", id="112", type="Request"
		public void ProcessMoveToStorage(MessageForm message) {
            message.Addressee = MySim.FindAgent(SimId.MovingToStorage);
            message.Code = Mc.Start;
            StartContinualAssistant(message);
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
			case Mc.Init:
				ProcessInit(message);
			break;

			case Mc.MoveToWorkplace:
				ProcessMoveToWorkplace(message);
			break;

			case Mc.Finish:
				switch (message.Sender.Id)
				{
				case SimId.MovingToStorage:
					ProcessFinishMovingToStorage(message);
				break;

				case SimId.MovingToWorkplace:
					ProcessFinishMovingToWorkplace(message);
				break;
				}
			break;

			case Mc.MoveToStorage:
				ProcessMoveToStorage(message);
			break;

			default:
				ProcessDefault(message);
			break;
			}
		}
		//meta! tag="end"

        public new AgentMovement MyAgent {
            get { return (AgentMovement)base.MyAgent; }
        }
    }
}