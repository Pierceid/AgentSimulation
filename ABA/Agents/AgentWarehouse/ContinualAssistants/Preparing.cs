using AgentSimulation.Structures.Enums;
using OSPABA;
using Simulation;
namespace Agents.AgentWarehouse.ContinualAssistants {
    //meta! id="79"
    public class Preparing : OSPABA.Process {
        public Preparing(int id, OSPABA.Simulation mySim, CommonAgent myAgent) : base(id, mySim, myAgent) {
        }

        override public void PrepareReplication() {
            base.PrepareReplication();
        }

        //meta! sender="AgentWarehouse", id="80", type="Start"
        public void ProcessStart(MessageForm message) {
            message.Code = SimId.Assembling;

            MyMessage myMessage = (MyMessage)message;
            MySimulation mySimulation = (MySimulation)MySim;

            if (myMessage.Product == null) return;

            double assemblingTime = myMessage.Product.Type switch {
                ProductType.Chair => mySimulation.Generators.ChairAssemblyTime.Next(),
                ProductType.Table => mySimulation.Generators.TableAssemblyTime.Next(),
                ProductType.Wardrobe => mySimulation.Generators.WardrobeAssemblyTime.Next(),
                _ => 0
            };

            double startTime = MySim.CurrentTime;
            double endTime = startTime + assemblingTime;

            Hold(assemblingTime, message);
        }

        //meta! userInfo="Process messages defined in code", id="0"
        public void ProcessDefault(MessageForm message) {
            switch (message.Code) {
            }
        }

        //meta! userInfo="Generated code: do not modify", tag="begin"
        override public void ProcessMessage(MessageForm message) {
            switch (message.Code) {
                case Mc.Start:
                    ProcessStart(message);
                    break;

                default:
                    ProcessDefault(message);
                    break;
            }
        }
        //meta! tag="end"
        public new AgentWarehouse MyAgent {
            get {
                return (AgentWarehouse)base.MyAgent;
            }
        }
    }
}