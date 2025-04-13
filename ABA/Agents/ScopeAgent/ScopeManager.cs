using OSPABA;
using Simulation;
namespace Agents.ScopeAgent
{
	//meta! id="3"
	public class ScopeManager : OSPABA.Manager
	{
		public ScopeManager(int id, OSPABA.Simulation mySim, Agent myAgent) :
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

		//meta! sender="ModelAgent", id="10", type="Notice"
		public void ProcessEnterAndExit(MessageForm message)
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
			case Mc.EnterAndExit:
				ProcessEnterAndExit(message);
			break;

			default:
				ProcessDefault(message);
			break;
			}
		}
		//meta! tag="end"
		public new ScopeAgent MyAgent
		{
			get
			{
				return (ScopeAgent)base.MyAgent;
			}
		}
	}
}
