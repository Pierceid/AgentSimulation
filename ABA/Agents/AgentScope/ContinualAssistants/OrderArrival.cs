using AgentSimulation.ABA.Simulation;
using OSPABA;
using Simulation;
namespace Agents.AgentScope.ContinualAssistants {
    //meta! id="163"
    public class OrderArrival : OSPABA.Scheduler {
        private static int orderId = 0;

        public OrderArrival(int id, OSPABA.Simulation mySim, CommonAgent myAgent) : base(id, mySim, myAgent) {
            MyAgent.AddOwnMessage(Mc.OrderEnter);
        }

        override public void PrepareReplication() {
            base.PrepareReplication();
            // Setup component for the next replication
        }

        //meta! sender="AgentScope", id="164", type="Start"
        public void ProcessStart(MessageForm message) {
            message.Code = Mc.OrderEnter;
            double time = ((CarpentrySimulation)MySim).Generators.OrderArrivalTime.Next();

            Hold(time, message);
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
        public new AgentScope MyAgent {
            get {
                return (AgentScope)base.MyAgent;
            }
        }
    }
}