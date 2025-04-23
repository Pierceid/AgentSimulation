using AgentSimulation.Presentation;
using System.Windows.Threading;
using System.Windows;
using OSPABA;

namespace AgentSimulation.Delegates {
    public class LineGraphDelegate : ISimDelegate {
        private Window window;
        private LineGraph lineGraph;

        public LineGraphDelegate(Window window, LineGraph lineGraph) {
            this.window = window;
            this.lineGraph = lineGraph;
        }

        public void Refresh(OSPABA.Simulation simulation) {
            window.Dispatcher.Invoke(() => {
                lineGraph.UpdatePlot(simulation);
            }, DispatcherPriority.Input);
        }

        public void SimStateChanged(OSPABA.Simulation sim, SimState state) {
            sim.InvokeSync(() => Refresh(sim));
        }
    }
}
