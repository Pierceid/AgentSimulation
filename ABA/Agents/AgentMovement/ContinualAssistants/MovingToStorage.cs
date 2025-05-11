using OSPABA;
using Simulation;

namespace Agents.AgentMovement.ContinualAssistants {
    //meta! id="110"
    public class MovingToStorage : OSPABA.Process {
        public MovingToStorage(int id, OSPABA.Simulation mySim, CommonAgent myAgent) : base(id, mySim, myAgent) {
        }

        override public void PrepareReplication() {
            base.PrepareReplication();
        }

		//meta! sender="AgentMovement", id="111", type="Start"
		public void ProcessStart(MessageForm message) {
            message.Code = Mc.Finish;
            double duration = ((MySimulation)MySim).Generators.WorkerMoveToStorageTime.Next();
            Hold(duration, message);
        }

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message) {
			var myMessage = (MyMessage)message;
			myMessage.Workplace = myMessage.Product?.Workplace;
            AssistantFinished(myMessage);
        }

		//meta! userInfo="Generated code: do not modify", tag="begin"
		override public void ProcessMessage(MessageForm message)
		{
			switch (message.Code)
			{
			case Mc.Start:
				ProcessStart(message);
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