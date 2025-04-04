using AgentSimulation.Structures.Enums;
using AgentSimulation.Utilities;
using System.ComponentModel;

namespace AgentSimulation.Structures.Objects {
    public class Order : INotifyPropertyChanged {
        public int Id { get; set; }
        public ProductType Type { get; set; }
        public double StartTime { get; set; }
        public double EndTime { get; set; }
        public string FormattedTime { get; set; }
        public Workplace? Workplace { get; set; }

        public Order(int id, ProductType type, double time) {
            Id = id;
            Type = type;
            StartTime = time;
            EndTime = time;
            FormattedTime = Util.FormatTime(StartTime);
            State = ProductState.Raw;
            Workplace = null;
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
