using OSPABA;
using Simulation;

namespace Agents.AgentWorkersC
{
	//meta! id="150"
	public class AgentWorkersC : OSPABA.Agent
	{
		public AgentWorkersC(int id, OSPABA.Simulation mySim, Agent parent) :
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
			new ManagerWorkersC(SimId.ManagerWorkersC, MySim, this);
			AddOwnMessage(Mc.GetWorkerC);
		}
		//meta! tag="end"
	}
}