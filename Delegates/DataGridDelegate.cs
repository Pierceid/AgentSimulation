﻿using Agents.AgentScope;
using Agents.AgentWorkersA;
using Agents.AgentWorkersB;
using Agents.AgentWorkersC;
using AgentSimulation.Structures.Entities;
using AgentSimulation.Structures.Enums;
using OSPABA;
using Simulation;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace AgentSimulation.Delegates {
    public class DataGridDelegate : ISimDelegate {
        private DataGrid[] dataGrids;

        private ObservableCollection<Order> Orders { get; }
        private ObservableCollection<Product> Products { get; }
        private ObservableCollection<Worker> Workers { get; }

        public DataGridDelegate(DataGrid[] dataGrids) {
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
                    var managerScope = ms.AgentScope.MyManager as ManagerScope;
                    var managerWorkersA = ms.AgentWorkersA.MyManager as ManagerWorkersA;
                    var managerWorkersB = ms.AgentWorkersB.MyManager as ManagerWorkersB;
                    var managerWorkersC = ms.AgentWorkersC.MyManager as ManagerWorkersC;

                    if (managerScope != null) {
                        var orders = managerScope.Orders.ToList();
                        if (orders.Count > 100) {
                            orders = orders.Where(o => o.State != "Completed").ToList();
                        }
                        SyncCollection(Orders, orders);

                        var products = managerScope.Products.ToList();
                        if (products.Count > 300) {
                            products = products.Where(p => p.State != ProductState.Finished).ToList();
                        }
                        SyncCollection(Products, products);
                    }

                    if (managerWorkersA != null && managerWorkersB != null && managerWorkersC != null) {
                        SyncCollection(Workers, managerWorkersA.Workers.Concat(managerWorkersB.Workers).Concat(managerWorkersC.Workers));
                    }
                }
            }
        }

        public void SimStateChanged(OSPABA.Simulation sim, SimState state) {
            sim.InvokeSync(() => Refresh(sim));
        }
    }
}
