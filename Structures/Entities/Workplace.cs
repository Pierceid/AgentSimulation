namespace AgentSimulation.Structures.Objects {
    public class Workplace {
        public int Id { get; }
        public bool IsOccupied { get; set; }
        public Product? Product { get; set; }
        public Worker? Worker { get; set; }

        public Workplace(int id) {
            Id = id;
            IsOccupied = false;
            Product = null;
            Worker = null;
        }

        public void AssignOrder(Product? order) {
            Product = order;
            IsOccupied = order != null;
        }

        public void AssignWorker(Worker? worker) {
            Worker = worker;
        }

        public void SetState(bool isOccupied) {
            IsOccupied = isOccupied;
        }
    }
}
