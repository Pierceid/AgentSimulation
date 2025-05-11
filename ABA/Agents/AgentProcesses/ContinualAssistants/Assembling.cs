using AgentSimulation.Structures.Enums;
using OSPABA;
using Simulation;

namespace Agents.AgentProcesses.ContinualAssistants {
    //meta! id="228"
    public class Assembling : OSPABA.Process {
        public Assembling(int id, OSPABA.Simulation mySim, CommonAgent myAgent) :
            base(id, mySim, myAgent) {
        }

        override public void PrepareReplication() {
            base.PrepareReplication();
        }

		//meta! sender="AgentProcesses", id="229", type="Start"
		public void ProcessStart(MessageForm message) {
            MyMessage myMessage = (MyMessage)message;
            MySimulation mySimulation = (MySimulation)MySim;
            myMessage.Code = Mc.Finish;

            if (myMessage.Product == null) return;

            double assemblingTime = myMessage.Product.Type switch {
                ProductType.Chair => mySimulation.Generators.ChairAssemblyTime.Next(),
                ProductType.Table => mySimulation.Generators.TableAssemblyTime.Next(),
                ProductType.Wardrobe => mySimulation.Generators.WardrobeAssemblyTime.Next(),
                _ => 0
            };

            Hold(assemblingTime, myMessage);
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