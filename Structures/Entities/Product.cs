using AgentSimulation.Structures.Enums;
using AgentSimulation.Utilities;
using System.ComponentModel;

namespace AgentSimulation.Structures.Objects {
    public class Product : INotifyPropertyChanged {
        public int Id { get; set; }
        public Order Order { get; set; }
        public ProductType Type { get; set; }
        public double StartTime { get; set; }
        public double EndTime { get; set; }
        public string FormattedTime { get; set; }
        public Workplace? Workplace { get; set; }
        public bool IsPickled { get; set; }

        public Product(int id, ProductType type, Order order) {
            Id = id;
            Type = type;
            Order = order;
            FormattedTime = Util.FormatTime(StartTime);
            State = ProductState.Raw;
            Workplace = null;
            IsPickled = false;
        }

        private ProductState state;
        public ProductState State {
            get => state;
            set {
                if (state != value) {
                    state = value;
                    OnPropertyChanged(nameof(State));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
