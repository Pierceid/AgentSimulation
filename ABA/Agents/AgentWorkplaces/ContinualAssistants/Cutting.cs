using AgentSimulation.Structures.Enums;
using OSPABA;
using Simulation;
namespace Agents.AgentWorkplaces.ContinualAssistants {
    //meta! id="62"
    public class Cutting : OSPABA.Process {

        public Cutting(int id, OSPABA.Simulation mySim, CommonAgent myAgent) : base(id, mySim, myAgent) {
        }

        override public void PrepareReplication() {
            base.PrepareReplication();
        }

        //meta! sender="AgentWorkplaces", id="63", type="Start"
        public void ProcessStart(MessageForm message) {
            message.Code = SimId.Cutting;

            MyMessage myMessage = (MyMessage)message;
            MySimulation mySimulation = (MySimulation)MySim;

            if (myMessage.Product == null) return;

            double cuttingTime = myMessage.Product.Type switch {
                ProductType.Chair => mySimulation.Generators.ChairCuttingTime.Next(),
                ProductType.Table => mySimulation.Generators.TableCuttingTime.Next(),
                ProductType.Wardrobe => mySimulation.Generators.WardrobeCuttingTime.Next(),
                _ => 0
            };

            double startTime = MySim.CurrentTime;
            double endTime = startTime + cuttingTime;

            Hold(cuttingTime, message);
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