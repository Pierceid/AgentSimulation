using AgentSimulation.Generators;
using AgentSimulation.Structures.Enums;
using OSPABA;
using Simulation;
namespace Agents.AgentWorkplaces.ContinualAssistants {
    //meta! id="64"
    public class Painting : OSPABA.Process {
        private RandomGenerators generators;

        public Painting(int id, OSPABA.Simulation mySim, CommonAgent myAgent) : base(id, mySim, myAgent) {
            generators = new();
        }

        override public void PrepareReplication() {
            base.PrepareReplication();
            // Setup component for the next replication
        }

		//meta! sender="AgentWorkplaces", id="65", type="Start"
		public void ProcessStart(MessageForm message) {
            message.Code = Mc.DoCutting;

            var myMessage = (MyMessage)message;

            if (myMessage.Order == null) return;

            double paintingTime = myMessage.Order.Type switch {
                ProductType.Chair => generators.ChairPaintingTime.Next(),
                ProductType.Table => generators.TablePaintingTime.Next(),
                ProductType.Wardrobe => generators.WardrobePaintingTime.Next(),
                _ => 0
            };

            double startTime = MySim.CurrentTime;
            double endTime = startTime + paintingTime;

            Hold(paintingTime, message);
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