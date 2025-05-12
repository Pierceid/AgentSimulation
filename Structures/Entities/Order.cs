using AgentSimulation.Structures.Enums;
using AgentSimulation.Utilities;
using System.ComponentModel;

namespace AgentSimulation.Structures.Objects {
    public class Order : INotifyPropertyChanged {
        public int Id { get; set; }
        public double StartTime { get; set; }
        public double EndTime { get; set; }
        public string FormattedTime { get; set; }

        private List<Product> products;
        public List<Product> Products {
            get => products;
            set {
                products = value;
                foreach (var product in products) {
                    product.PropertyChanged += ProductPropertyChanged;
                }
            }
        }

        private string state;
        public string State {
            get => state;
            set {
                if (state != value) {
                    state = value;
                    OnPropertyChanged(nameof(State));
                }
            }
        }

        public Order(int id, double startTime) {
            Id = id;
            StartTime = startTime;
            FormattedTime = Util.FormatTime(startTime);
            Products = new();
            State = "0/0";
        }

        public void AddProducts(List<Product> products) {
            Products = products;
            State = $"0/{Products.Count}";
        }

        public void UpdateProduct(Product product) {
            var match = Products.FirstOrDefault(p => p.Id == product.Id);

            if (match != null) {
                match.State = product.State;
                CheckOrderCompletion();
            }
        }

        private void ProductPropertyChanged(object? sender, PropertyChangedEventArgs e) {
            if (e.PropertyName == nameof(ProductState.Finished)) {
                CheckOrderCompletion();
            }
        }

        private void CheckOrderCompletion() {
            var finished = Products.FindAll(p => p.State == ProductState.Finished);

            if (finished.Count == Products.Count) {
                State = "Completed";
            } else {
                State = $"{finished.Count}/{Products.Count}";
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
