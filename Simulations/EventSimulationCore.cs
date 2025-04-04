using AgentSimulation.Generators;
using AgentSimulation.Observer;
using AgentSimulation.Structures.Events;
using System.Windows;
using System.Windows.Threading;

namespace AgentSimulation.Simulations {
    public abstract class EventSimulationCore<T> : SimulationCore, ISubject where T : class, new() {
        private List<IObserver> observers = [];
        public PriorityQueue<Event<T>, double> EventCalendar { get; set; }
        public T Data { get; set; }
        public RandomGenerators Generators { get; set; }
        public bool IsPaused { get; set; }
        public double Speed { get; set; }
        public double SimulationTime { get; set; }
        public double UpdateTime { get; set; }
        public double EndOfSimulationTime { get; set; }

        protected EventSimulationCore(int replicationStock, double endOfSimulationTime) : base(replicationStock) {
            this.EventCalendar = new();
            this.Data = new T();
            this.Generators = new();
            this.IsPaused = false;
            this.Speed = 1.0;
            this.SimulationTime = 0.0;
            this.UpdateTime = 1000.0 / 39.1;
            this.EndOfSimulationTime = endOfSimulationTime;
        }

        public override void BeforeSimulation() {
            this.SimulationTime = 0.0;
        }

        public override void AfterSimulation() {
            this.EventCalendar.Clear();
        }

        public override void BeforeSimulationRun() {

        }

        public override void AfterSimulationRun() {

        }

        public override void Experiment() {
            try {
                while (this.SimulationTime < this.EndOfSimulationTime && this.EventCalendar.Count > 0 && this.isRunning) {
                    while (this.IsPaused) {
                        Thread.Sleep(200);
                    }

                    if (this.EventCalendar.TryDequeue(out var nextEvent, out var priority)) {
                        if (this.SimulationTime > priority) throw new Exception("Simulation Time > Next Event Execution");

                        this.SimulationTime = priority;
                        nextEvent.Execute();
                    }
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        public void Pause() {
            this.IsPaused = true;
        }

        public void Resume() {
            this.IsPaused = false;
        }

        public void Attach(IObserver observer) {
            if (!observers.Contains(observer)) {
                observers.Add(observer);
            }
        }

        public void Detach(IObserver observer) {
            observers.Remove(observer);
        }

        public void Notify() {
            Application.Current.Dispatcher.Invoke(() => {
                foreach (var observer in observers) {
                    observer.Refresh(this);
                }
            }, DispatcherPriority.Background);
        }
    }
}
