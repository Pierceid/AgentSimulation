using Agents.AgentCarpentry;
using Agents.AgentModel;
using Agents.AgentMovement;
using Agents.AgentScope;
using Agents.AgentWarehouse;
using Agents.AgentWorkers;
using Agents.AgentWorkersA;
using Agents.AgentWorkersB;
using Agents.AgentWorkersC;
using Agents.AgentWorkplaces;
using AgentSimulation.Generators;
using AgentSimulation.Statistics;

namespace Simulation {
    public class MySimulation : OSPABA.Simulation {
        public int OrdersCount { get; set; }
        public int FinishedOrdersCount { get; set; }
        public int PendingOrdersCount { get; set; }
        public Average AverageOrderTime { get; set; }
        public RandomGenerators Generators { get; set; }
        public int WorkersACount { get; set; }
        public int WorkersBCount { get; set; }
        public int WorkersCCount { get; set; }
        public int WorkplacesCount { get; set; }

        public MySimulation() {
            Init();
        }

        override public void PrepareSimulation() {
            base.PrepareSimulation();
            // Create global statistcis
        }

        override public void PrepareReplication() {
            base.PrepareReplication();
            // Reset entities, queues, local statistics, etc...
        }

        override public void ReplicationFinished() {
            // Collect local statistics into global, update UI, etc...
            base.ReplicationFinished();
        }

        override public void SimulationFinished() {
            // Display simulation results
            base.SimulationFinished();
        }

        //meta! userInfo="Generated code: do not modify", tag="begin"
        private void Init() {
            OrdersCount = 0;
            FinishedOrdersCount = 0;
            PendingOrdersCount = 0;
            AverageOrderTime = new();
            Generators = new();

            WorkersACount = 0;
            WorkersBCount = 0;
            WorkersCCount = 0;
            WorkplacesCount = 0;

            AgentModel = new AgentModel(SimId.AgentModel, this, null);
            AgentScope = new AgentScope(SimId.AgentScope, this, AgentModel);
            AgentCarpentry = new AgentCarpentry(SimId.AgentCarpentry, this, AgentModel);
            AgentWorkplaces = new AgentWorkplaces(SimId.AgentWorkplaces, this, AgentCarpentry);
            AgentMovement = new AgentMovement(SimId.AgentMovement, this, AgentCarpentry);
            AgentWarehouse = new AgentWarehouse(SimId.AgentWarehouse, this, AgentCarpentry);
            AgentWorkers = new AgentWorkers(SimId.AgentWorkers, this, AgentCarpentry);
            AgentWorkersA = new AgentWorkersA(SimId.AgentWorkersA, this, AgentWorkers);
            AgentWorkersC = new AgentWorkersC(SimId.AgentWorkersC, this, AgentWorkers);
            AgentWorkersB = new AgentWorkersB(SimId.AgentWorkersB, this, AgentWorkers);
        }
        public AgentModel AgentModel { get; set; }
        public AgentScope AgentScope { get; set; }
        public AgentCarpentry AgentCarpentry { get; set; }
        public AgentWorkplaces AgentWorkplaces { get; set; }
        public AgentMovement AgentMovement { get; set; }
        public AgentWarehouse AgentWarehouse { get; set; }
        public AgentWorkers AgentWorkers { get; set; }
        public AgentWorkersA AgentWorkersA { get; set; }
        public AgentWorkersC AgentWorkersC { get; set; }
        public AgentWorkersB AgentWorkersB { get; set; }
        //meta! tag="end"
    }
}