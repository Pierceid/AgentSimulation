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
using AgentSimulation.Observer;
using AgentSimulation.Structures.Enums;
using AgentSimulation.Structures.Objects;
using OSPStat;
using System.Windows;
using System.Windows.Threading;

namespace Simulation {
    public class MySimulation : OSPABA.Simulation, ISubject {
        private List<IObserver> observers = [];
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
            Notify();
        }

        override public void ReplicationFinished() {
            base.ReplicationFinished();

            Notify();
        }

        override public void SimulationFinished() {
            Notify();

            base.SimulationFinished();
        }

        //meta! userInfo="Generated code: do not modify", tag="begin"
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

        public void InitComponents(int workersA, int workersB, int workersC) {
            Parallel.For(0, workersA, a => { lock (WorkersA) { WorkersA.Add(new Worker(a, WorkerGroup.A)); } });
            Parallel.For(0, workersB, b => { lock (WorkersB) { WorkersB.Add(new Worker(b + workersA, WorkerGroup.B)); } });
            Parallel.For(0, workersC, c => { lock (WorkersC) { WorkersC.Add(new Worker(c + workersA + workersB, WorkerGroup.C)); } });
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

            InitComponents(workersA, workersB, workersC);
        }

        public void Attach(IObserver observer) {
            if (!observers.Contains(observer)) {
                observers.Add(observer);
            }
        }

        public void Detach(IObserver observer) {
            observers.Remove(observer);
        }

        public void Notify() {
            Application.Current.Dispatcher.Invoke(() => {
                foreach (var observer in observers) {
                    observer.Refresh(this);
                }
            }, DispatcherPriority.Background);
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