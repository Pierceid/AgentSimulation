using AgentSimulation.Statistics;
using AgentSimulation.Structures.Enums;
using AgentSimulation.Utilities;
using OSPAnimator;
using System.ComponentModel;

namespace AgentSimulation.Structures.Objects {
    public class Worker : INotifyPropertyChanged {
        public int Id { get; }
        public WorkerGroup Group { get; set; }
        public Utility Utility { get; set; }
        private WorkerState state;
        public WorkerState State {
            get => state;
            set {
                if (state != value) {
                    state = value;
                    OnPropertyChanged(nameof(State));
                }
            }
        }

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

        public AnimImageItem Image { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public static Random Random { get; } = new();

        public Worker(int id, WorkerGroup group) {
            Id = id;
            Group = group;
            IsBusy = false;
            Product = null;
            Workplace = null;
            State = WorkerState.WAITING;
            Utility = new();
            Image = new(Util.GetFilePath(group == WorkerGroup.A ? "worker_a.png" : group == WorkerGroup.B ? "worker_b.png" : "worker_c.png"));
            var (x, y) = GetRandomPosition();     
            X = x;
            Y = y;
            Image.SetToolTip($"Worker: {Id}\nGroup: {Group}\nState: {State}\nProduct: {Product?.Id}\nIsBusy: {IsBusy}");
        }

        public (int x, int y) GetRandomPosition() => (Random.Next(0, 576), Random.Next(0, 120));

        public void SetIsBusy(bool isBusy) {
            if (!isBusy) Product = null;
            IsBusy = isBusy;
            Image.SetToolTip($"Worker: {Id}\nGroup: {Group}\nState: {State}\nProduct: {Product?.Id}\nIsBusy: {IsBusy}");
        }

        public void SetState(WorkerState workerState) {
            State = workerState;
            Image.SetToolTip($"Worker: {Id}\nGroup: {Group}\nState: {State}\nProduct: {Product?.Id}\nIsBusy: {IsBusy}");
        }

        public void SetProduct(Product? order) {
            Product = order;
            IsBusy = order != null;
            Image.SetToolTip($"Worker: {Id}\nGroup: {Group}\nState: {State}\nProduct: {Product?.Id}\nIsBusy: {IsBusy}");
        }

        public void SetWorkplace(Workplace? workplace) {
            Workplace = workplace;
            Image.SetToolTip($"Worker: {Id}\nGroup: {Group}\nState: {State}\nProduct: {Product?.Id}\nIsBusy: {IsBusy}");
        }

        public void Clear() {
            Product = null;
            Workplace = null;
            IsBusy = false;
            Image.SetToolTip($"Worker: {Id}\nGroup: {Group}\nState: {State}\nProduct: {Product?.Id}\nIsBusy: {IsBusy}");
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
