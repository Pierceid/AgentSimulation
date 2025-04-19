using AgentSimulation.Generators;
using AgentSimulation.Structures.Enums;
using OSPABA;
using Simulation;
namespace Agents.AgentWorkplaces.ContinualAssistants {
    //meta! id="66"
    public class Assembling : OSPABA.Process {
        private RandomGenerators generators;

        public Assembling(int id, OSPABA.Simulation mySim, CommonAgent myAgent) : base(id, mySim, myAgent) {
            generators = new();
        }

        override public void PrepareReplication() {
            base.PrepareReplication();
            // Setup component for the next replication
        }

        //meta! sender="AgentWorkplaces", id="67", type="Start"
        public void ProcessStart(MessageForm message) {
            message.Code = Mc.DoCutting;

            var myMessage = (MyMessage)message;

            if (myMessage.Order == null) return;

            double assemblingTime = myMessage.Order.Type switch {
                ProductType.Chair => generators.ChairAssemblyTime.Next(),
                ProductType.Table => generators.TableAssemblyTime.Next(),
                ProductType.Wardrobe => generators.WardrobeAssemblyTime.Next(),
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
        public new AgentWorkplaces MyAgent {
            get {
                return (AgentWorkplaces)base.MyAgent;
            }
        }
    }
}