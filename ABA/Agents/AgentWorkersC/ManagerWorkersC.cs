using OSPABA;
using Simulation;
namespace Agents.AgentWorkersC
{
	//meta! id="150"
	public class ManagerWorkersC : OSPABA.Manager
	{
		public ManagerWorkersC(int id, OSPABA.Simulation mySim, Agent myAgent) :
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

		//meta! sender="AgentWorkers", id="156", type="Request"
		public void ProcessGetWorkerC(MessageForm message)
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
			case Mc.GetWorkerC:
				ProcessGetWorkerC(message);
			break;

			default:
				ProcessDefault(message);
			break;
			}
		}
		//meta! tag="end"
		public new AgentWorkersC MyAgent
		{
			get
			{
				return (AgentWorkersC)base.MyAgent;
			}
		}
	}
}