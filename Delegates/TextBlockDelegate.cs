using Agents.AgentCarpentry;
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
                    double[] intervalUtilityA = (ms.AverageUtilityA.SampleSize < 2) ? [double.NaN, double.NaN] : ms.AverageUtilityA.ConfidenceInterval95;
                    double[] intervalUtilityB = (ms.AverageUtilityB.SampleSize < 2) ? [double.NaN, double.NaN] : ms.AverageUtilityB.ConfidenceInterval95;
                    double[] intervalUtilityC = (ms.AverageUtilityC.SampleSize < 2) ? [double.NaN, double.NaN] : ms.AverageUtilityC.ConfidenceInterval95;
                    double[] intervalOrderTime = (ms.AverageOrderTime.SampleSize < 2) ? [double.NaN, double.NaN] : ms.AverageOrderTime.ConfidenceInterval95;

                    if (!double.IsNaN(intervalUtilityA[0]) && !double.IsNaN(intervalUtilityA[1])) {
                        this.textBlocks[5].Text = $"( {(intervalUtilityA[0] * 100):F2} ; {(intervalUtilityA[1] * 100):F2} ) %";
                    }
                    if (!double.IsNaN(intervalUtilityB[0]) && !double.IsNaN(intervalUtilityB[1])) {
                        this.textBlocks[6].Text = $"( {(intervalUtilityB[0] * 100):F2} ; {(intervalUtilityB[1] * 100):F2} ) %";
                    }
                    if (!double.IsNaN(intervalUtilityC[0]) && !double.IsNaN(intervalUtilityC[1])) {
                        this.textBlocks[7].Text = $"( {(intervalUtilityC[0] * 100):F2} ; {(intervalUtilityC[1] * 100):F2} ) %";
                    }

                    this.textBlocks[8].Text = $"{ms.AverageFinishedOrdersCount.Mean():F2}";
                    this.textBlocks[9].Text = $"{ms.AveragePendingOrdersCount.Mean():F2}";

                    if (!double.IsNaN(intervalOrderTime[0]) && !double.IsNaN(intervalOrderTime[1])) {
                        this.textBlocks[10].Text = $"( {(intervalOrderTime[0] / 3600):F2} ; {(intervalOrderTime[1] / 3600):F2} ) h";
                    }
                }
            }
        }

        public void SimStateChanged(OSPABA.Simulation sim, SimState state) {
            sim.InvokeSync(() => Refresh(sim));
        }
    }
}
