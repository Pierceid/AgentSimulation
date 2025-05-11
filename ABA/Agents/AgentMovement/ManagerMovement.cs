using OSPABA;
using Simulation;
using System.Windows;

namespace Agents.AgentMovement {
    //meta! id="43"
    public class ManagerMovement : OSPABA.Manager {
        public ManagerMovement(int id, OSPABA.Simulation mySim, Agent myAgent) : base(id, mySim, myAgent) {
            Init();
        }

        override public void PrepareReplication() {
            base.PrepareReplication();
            PetriNet?.Clear();
        }

        //meta! userInfo="Removed from model"
        public void ProcessInit(MessageForm message) {

        }

		//meta! sender="MovingToWorkplace", id="102", type="Finish"
		public void ProcessFinishMovingToWorkplace(MessageForm message) {
            message.Code = Mc.MoveToWorkplace;
            message.Addressee = MySim.FindAgent(SimId.AgentCarpentry);
            Response(message);
        }

		//meta! sender="MovingToStorage", id="111", type="Finish"
		public void ProcessFinishMovingToStorage(MessageForm message) {
            message.Code = Mc.MoveToStorage;
            message.Addressee = MySim.FindAgent(SimId.AgentCarpentry);
            Response(message);
        }

		//meta! sender="AgentCarpentry", id="55", type="Request"
		public void ProcessMoveToWorkplace(MessageForm message) {
            message.Addressee = MyAgent.FindAssistant(SimId.MovingToWorkplace);
            message.Code = Mc.Start;
            StartContinualAssistant(message);
        }

		//meta! sender="AgentCarpentry", id="112", type="Request"
		public void ProcessMoveToStorage(MessageForm message) {
            message.Addressee = MyAgent.FindAssistant(SimId.MovingToStorage);
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
			case Mc.MoveToStorage:
				ProcessMoveToStorage(message);
			break;

			case Mc.Finish:
				switch (message.Sender.Id)
				{
				case SimId.MovingToWorkplace:
					ProcessFinishMovingToWorkplace(message);
				break;

				case SimId.MovingToStorage:
					ProcessFinishMovingToStorage(message);
				break;
				}
			break;

			case Mc.MoveToWorkplace:
				ProcessMoveToWorkplace(message);
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