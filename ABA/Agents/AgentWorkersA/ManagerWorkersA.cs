using OSPABA;
using Simulation;
namespace Agents.AgentWorkersA
{
	//meta! id="149"
	public class ManagerWorkersA : OSPABA.Manager
	{
		public ManagerWorkersA(int id, OSPABA.Simulation mySim, Agent myAgent) :
			base(id, mySim, myAgent)
		{
			Init();
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication

			if (PetriNet != null)
			{
				PetriNet.Clear();
			}
		}

		//meta! sender="AgentWorkers", id="159", type="Request"
		public void ProcessGetWorkerA(MessageForm message)
		{
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
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
			case Mc.GetWorkerA:
				ProcessGetWorkerA(message);
			break;

			default:
				ProcessDefault(message);
			break;
			}
		}
		//meta! tag="end"
		public new AgentWorkersA MyAgent
		{
			get
			{
				return (AgentWorkersA)base.MyAgent;
			}
		}
	}
}