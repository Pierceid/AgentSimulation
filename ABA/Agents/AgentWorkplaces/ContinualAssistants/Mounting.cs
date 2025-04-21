using AgentSimulation.Generators;
using OSPABA;
using Simulation;
namespace Agents.AgentWorkplaces.ContinualAssistants {
    //meta! id="68"
    public class Mounting : OSPABA.Process {
        private RandomGenerators generators;

        public Mounting(int id, OSPABA.Simulation mySim, CommonAgent myAgent) : base(id, mySim, myAgent) {
            generators = new();
        }

        override public void PrepareReplication() {
            base.PrepareReplication();
            // Setup component for the next replication
        }

		//meta! sender="AgentWorkplaces", id="69", type="Start"
		public void ProcessStart(MessageForm message) {
            message.Code = Mc.DoCutting;

            var myMessage = (MyMessage)message;

            if (myMessage.Order == null) return;

            double mountingTime = generators.WardrobeMountingTime.Next();

            double startTime = MySim.CurrentTime;
            double endTime = startTime + mountingTime;

            Hold(mountingTime, message);
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
        public new AgentWorkplaces MyAgent {
            get {
                return (AgentWorkplaces)base.MyAgent;
            }
        }
    }
}