using Agents.AgentCarpentry;
using Agents.AgentScope;
using Agents.AgentWorkers;
using Agents.AgentWorkersA;
using Agents.AgentWorkersB;
using Agents.AgentWorkersC;
using AgentSimulation.Utilities;
using OSPABA;
using Simulation;
using System.Windows.Controls;

namespace AgentSimulation.Delegates {
    public class TextBlockDelegate : ISimDelegate {
        private TextBlock[] textBlocks;

        public TextBlockDelegate(TextBlock[] textBlocks) {
            if (textBlocks.Length < 11) return;

            this.textBlocks = textBlocks;
        }

        public void Refresh(OSPABA.Simulation simulation) {
            if (simulation is MySimulation ms) {
                if (ms.Speed != double.MaxValue) {
                    var managerCarpentry = ms.AgentCarpentry.MyManager as ManagerCarpentry;

                    this.textBlocks[0].Text = Util.FormatTime(ms.CurrentTime);

                    if (managerCarpentry != null) {
                        this.textBlocks[1].Text = $"{managerCarpentry.QueueA.Count:F0}";
                        this.textBlocks[2].Text = $"{managerCarpentry.QueueB.Count:F0}";
                        this.textBlocks[3].Text = $"{managerCarpentry.QueueC.Count:F0}";
                        this.textBlocks[4].Text = $"{managerCarpentry.QueueD.Count:F0}";
                    }
                } else {
                    this.textBlocks[5].Text = $"{(100 * ms.AverageUtilityA.Mean()):F2}%";
                    this.textBlocks[6].Text = $"{(100 * ms.AverageUtilityB.Mean()):F2}%";
                    this.textBlocks[7].Text = $"{(100 * ms.AverageUtilityC.Mean()):F2}%";

                    this.textBlocks[8].Text = $"{ms.AverageFinishedOrdersCount.Mean():F2}";
                    this.textBlocks[9].Text = $"{ms.AveragePendingOrdersCount.Mean():F2}";

                    double[] interval = (ms.AverageOrderTime.SampleSize < 2) ? [double.NaN, double.NaN] : ms.AverageOrderTime.ConfidenceInterval95;

                    if (!double.IsNaN(interval[0]) && !double.IsNaN(interval[1])) {
                        this.textBlocks[10].Text = $"< {(interval[0] / 3600):F2}h ; {(interval[1] / 3600):F2}h >";
                    }
                }
            }
        }

        public void SimStateChanged(OSPABA.Simulation sim, SimState state) {
            sim.InvokeSync(() => Refresh(sim));
        }
    }
}
