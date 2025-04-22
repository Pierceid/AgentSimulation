using AgentSimulation.Statistics;
using AgentSimulation.Structures.Enums;

namespace AgentSimulation.Structures.Objects {
    public class ProductionManager {
        public List<Product> Orders { get; } = new();
        public LinkedList<Product> QueueA { get; } = new();
        public LinkedList<Product> QueueB { get; } = new();
        public LinkedList<Product> QueueC { get; } = new();
        public LinkedList<Product> QueueD { get; } = new();
        public List<Worker> WorkersA { get; } = new();
        public List<Worker> WorkersB { get; } = new();
        public List<Worker> WorkersC { get; } = new();
        public List<Workplace> Workplaces { get; } = new();
        public Average AverageOrderTime { get; set; } = new();
        public Counter AverageFinishedOrders { get; set; } = new();
        public Counter AveragePendingOrders { get; set; } = new();
        public Utility AverageUtilityA { get; set; } = new();
        public Utility AverageUtilityB { get; set; } = new();
        public Utility AverageUtilityC { get; set; } = new();

        private static int nextWorkplaceId = 0;

        public ProductionManager() { }

        public ProductionManager(int workersA, int workersB, int workersC) {
            InitComponents(workersA, workersB, workersC);
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
            QueueA.Clear();
            QueueB.Clear();
            QueueC.Clear();
            QueueD.Clear();
            WorkersA.Clear();
            WorkersB.Clear();
            WorkersC.Clear();
            Workplaces.Clear();

            AverageOrderTime.Clear();
            AverageFinishedOrders.Clear();
            AveragePendingOrders.Clear();
            AverageUtilityA.Clear();
            AverageUtilityB.Clear();
            AverageUtilityC.Clear();

            InitComponents(workersA, workersB, workersC);
        }

        public List<Worker> GetAvailableWorkers(ProductState state) {
            return state switch {
                ProductState.Raw => [.. WorkersA.FindAll(w => !w.IsBusy)],
                ProductState.Painted => [.. WorkersB.FindAll(w => !w.IsBusy)],
                ProductState.Cut => [.. WorkersC.FindAll(w => !w.IsBusy)],
                ProductState.Assembled => [.. WorkersC.FindAll(w => !w.IsBusy)],
                _ => []
            };
        }

        public Workplace GetOrCreateWorkplace() {
            Workplace? workplace = Workplaces.FirstOrDefault(w => !w.IsOccupied);

            if (workplace == null) {
                workplace = new Workplace(nextWorkplaceId++);
                Workplaces.Add(workplace);
            }

            return workplace;
        }
    }
}
