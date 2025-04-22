using OSPABA;
using Simulation;
namespace Agents.AgentWarehouse {
    //meta! id="77"
    public class ManagerWarehouse : OSPABA.Manager {
        public ManagerWarehouse(int id, OSPABA.Simulation mySim, Agent myAgent) : base(id, mySim, myAgent) {
            Init();
        }

        override public void PrepareReplication() {
            base.PrepareReplication();

            if (PetriNet != null) {
                PetriNet.Clear();
            }
        }

		//meta! sender="AgentCarpentry", id="84", type="Notice"
		public void ProcessInit(MessageForm message) {
        }

		//meta! sender="AgentCarpentry", id="140", type="Request"
		public void ProcessDoPreparing(MessageForm message) {
        }

		//meta! sender="Preparing", id="80", type="Finish"
		public void ProcessFinish(MessageForm message) {
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

			case Mc.Init:
				ProcessInit(message);
			break;

			case Mc.DoPreparing:
				ProcessDoPreparing(message);
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