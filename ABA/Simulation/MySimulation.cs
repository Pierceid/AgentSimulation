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
using AgentSimulation.Structures.Enums;
using AgentSimulation.Structures.Objects;
using OSPStat;

namespace Simulation {
    public class MySimulation : OSPABA.Simulation {
        public Stat FinishedOrdersCount { get; set; } = new();
        public Stat PendingOrdersCount { get; set; } = new();
        public Stat AverageOrderTime { get; set; } = new();
        public RandomGenerators Generators { get; set; } = new();
        public List<Order> Orders { get; set; } = new();
        public List<Product> Products { get; set; } = new();
        public List<Worker> WorkersA { get; set; } = new();
        public List<Worker> WorkersB { get; set; } = new();
        public List<Worker> WorkersC { get; set; } = new();
        public List<Workplace> Workplaces { get; set; } = new();
        public double Speed { get; set; } = 1.0;

        public MySimulation() {
            Init();
        }

        override public void PrepareSimulation() {
            base.PrepareSimulation();

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

        private void Init() {
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

        public void InitWorkers(int workersA, int workersB, int workersC) {
            Parallel.For(0, workersA, a => { lock (WorkersA) { WorkersA.Add(new Worker(a, WorkerGroup.A)); } });
            Parallel.For(0, workersB, b => { lock (WorkersB) { WorkersB.Add(new Worker(b + workersA, WorkerGroup.B)); } });
            Parallel.For(0, workersC, c => { lock (WorkersC) { WorkersC.Add(new Worker(c + workersA + workersB, WorkerGroup.C)); } });
        }

        public void InitWorkplaces(int workplaces) {
            Parallel.For(0, workplaces, w => { lock (Workplaces) { Workplaces.Add(new Workplace(w)); } });
        }

        public void InitSpeed(double speed) {
            Speed = speed;

            if (Speed > 0) {
                SetSimSpeed(Speed, 0.1);
            } else {
                SetMaxSimSpeed();
            }
        }

        public void Clear() {
            int workersA = WorkersA.Count;
            int workersB = WorkersB.Count;
            int workersC = WorkersC.Count;

            Orders.Clear();
            Products.Clear();
            WorkersA.Clear();
            WorkersB.Clear();
            WorkersC.Clear();
            Workplaces.Clear();

            AverageOrderTime.Clear();

            InitWorkers(workersA, workersB, workersC);
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
    }
}
