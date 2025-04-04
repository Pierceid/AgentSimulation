using AgentSimulation.Simulations;
using AgentSimulation.Structures.Objects;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace AgentSimulation.Observer {
    public class DataGridObserver : IObserver {
        private DataGrid[] dataGrids;

        private ObservableCollection<Worker> Workers { get; }
        private ObservableCollection<Order> Orders { get; }

        public DataGridObserver(DataGrid[] dataGrids) {
            Workers = new();
            Orders = new();

            this.dataGrids = dataGrids;

            this.dataGrids[0].ItemsSource = Orders;
            this.dataGrids[1].ItemsSource = Workers;
        }

        public void Refresh(SimulationCore simulationCore) {
            if (simulationCore is EventSimulationCore<ProductionManager> esc) {
                if (esc.Speed != double.MaxValue) {
                    SyncCollection(Orders, esc.Data.Orders);
                    SyncCollection(Workers, esc.Data.WorkersA.Concat(esc.Data.WorkersB).Concat(esc.Data.WorkersC));
                }
            }
        }

        private void SyncCollection<T>(ObservableCollection<T> collection, IEnumerable<T> newItems) where T : class {
            var newItemsSet = newItems.ToHashSet();

            for (int i = collection.Count - 1; i >= 0; i--) {
                if (!newItemsSet.Contains(collection[i])) {
                    collection.RemoveAt(i);
                }
            }

            foreach (var item in newItems) {
                if (!collection.Contains(item)) {
                    collection.Add(item);
                }
            }
        }
    }
}
