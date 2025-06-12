using AgentSimulation.Structures.Enums;
using OSPABA;
using Simulation;
namespace Agents.AgentProcesses.ContinualAssistants {
    //meta! id="230"
    public class Pickling : OSPABA.Process {
        public Pickling(int id, OSPABA.Simulation mySim, CommonAgent myAgent) : base(id, mySim, myAgent) {
        }

        override public void PrepareReplication() {
            base.PrepareReplication();
        }

		//meta! sender="AgentProcesses", id="231", type="Start"
		public void ProcessStart(MessageForm message) {
            MyMessage myMessage = (MyMessage)message;
            MySimulation mySimulation = (MySimulation)MySim;
            myMessage.Code = Mc.Finish;

            if (myMessage.Product == null) return;

            double picklingTime = myMessage.Product.Type switch {
                ProductType.Chair => mySimulation.Generators.ChairPicklingTime.Next(),
                ProductType.Table => mySimulation.Generators.TablePicklingTime.Next(),
                ProductType.Wardrobe => mySimulation.Generators.WardrobePicklingTime.Next(),
                _ => 0
            };

            Hold(picklingTime, myMessage);
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