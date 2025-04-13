using OSPABA;
using Simulation;
namespace Agents.CarpentryAgent
{
	//meta! id="4"
	public class CarpentryManager : OSPABA.Manager
	{
		public CarpentryManager(int id, OSPABA.Simulation mySim, Agent myAgent) :
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

		//meta! sender="ModelAgent", id="12", type="Request"
		public void ProcessProcessOrder(MessageForm message)
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
			case Mc.ProcessOrder:
				ProcessProcessOrder(message);
			break;

			default:
				ProcessDefault(message);
			break;
			}
		}
		//meta! tag="end"
		public new CarpentryAgent MyAgent
		{
			get
			{
				return (CarpentryAgent)base.MyAgent;
			}
		}
	}
}
