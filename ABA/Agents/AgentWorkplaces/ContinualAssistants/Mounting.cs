using AgentSimulation.Structures.Enums;
using OSPABA;
using Simulation;

namespace Agents.AgentWorkplaces.ContinualAssistants {
    //meta! id="68"
    public class Mounting : OSPABA.Process {
        public Mounting(int id, OSPABA.Simulation mySim, CommonAgent myAgent) : base(id, mySim, myAgent) {
        }

        override public void PrepareReplication() {
            base.PrepareReplication();
        }

        //meta! sender="AgentWorkplaces", id="69", type="Start"
        public void ProcessStart(MessageForm message) {
            message.Code = SimId.Mounting;

            MyMessage myMessage = (MyMessage)message;
            MySimulation mySimulation = (MySimulation)MySim;

            if (myMessage.Product == null) return;

            double mountingTime = mySimulation.Generators.WardrobeMountingTime.Next();
            Hold(mountingTime, message);
        }

        public void ProcessFinish(MessageForm message) {
            MyMessage myMessage = (MyMessage)message;

            myMessage.Code = Mc.Finish;
            myMessage.Addressee = MySim.FindAgent(SimId.AgentWorkplaces);
            AssistantFinished(myMessage);
        }

        //meta! userInfo="Process messages defined in code", id="0"
        public void ProcessDefault(MessageForm message) { }

        //meta! userInfo="Generated code: do not modify", tag="begin"
        override public void ProcessMessage(MessageForm message) {
            switch (message.Code) {
                case Mc.Start:
                    ProcessStart(message);
                    break;
                case Mc.Finish:
                    ProcessFinish(message);
                    break;
                default:
                    ProcessDefault(message);
                    break;
            }
        }
        //meta! tag="end"

        public new AgentWorkplaces MyAgent => (AgentWorkplaces)base.MyAgent;
    }
}
