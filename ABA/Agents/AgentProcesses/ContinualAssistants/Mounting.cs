using OSPABA;
using Simulation;
namespace Agents.AgentProcesses.ContinualAssistants {
    //meta! id="226"
    public class Mounting : OSPABA.Process {
        public Mounting(int id, OSPABA.Simulation mySim, CommonAgent myAgent) : base(id, mySim, myAgent) {
        }

        override public void PrepareReplication() {
            base.PrepareReplication();
        }

		//meta! sender="AgentProcesses", id="227", type="Start"
		public void ProcessStart(MessageForm message) {
            MyMessage myMessage = (MyMessage)message;
            MySimulation mySimulation = (MySimulation)MySim;
            myMessage.Code = Mc.Finish;

            if (myMessage.Product == null) return;

            double mountingTime = mySimulation.Generators.WardrobeMountingTime.Next();

            Hold(mountingTime, myMessage);
        }

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message) {
            AssistantFinished(message);
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
        public new AgentProcesses MyAgent {
            get {
                return (AgentProcesses)base.MyAgent;
            }
        }
    }
}