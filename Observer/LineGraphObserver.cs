using AgentSimulation.Presentation;
using System.Windows;
using System.Windows.Threading;

namespace AgentSimulation.Observer {
    public class LineGraphObserver : IObserver {
        private Window window;
        private LineGraph lineGraph;

        public LineGraphObserver(Window window, LineGraph lineGraph) {
            this.window = window;
            this.lineGraph = lineGraph;
        }

        public void Refresh(OSPABA.Simulation simulation) {
            window.Dispatcher.Invoke(() => {
                lineGraph.UpdatePlot(simulation);
            }, DispatcherPriority.Input);
        }
    }
}
