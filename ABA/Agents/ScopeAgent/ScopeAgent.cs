using OSPABA;
using Simulation;
namespace Agents.ScopeAgent
{
	//meta! id="3"
	public class ScopeAgent : OSPABA.Agent
	{
		public ScopeAgent(int id, OSPABA.Simulation mySim, Agent parent) :
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
			new ScopeManager(SimId.ScopeManager, MySim, this);
			AddOwnMessage(Mc.EnterAndExit);
		}
		//meta! tag="end"
	}
}
