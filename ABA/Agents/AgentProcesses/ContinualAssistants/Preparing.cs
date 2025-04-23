using OSPABA;
using Simulation;
namespace Agents.AgentProcesses.ContinualAssistants {
    //meta! id="79"
    public class Preparing : OSPABA.Process {
        public Preparing(int id, OSPABA.Simulation mySim, CommonAgent myAgent) : base(id, mySim, myAgent) {
        }

        override public void PrepareReplication() {
            base.PrepareReplication();
        }

		//meta! sender="AgentProcesses", id="80", type="Start"
		public void ProcessStart(MessageForm message) {
            MyMessage myMessage = (MyMessage)message;
            MySimulation mySimulation = (MySimulation)MySim;

            if (myMessage.Product == null) return;

            double preparingTime = mySimulation.Generators.MaterialPreparationTime.Next();

            Hold(preparingTime, message);
        }

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message) {
            switch (message.Code) {
            }
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