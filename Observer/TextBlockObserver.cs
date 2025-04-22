using AgentSimulation.Utilities;
using Simulation;
using System.Windows.Controls;

namespace AgentSimulation.Observer {
    public class TextBlockObserver : IObserver {
        private TextBlock[] textBlocks;

        public TextBlockObserver(TextBlock[] textBlocks) {
            if (textBlocks.Length < 11) return;

            this.textBlocks = textBlocks;
        }

        public void Refresh(OSPABA.Simulation simulation) {
            if (simulation is MySimulation ms) {
                if (ms.Speed != double.MaxValue) {
                    this.textBlocks[0].Text = Util.FormatTime(ms.CurrentTime);

                    //this.textBlocks[1].Text = $"{ms.AgentCarpentry.MyManager.QueueA.Count:F0}";
                    //this.textBlocks[2].Text = $"{ms.AgentCarpentry.MyManager.QueueB.Count:F0}";
                    //this.textBlocks[3].Text = $"{ms.AgentCarpentry.MyManager.QueueC.Count:F0}";
                    //this.textBlocks[4].Text = $"{ms.AgentCarpentry.MyManager.QueueD.Count:F0}";
                }

                //this.textBlocks[5].Text = $"{(100 * ms.AgentWorkersA.AverageUtilityA.GetAverage()):F2}%";
                //this.textBlocks[6].Text = $"{(100 * ms.AgentWorkersB.AverageUtilityB.GetAverage()):F2}%";
                //this.textBlocks[7].Text = $"{(100 * ms.AgentWorkersC.AverageUtilityC.GetAverage()):F2}%";

                this.textBlocks[8].Text = $"{ms.FinishedOrdersCount.Mean():F2}";
                this.textBlocks[9].Text = $"{ms.PendingOrdersCount.Mean():F2}";

                double[] interval = ms.AverageOrderTime.ConfidenceInterval95;

                this.textBlocks[10].Text = $"< {interval[0]:F0} , {interval[1]:F0} >";
            }
        }
    }
}
