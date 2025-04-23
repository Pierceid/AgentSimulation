using AgentSimulation.Structures.Enums;
using OSPABA;
using Simulation;
namespace Agents.AgentProcesses.ContinualAssistants {
    //meta! id="224"
    public class Painting : OSPABA.Process {
        public Painting(int id, OSPABA.Simulation mySim, CommonAgent myAgent) : base(id, mySim, myAgent) {
        }

        override public void PrepareReplication() {
            base.PrepareReplication();
        }

		//meta! sender="AgentProcesses", id="225", type="Start"
		public void ProcessStart(MessageForm message) {
            MyMessage myMessage = (MyMessage)message;
            MySimulation mySimulation = (MySimulation)MySim;

            if (myMessage.Product == null) return;

            double paintingTime = myMessage.Product.Type switch {
                ProductType.Chair => mySimulation.Generators.ChairPaintingTime.Next(),
                ProductType.Table => mySimulation.Generators.TablePaintingTime.Next(),
                ProductType.Wardrobe => mySimulation.Generators.WardrobePaintingTime.Next(),
                _ => 0
            };

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
        public new AgentProcesses MyAgent {
            get {
                return (AgentProcesses)base.MyAgent;
            }
        }
    }
}