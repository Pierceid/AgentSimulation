using OSPABA;
using Simulation;
using Agents.AgentWorkers;
namespace Agents.AgentWorkers.InstantAssistants
{
	//meta! id="103"
	public class ChangeWorkerState : OSPABA.Action
	{
		public ChangeWorkerState(int id, OSPABA.Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
		}

		override public void Execute(MessageForm message)
		{
		}
		public new AgentWorkers MyAgent
		{
			get
			{
				return (AgentWorkers)base.MyAgent;
			}
		}
	}
}