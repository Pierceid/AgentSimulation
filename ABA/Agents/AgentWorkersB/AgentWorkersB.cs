using OSPABA;
using Simulation;

namespace Agents.AgentWorkersB
{
	//meta! id="151"
	public class AgentWorkersB : OSPABA.Agent
	{
		public AgentWorkersB(int id, OSPABA.Simulation mySim, Agent parent) :
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
			new ManagerWorkersB(SimId.ManagerWorkersB, MySim, this);
			AddOwnMessage(Mc.GetWorkerB);
		}
		//meta! tag="end"
	}
}