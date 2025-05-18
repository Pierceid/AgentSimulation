using AgentSimulation.Utilities;
using OSPAnimator;

namespace AgentSimulation.Structures.Objects {
    public class Workplace {
        public int Id { get; }
        public bool IsOccupied { get; set; }
        public Product? Product { get; set; }
        public Worker? Worker { get; set; }
        public AnimImageItem Image { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Workplace(int id) {
            Id = id;
            IsOccupied = false;
            Product = null;
            Worker = null;
            Image = new(Util.GetFilePath("workplace.png"));
            Image.SetToolTip($"Product: {Product?.Id}\nType: {Product?.Type}\nState: {Product?.State}\nIsOccupied: {IsOccupied}");
            int columns = (Constants.ANIMATION_WIDTH / Constants.IMAGE_SIZE) - 5;

            X = (Id % columns) * (Constants.IMAGE_SIZE + 22);
            Y = (Id / columns + 3) * (Constants.IMAGE_SIZE + 22);
        }

        public void AssignProduct(Product? product) {
            Product = product;
            IsOccupied = product != null;
            Image.SetToolTip($"Product: {Product?.Id}\nType: {Product?.Type}\nState: {Product?.State}\nIsOccupied: {IsOccupied}");
        }

        public void AssignWorker(Worker? worker) {
            Worker = worker;
            Image.SetToolTip($"Product: {Product?.Id}\nType: {Product?.Type}\nState: {Product?.State}\nIsOccupied: {IsOccupied}");
        }

        public void SetState(bool isOccupied) {
            if (!isOccupied) Product = null;
            IsOccupied = isOccupied;
            Image.SetToolTip($"Product: {Product?.Id}\nType: {Product?.Type}\nState: {Product?.State}\nIsOccupied: {IsOccupied}");
        }

        public void Clear() {
            Product = null;
            Worker = null;
            IsOccupied = false;
        }
    }
}
