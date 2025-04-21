using AgentSimulation.Structures;
using OSPABA;
using Simulation;
namespace Agents.AgentScope {
    //meta! id="3"
    public class ManagerScope : OSPABA.Manager {
        public ManagerScope(int id, OSPABA.Simulation mySim, Agent myAgent) : base(id, mySim, myAgent) {
            Init();
        }

        override public void PrepareReplication() {
            base.PrepareReplication();
            // Setup component for the next replication

            if (PetriNet != null) {
                PetriNet.Clear();
            }
        }

		//meta! sender="AgentModel", id="25", type="Notice"
		public void ProcessInit(MessageForm message) {
            message.Addressee = MyAgent.FindAssistant(SimId.OrderArrival);
            StartContinualAssistant(message);
        }

		//meta! sender="OrderArrival", id="164", type="Finish"
		public void ProcessFinish(MessageForm message) {
        }

		//meta! sender="AgentModel", id="10", type="Notice"
		public void ProcessOrderExit(MessageForm message) {
            var myMessage = (MyMessage)message.CreateCopy();

            if (myMessage.Order == null) return;

            myMessage.Order.EndTime = MySim.CurrentTime;

            ((MySimulation)MySim).FinishedOrdersCount++;
            ((MySimulation)MySim).AverageOrderTime.AddSample(myMessage.Order.EndTime - myMessage.Order.StartTime);

            if (MySim.CurrentTime >= Constants.END_OF_REPLICATION) {
                MySim.StopReplication();
            }
        }

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message) {
            switch (message.Code) {
            }
        }

		//meta! userInfo="Generated code: do not modify", tag="begin"
		public void Init()
		{
		}

		override public void ProcessMessage(MessageForm message)
		{
			switch (message.Code)
			{
			case Mc.Finish:
				ProcessFinish(message);
			break;

			case Mc.OrderExit:
				ProcessOrderExit(message);
			break;

			case Mc.Init:
				ProcessInit(message);
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