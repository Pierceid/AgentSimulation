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

        public void AssignOrder(Product? product) {
            Product = product;
            IsOccupied = product != null;
        }

        public void AssignWorker(Worker? worker) {
            Worker = worker;
        }

        public void SetState(bool isOccupied) {
            if (!isOccupied) Product = null;
            IsOccupied = isOccupied;
        }
    }
}
