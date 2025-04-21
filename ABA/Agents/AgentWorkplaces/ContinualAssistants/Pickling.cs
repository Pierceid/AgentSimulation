using AgentSimulation.Generators;
using AgentSimulation.Structures.Enums;
using OSPABA;
using Simulation;
namespace Agents.AgentWorkplaces.ContinualAssistants {
    //meta! id="146"
    public class Pickling : OSPABA.Process {
        private RandomGenerators generators;

        public Pickling(int id, OSPABA.Simulation mySim, CommonAgent myAgent) : base(id, mySim, myAgent) {
            generators = new();
        }

        override public void PrepareReplication() {
            base.PrepareReplication();
            // Setup component for the next replication
        }

		//meta! sender="AgentWorkplaces", id="147", type="Start"
		public void ProcessStart(MessageForm message) {
            message.Code = Mc.DoCutting;

            var myMessage = (MyMessage)message;

            if (myMessage.Order == null) return;

            double picklingTime = myMessage.Order.Type switch {
                ProductType.Chair => generators.ChairPicklingTime.Next(),
                ProductType.Table => generators.TablePicklingTime.Next(),
                ProductType.Wardrobe => generators.WardrobePicklingTime.Next(),
                _ => 0
            };

            double startTime = MySim.CurrentTime;
            double endTime = startTime + picklingTime;

            Hold(picklingTime, message);
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