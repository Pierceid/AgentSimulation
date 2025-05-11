using AgentSimulation.Structures.Enums;
using OSPABA;
using Simulation;
using System.Windows;

namespace Agents.AgentProcesses.ContinualAssistants {
    //meta! id="222"
    public class Cutting : OSPABA.Process {
        public Cutting(int id, OSPABA.Simulation mySim, CommonAgent myAgent) : base(id, mySim, myAgent) {
        }

        override public void PrepareReplication() {
            base.PrepareReplication();
        }

		//meta! sender="AgentProcesses", id="223", type="Start"
		public void ProcessStart(MessageForm message) {
            MessageBox.Show("cutting");
            MyMessage myMessage = (MyMessage)message;
            MySimulation mySimulation = (MySimulation)MySim;
            myMessage.Code = Mc.Finish;

            if (myMessage.Product == null) return;

            double cuttingTime = myMessage.Product.Type switch {
                ProductType.Chair => mySimulation.Generators.ChairCuttingTime.Next(),
                ProductType.Table => mySimulation.Generators.TableCuttingTime.Next(),
                ProductType.Wardrobe => mySimulation.Generators.WardrobeCuttingTime.Next(),
                _ => 0
            };

            Hold(cuttingTime, myMessage);
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