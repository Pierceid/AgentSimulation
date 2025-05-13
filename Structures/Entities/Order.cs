using AgentSimulation.Structures.Enums;
using AgentSimulation.Utilities;
using System.ComponentModel;

namespace AgentSimulation.Structures.Objects {
    public class Order : INotifyPropertyChanged {
        public int Id { get; set; }
        public double StartTime { get; set; }
        public double EndTime { get; set; }
        public string FormattedTime { get; set; }

        private List<Product> products = new();
        public List<Product> Products {
            get => products;
            set {
                products = value;
                foreach (var product in products) {
                    product.PropertyChanged += ProductPropertyChanged;
                }
                UpdateState();
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
            State = "0/0";
        }

        public void AddProducts(List<Product> products) {
            Products = products;
        }

        public void UpdateProduct(Product product) {
            var index = Products.FindIndex(p => p.Id == product.Id);
            if (index != -1) {
                Products[index] = product;
                product.PropertyChanged += ProductPropertyChanged;
                UpdateState();
            }
        }

        private void ProductPropertyChanged(object? sender, PropertyChangedEventArgs e) {
            if (e.PropertyName == nameof(Product.State)) {
                UpdateState();
            }
        }

        private void UpdateState() {
            var finishedCount = Products.Count(p => p.State == ProductState.Finished);

            if (finishedCount == Products.Count && Products.Count > 0) {
                State = "Completed";
            } else {
                State = $"{finishedCount}/{Products.Count}";
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
