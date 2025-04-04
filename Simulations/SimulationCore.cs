namespace AgentSimulation.Simulations {
    public abstract class SimulationCore {
        protected Thread? thread;
        protected bool isRunning;
        public int CurrentReplication { get; set; }
        public int ReplicationStock { get; set; }

        private readonly ManualResetEvent stopSignal = new(false);

        protected SimulationCore(int replicationStock) {
            this.thread = null;
            this.isRunning = false;
            this.CurrentReplication = 0;
            this.ReplicationStock = replicationStock;
        }

        public void Start() {
            this.isRunning = true;
            stopSignal.Reset();
        }

        public void Stop() {
            this.isRunning = false;
            stopSignal.Set();

            if (this.thread != null && this.thread.IsAlive) {
                this.thread.Join(500);

                if (this.thread.IsAlive) {
                    this.thread.Interrupt();
                }
            }

            this.thread = null;
        }

        public void RunSimulation() {
            this.isRunning = true;
            stopSignal.Reset();

            this.thread = new Thread(Simulate) { IsBackground = true };
            this.thread.Start();
        }

        private void Simulate() {
            try {
                BeforeSimulationRun();

                for (this.CurrentReplication = 0; this.CurrentReplication < this.ReplicationStock; this.CurrentReplication++) {
                    if (!this.isRunning || stopSignal.WaitOne(0)) break;

                    BeforeSimulation();
                    Experiment();
                    AfterSimulation();
                }

                AfterSimulationRun();
            } catch (ThreadInterruptedException) { } finally {
                this.isRunning = false;
            }
        }

        public abstract void Experiment();
        public abstract void BeforeSimulation();
        public abstract void AfterSimulation();
        public abstract void BeforeSimulationRun();
        public abstract void AfterSimulationRun();
    }
}
