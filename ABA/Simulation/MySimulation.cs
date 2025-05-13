using Agents.AgentCarpentry;
using Agents.AgentModel;
using Agents.AgentMovement;
using Agents.AgentProcesses;
using Agents.AgentScope;
using Agents.AgentWorkers;
using Agents.AgentWorkersA;
using Agents.AgentWorkersB;
using Agents.AgentWorkersC;
using Agents.AgentWorkplaces;
using AgentSimulation.Generators;
using AgentSimulation.Utilities;
using OSPStat;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;

namespace Simulation {
    public class MySimulation : OSPABA.Simulation {
        public Stat AverageFinishedOrdersCount { get; set; } = new();
        public Stat AveragePendingOrdersCount { get; set; } = new();
        public Stat AverageOrderTime { get; set; } = new();
        public Stat AverageUtilityA { get; set; } = new();
        public Stat AverageUtilityB { get; set; } = new();
        public Stat AverageUtilityC { get; set; } = new();
        public RandomGenerators Generators { get; set; } = new();
        public double Speed { get; set; } = 1.0;

        public MySimulation() {
            Init();
        }

        override public void PrepareSimulation() {
            base.PrepareSimulation();

            Clear();

            AverageFinishedOrdersCount.Clear();
            AveragePendingOrdersCount.Clear();
            AverageOrderTime.Clear();
            AverageUtilityA.Clear();
            AverageUtilityB.Clear();
            AverageUtilityC.Clear();
        }

        override public void PrepareReplication() {
            base.PrepareReplication();

            Clear();
        }

        override public void ReplicationFinished() {
            base.ReplicationFinished();

            var managerScope = AgentScope.MyManager as ManagerScope;
            var managerCarpentry = AgentCarpentry.MyManager as ManagerCarpentry;
            var managerWorkersA = AgentWorkersA.MyManager as ManagerWorkersA;
            var managerWorkersB = AgentWorkersB.MyManager as ManagerWorkersB;
            var managerWorkersC = AgentWorkersC.MyManager as ManagerWorkersC;

            if (managerScope != null && managerCarpentry != null && managerWorkersA != null && managerWorkersB != null && managerWorkersC != null) {
                AverageFinishedOrdersCount.AddSample(managerScope.FinishedOrdersCount.SampleSize);
                AveragePendingOrdersCount.AddSample(managerCarpentry.QueueA.Count);
                AverageOrderTime.AddSample(managerScope.OrderTimes.Mean());
                AverageUtilityA.AddSample(managerWorkersA.GetAverageUtility());
                AverageUtilityB.AddSample(managerWorkersB.GetAverageUtility());
                AverageUtilityC.AddSample(managerWorkersC.GetAverageUtility());
            }

            OnRefreshUI(sim => Delegates.ForEach(d => d.Refresh(sim)));
        }

        override public void SimulationFinished() {
            base.SimulationFinished();
        }

        public void Clear() {
            var managerScope = AgentScope.MyManager as ManagerScope;
            var managerWorkersA = AgentWorkersA.MyManager as ManagerWorkersA;
            var managerWorkersB = AgentWorkersB.MyManager as ManagerWorkersB;
            var managerWorkersC = AgentWorkersC.MyManager as ManagerWorkersC;
            var managerCarpentry = AgentCarpentry.MyManager as ManagerCarpentry;

            managerScope?.Clear();
            managerWorkersA?.Clear();
            managerWorkersB?.Clear();
            managerWorkersC?.Clear();
            managerCarpentry?.Clear();
        }

        public void InitWorkers(int workersA, int workersB, int workersC) {
            var managerWorkersA = AgentWorkersA.MyManager as ManagerWorkersA;
            var managerWorkersB = AgentWorkersB.MyManager as ManagerWorkersB;
            var managerWorkersC = AgentWorkersC.MyManager as ManagerWorkersC;

            managerWorkersA?.InitWorkers(workersA);
            managerWorkersB?.InitWorkers(workersB);
            managerWorkersC?.InitWorkers(workersC);
        }

        public void InitWorkplaces(int workplaces) {
            var managerCarpentry = AgentCarpentry.MyManager as ManagerCarpentry;

            managerCarpentry?.InitWorkplaces(workplaces);
        }

        public void InitSpeed(double speed) {
            Speed = speed;

            if (Speed == double.MaxValue) {
                SetMaxSimSpeed();
            } else if (Speed > 0) {
                SetSimSpeed(Speed, 0.1);
            }
        }

        private void IniAnimator(ContentControl contentControl) {
            if (AnimatorExists) {
                Animator.ClearAll();
            } else {
                CreateAnimator();
            }

            Animator.SetBackgroundImage(new Bitmap(Util.GetFilePath("grey_background.png")));
            Animator.Canvas.Width = 1000;
            Animator.Canvas.Height = 800;
            Animator.Canvas.Margin = new Thickness(10);

            contentControl.Content = Animator.Canvas;
        }

        public void StartAnimation(ContentControl contentControl) {
            IniAnimator(contentControl);

            Animator.SetSynchronizedTime(false);
        }

        public void StopAnimation() {
            if (AnimatorExists) {
                Animator.ClearAll();
            }
        }

        //meta! userInfo="Generated code: do not modify", tag="begin"
        private void Init() {
            AgentModel = new AgentModel(SimId.AgentModel, this, null);
            AgentScope = new AgentScope(SimId.AgentScope, this, AgentModel);
            AgentCarpentry = new AgentCarpentry(SimId.AgentCarpentry, this, AgentModel);
            AgentWorkplaces = new AgentWorkplaces(SimId.AgentWorkplaces, this, AgentCarpentry);
            AgentMovement = new AgentMovement(SimId.AgentMovement, this, AgentCarpentry);
            AgentProcesses = new AgentProcesses(SimId.AgentProcesses, this, AgentCarpentry);
            AgentWorkers = new AgentWorkers(SimId.AgentWorkers, this, AgentCarpentry);
            AgentWorkersA = new AgentWorkersA(SimId.AgentWorkersA, this, AgentWorkers);
            AgentWorkersC = new AgentWorkersC(SimId.AgentWorkersC, this, AgentWorkers);
            AgentWorkersB = new AgentWorkersB(SimId.AgentWorkersB, this, AgentWorkers);
        }
        public AgentModel AgentModel { get; set; }
        public AgentScope AgentScope { get; set; }
        public AgentCarpentry AgentCarpentry { get; set; }
        public AgentWorkplaces AgentWorkplaces { get; set; }
        public AgentMovement AgentMovement { get; set; }
        public AgentProcesses AgentProcesses { get; set; }
        public AgentWorkers AgentWorkers { get; set; }
        public AgentWorkersA AgentWorkersA { get; set; }
        public AgentWorkersC AgentWorkersC { get; set; }
        public AgentWorkersB AgentWorkersB { get; set; }
        //meta! tag="end"
    }
}