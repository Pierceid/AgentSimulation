using OSPABA;
using Simulation;

namespace Agents.AgentWorkersA
{
	//meta! id="149"
	public class AgentWorkersA : OSPABA.Agent
	{
		public AgentWorkersA(int id, OSPABA.Simulation mySim, Agent parent) :
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
			new ManagerWorkersA(SimId.ManagerWorkersA, MySim, this);
			AddOwnMessage(Mc.GetWorkerA);
		}
		//meta! tag="end"
	}
}