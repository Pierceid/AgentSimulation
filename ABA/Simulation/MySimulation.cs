using Agents.AgentCarpentry;
using Agents.AgentModel;
using Agents.AgentMovement;
using Agents.AgentProcesses;
using Agents.AgentScope;
using Agents.AgentWorkers;
using Agents.AgentWorkersA;
using Agents.AgentWorkersB;
using Agents.AgentWorkersC;
using Agents.AgentWorkplaces;
using AgentSimulation.Generators;
using OSPStat;

namespace Simulation {
    public class MySimulation : OSPABA.Simulation {
        public Stat FinishedOrdersCount { get; set; } = new();
        public Stat PendingOrdersCount { get; set; } = new();
        public Stat AverageOrderTime { get; set; } = new();
        public RandomGenerators Generators { get; set; } = new();
        public double Speed { get; set; } = 1.0;

        public MySimulation() {
            Init();
        }

        override public void PrepareSimulation() {
            base.PrepareSimulation();

            Clear();

            FinishedOrdersCount.Clear();
            PendingOrdersCount.Clear();
            AverageOrderTime.Clear();
        }

        override public void PrepareReplication() {
            base.PrepareReplication();

            Clear();
        }

        override public void ReplicationFinished() {
            base.ReplicationFinished();

            OnRefreshUI(sim => Delegates.ForEach(d => d.Refresh(sim)));
        }

        override public void SimulationFinished() {
            base.SimulationFinished();
        }

        public void Clear() {
            var managerScope = AgentScope.MyManager as ManagerScope;
            var managerWorkersA = AgentWorkersA.MyManager as ManagerWorkersA;
            var managerWorkersB = AgentWorkersB.MyManager as ManagerWorkersB;
            var managerWorkersC = AgentWorkersC.MyManager as ManagerWorkersC;
            var managerCarpentry = AgentCarpentry.MyManager as ManagerCarpentry;

            managerScope?.Clear();
            managerWorkersA?.Clear();
            managerWorkersB?.Clear();
            managerWorkersC?.Clear();
            managerCarpentry?.Clear();

            AverageOrderTime.Clear();
        }

        public void InitWorkers(int workersA, int workersB, int workersC) {
            var managerWorkersA = AgentWorkersA.MyManager as ManagerWorkersA;
            var managerWorkersB = AgentWorkersB.MyManager as ManagerWorkersB;
            var managerWorkersC = AgentWorkersC.MyManager as ManagerWorkersC;

            managerWorkersA?.InitWorkers(workersA);
            managerWorkersB?.InitWorkers(workersB);
            managerWorkersC?.InitWorkers(workersC);
        }

        public void InitWorkplaces(int workplaces) {
            var managerCarpentry = AgentCarpentry.MyManager as ManagerCarpentry;

            managerCarpentry?.InitWorkplaces(workplaces);
        }

        public void InitSpeed(double speed) {
            Speed = speed;

            if (Speed > 0) {
                SetSimSpeed(Speed, 0.1);
            } else {
                SetMaxSimSpeed();
            }
        }

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			AgentModel = new AgentModel(SimId.AgentModel, this, null);
			AgentScope = new AgentScope(SimId.AgentScope, this, AgentModel);
			AgentCarpentry = new AgentCarpentry(SimId.AgentCarpentry, this, AgentModel);
			AgentWorkplaces = new AgentWorkplaces(SimId.AgentWorkplaces, this, AgentCarpentry);
			AgentMovement = new AgentMovement(SimId.AgentMovement, this, AgentCarpentry);
			AgentProcesses = new AgentProcesses(SimId.AgentProcesses, this, AgentCarpentry);
			AgentWorkers = new AgentWorkers(SimId.AgentWorkers, this, AgentCarpentry);
			AgentWorkersA = new AgentWorkersA(SimId.AgentWorkersA, this, AgentWorkers);
			AgentWorkersC = new AgentWorkersC(SimId.AgentWorkersC, this, AgentWorkers);
			AgentWorkersB = new AgentWorkersB(SimId.AgentWorkersB, this, AgentWorkers);
		}
		public AgentModel AgentModel
		{ get; set; }
		public AgentScope AgentScope
		{ get; set; }
		public AgentCarpentry AgentCarpentry
		{ get; set; }
		public AgentWorkplaces AgentWorkplaces
		{ get; set; }
		public AgentMovement AgentMovement
		{ get; set; }
		public AgentProcesses AgentProcesses
		{ get; set; }
		public AgentWorkers AgentWorkers
		{ get; set; }
		public AgentWorkersA AgentWorkersA
		{ get; set; }
		public AgentWorkersC AgentWorkersC
		{ get; set; }
		public AgentWorkersB AgentWorkersB
		{ get; set; }
		//meta! tag="end"
    }
}