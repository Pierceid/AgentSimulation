using OSPABA;
using Agents.ScopeAgent;
using Agents.ModelAgent;
using Agents.CarpentryAgent;

namespace Simulation
{
	public class MySimulation : OSPABA.Simulation
	{
		public MySimulation()
		{
			Init();
		}

		override public void PrepareSimulation()
		{
			base.PrepareSimulation();
			// Create global statistcis
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Reset entities, queues, local statistics, etc...
		}

		override public void ReplicationFinished()
		{
			// Collect local statistics into global, update UI, etc...
			base.ReplicationFinished();
		}

		override public void SimulationFinished()
		{
			// Display simulation results
			base.SimulationFinished();
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			ModelAgent = new ModelAgent(SimId.ModelAgent, this, null);
			ScopeAgent = new ScopeAgent(SimId.ScopeAgent, this, ModelAgent);
			CarpentryAgent = new CarpentryAgent(SimId.CarpentryAgent, this, ModelAgent);
		}
		public ModelAgent ModelAgent
		{ get; set; }
		public ScopeAgent ScopeAgent
		{ get; set; }
		public CarpentryAgent CarpentryAgent
		{ get; set; }
		//meta! tag="end"
	}
}