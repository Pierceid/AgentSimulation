using OSPABA;
using Simulation;
namespace Agents.CarpentryAgent
{
	//meta! id="4"
	public class CarpentryAgent : OSPABA.Agent
	{
		public CarpentryAgent(int id, OSPABA.Simulation mySim, Agent parent) :
			base(id, mySim, parent)
		{
			Init();
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			new CarpentryManager(SimId.CarpentryManager, MySim, this);
			AddOwnMessage(Mc.ProcessOrder);
		}
		//meta! tag="end"
	}
}
