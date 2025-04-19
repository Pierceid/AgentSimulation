using OSPABA;
using Simulation;
namespace Agents.AgentWorkersB
{
	//meta! id="151"
	public class ManagerWorkersB : OSPABA.Manager
	{
		public ManagerWorkersB(int id, OSPABA.Simulation mySim, Agent myAgent) :
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

		//meta! sender="AgentWorkers", id="157", type="Request"
		public void ProcessGetWorkerB(MessageForm message)
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
			case Mc.GetWorkerB:
				ProcessGetWorkerB(message);
			break;

			default:
				ProcessDefault(message);
			break;
			}
		}
		//meta! tag="end"
		public new AgentWorkersB MyAgent
		{
			get
			{
				return (AgentWorkersB)base.MyAgent;
			}
		}
	}
}