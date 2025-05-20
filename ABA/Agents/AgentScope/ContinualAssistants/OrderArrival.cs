using AgentSimulation.Structures;
using OSPABA;
using Simulation;
namespace Agents.AgentScope.ContinualAssistants {
    //meta! id="163"
    public class OrderArrival : OSPABA.Scheduler {
        public OrderArrival(int id, OSPABA.Simulation mySim, CommonAgent myAgent) : base(id, mySim, myAgent) {
        }

        override public void PrepareReplication() {
            base.PrepareReplication();
        }

		//meta! sender="AgentScope", id="164", type="Start"
		public void ProcessStart(MessageForm message) {
            MyMessage myMessage = (MyMessage)message.CreateCopy();
            MySimulation mySimulation = (MySimulation)MySim;

            myMessage.Code = Mc.PlanOrderArrival;
            double time = mySimulation.Generators.OrderArrivalTime.Next();

            if (MySim.CurrentTime + time < Constants.SIMULATION_TIME) {
                Hold(time, myMessage);
            }
        }

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message) {
            switch (message.Code) {
            }
        }

		//meta! sender="AgentScope", id="186", type="Notice"
		public void ProcessPlanOrderArrival(MessageForm message) {
            MyMessage myMessage = (MyMessage)message.CreateCopy();
            myMessage.Addressee = MyAgent;
            Notice(myMessage);
        }

		//meta! userInfo="Generated code: do not modify", tag="begin"
		override public void ProcessMessage(MessageForm message)
		{
			switch (message.Code)
			{
			case Mc.Start:
				ProcessStart(message);
			break;

			case Mc.PlanOrderArrival:
				ProcessPlanOrderArrival(message);
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