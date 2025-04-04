using AgentSimulation.Simulations;
using AgentSimulation.Structures.Objects;
using AgentSimulation.Utilities;
using System.Windows.Controls;

namespace AgentSimulation.Observer {
    public class TextBlockObserver : IObserver {
        private TextBlock[] textBlocks;

        public TextBlockObserver(TextBlock[] textBlocks) {
            if (textBlocks.Length < 11) return;

            this.textBlocks = textBlocks;
        }

        public void Refresh(SimulationCore simulationCore) {
            if (simulationCore is EventSimulationCore<ProductionManager> esc) {
                if (esc is Carpentry c) {
                    if (c.Speed != double.MaxValue) {
                        this.textBlocks[0].Text = Util.FormatTime(c.SimulationTime);

                        this.textBlocks[1].Text = $"{c.Data.QueueA.Count:F0}";
                        this.textBlocks[2].Text = $"{c.Data.QueueB.Count:F0}";
                        this.textBlocks[3].Text = $"{c.Data.QueueC.Count:F0}";
                        this.textBlocks[4].Text = $"{c.Data.QueueD.Count:F0}";
                    }

                    this.textBlocks[5].Text = $"{(100 * c.AverageUtilityA.GetAverage()):F2}%";
                    this.textBlocks[6].Text = $"{(100 * c.AverageUtilityB.GetAverage()):F2}%";
                    this.textBlocks[7].Text = $"{(100 * c.AverageUtilityC.GetAverage()):F2}%";

                    this.textBlocks[8].Text = $"{c.AverageFinishedOrders.GetAverage():F2}";
                    this.textBlocks[9].Text = $"{c.AveragePendingOrders.GetAverage():F2}";

                    (double bottom, double top) = c.AverageOrderTime.GetConfidenceInterval();

                    this.textBlocks[10].Text = $"< {top:F0} , {bottom:F0} >";
                }
            }
        }
    }
}
