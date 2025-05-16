using OSPABA;
using Simulation;
namespace Agents.AgentProcesses.ContinualAssistants {
    //meta! id="277"
    public class Drying : OSPABA.Process {
        public Drying(int id, OSPABA.Simulation mySim, CommonAgent myAgent) :
            base(id, mySim, myAgent) {
        }

        override public void PrepareReplication() {
            base.PrepareReplication();
            // Setup component for the next replication
        }

        //meta! sender="AgentProcesses", id="278", type="Start"
        public void ProcessStart(MessageForm message) {
            MyMessage myMessage = (MyMessage)message;
            MySimulation mySimulation = (MySimulation)MySim;
            myMessage.Code = Mc.Finish;

            if (myMessage.Product == null) return;

            double dryingTime = mySimulation.Generators.DryingTime.Next();

            Hold(dryingTime, myMessage);
        }

        //meta! userInfo="Process messages defined in code", id="0"
        public void ProcessDefault(MessageForm message) {
            AssistantFinished(message);
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
        public new AgentProcesses MyAgent {
            get {
                return (AgentProcesses)base.MyAgent;
            }
        }
    }
}
