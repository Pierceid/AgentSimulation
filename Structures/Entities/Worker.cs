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

        private Product? product;
        public Product? Product {
            get => product;
            set {
                if (product != value) {
                    product = value;
                    OnPropertyChanged(nameof(Product));
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
            Product = null;
            Workplace = null;
            Utility = new();
        }

        public void SetState(bool isBusy) {
            if (!isBusy) Product = null;

            IsBusy = isBusy;
        }

        public void SetProduct(Product? order) {
            Product = order;
            IsBusy = order != null;
        }

        public void SetWorkplace(Workplace? workplace) {
            Workplace = workplace;
        }

        public void Clear() {
            Product = null;
            Workplace = null;
            IsBusy = false;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
