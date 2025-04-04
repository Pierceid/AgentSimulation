using AgentSimulation.Statistics;
using AgentSimulation.Structures.Enums;
using System.ComponentModel;

namespace AgentSimulation.Structures.Objects {
    public class Worker : INotifyPropertyChanged {
        public int Id { get; }
        public WorkerGroup Group { get; set; }
        public Utility Utility { get; set; }

        private bool isBusy;
        public bool IsBusy {
            get => isBusy;
            set {
                if (isBusy != value) {
                    isBusy = value;
                    OnPropertyChanged(nameof(IsBusy));
                }
            }
        }

        private Order? order;
        public Order? Order {
            get => order;
            set {
                if (order != value) {
                    order = value;
                    OnPropertyChanged(nameof(Order));
                }
            }
        }

        private Workplace? workplace;
        public Workplace? Workplace {
            get => workplace;
            set {
                if (workplace != value) {
                    workplace = value;
                    OnPropertyChanged(nameof(Workplace));
                }
            }
        }

        public Worker(int id, WorkerGroup group) {
            Id = id;
            Group = group;
            IsBusy = false;
            Order = null;
            Workplace = null;
            Utility = new();
        }

        public void SetState(bool isBusy) {
            if (!isBusy) Order = null;

            IsBusy = isBusy;
        }

        public void SetOrder(Order? order) {
            Order = order;
            IsBusy = order != null;
        }

        public void SetWorkplace(Workplace? workplace) {
            Workplace = workplace;
        }

        public void Clear() {
            Order = null;
            Workplace = null;
            IsBusy = false;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
