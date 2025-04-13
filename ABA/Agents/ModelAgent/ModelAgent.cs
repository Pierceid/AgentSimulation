using OSPABA;
using Simulation;
namespace Agents.ModelAgent
{
	//meta! id="2"
	public class ModelAgent : OSPABA.Agent
	{
		public ModelAgent(int id, OSPABA.Simulation mySim, Agent parent) :
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
			new ModelManager(SimId.ModelManager, MySim, this);
			AddOwnMessage(Mc.ProcessOrder);
		}
		//meta! tag="end"
	}
}
