using AgentSimulation.Structures.Objects;
using Simulation;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace AgentSimulation.Observer {
    public class DataGridObserver : IObserver {
        private DataGrid[] dataGrids;

        private ObservableCollection<Order> Orders { get; }
        private ObservableCollection<Product> Products { get; }
        private ObservableCollection<Worker> Workers { get; }

        public DataGridObserver(DataGrid[] dataGrids) {
            Orders = new();
            Products = new();
            Workers = new();

            this.dataGrids = dataGrids;

            this.dataGrids[0].ItemsSource = Orders;
            this.dataGrids[1].ItemsSource = Products;
            this.dataGrids[2].ItemsSource = Workers;
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

        public void Refresh(OSPABA.Simulation simulation) {
            if (simulation is MySimulation ms) {
                if (ms.Speed != double.MaxValue) {
                    SyncCollection(Orders, ms.Orders);
                    SyncCollection(Products, ms.Products);
                    SyncCollection(Workers, ms.WorkersA.Concat(ms.WorkersB).Concat(ms.WorkersC));
                }
            }
        }
    }
}
