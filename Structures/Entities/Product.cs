using AgentSimulation.Structures.Enums;
using AgentSimulation.Utilities;
using System.ComponentModel;

namespace AgentSimulation.Structures.Entities {
    public class Product : INotifyPropertyChanged {
        public int Id { get; set; }
        public Order Order { get; set; }
        public ProductType Type { get; set; }
        public double StartTime { get; set; }
        public double EndTime { get; set; }
        public string FormattedTime { get; set; }
        public Worker? WorkerToCut { get; set; }
        public Worker? WorkerToPaint { get; set; }
        public Worker? WorkerToPickle { get; set; }
        public Worker? WorkerToAssemble { get; set; }
        public Worker? WorkerToMount { get; set; }
        public Worker? WorkerToDry { get; set; }
        public bool IsPickled { get; set; }

        private Workplace? workplace;
        public Workplace? Workplace {
            get => workplace;
            set {
                if (workplace != value) {
                    workplace = value;
                    OnPropertyChanged(nameof(workplace));
                }
            }
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

        public Product(int id, ProductType type, Order order) {
            Id = id;
            Type = type;
            Order = order;
            FormattedTime = Util.FormatTime(StartTime);
            State = ProductState.Raw;
            Workplace = null;
            IsPickled = false;
            WorkerToCut = null;
            WorkerToPaint = null;
            WorkerToPickle = null;
            WorkerToAssemble = null;
            WorkerToMount = null;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
