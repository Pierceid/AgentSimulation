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
using AgentSimulation.Structures;
using AgentSimulation.Utilities;
using OSPAnimator;
using OSPStat;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

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

        private Window? animatorWindow = null;

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

            if (AnimatorExists) {
                Animator.ClearAll();
                Application.Current.Dispatcher.Invoke(() => InitAnimator());
            }
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

                //if (AverageOrderTime.SampleSize >= 30) {
                //    double[] interval = AverageOrderTime.ConfidenceInterval95;

                //    if (interval[0] >= 0.99 * AverageOrderTime.Mean() || interval[1] <= 1.01 * AverageOrderTime.Mean()) {
                //        StopSimulation();
                //    }
                //}
            }

            OnRefreshUI(sim => Delegates.ForEach(d => d.Refresh(sim)));
        }

        override public void SimulationFinished() {
            base.SimulationFinished();
        }

        public void Clear() {
            (AgentScope.MyManager as ManagerScope)?.Clear();
            (AgentWorkersA.MyManager as ManagerWorkersA)?.Clear();
            (AgentWorkersB.MyManager as ManagerWorkersB)?.Clear();
            (AgentWorkersC.MyManager as ManagerWorkersC)?.Clear();
            (AgentCarpentry.MyManager as ManagerCarpentry)?.Clear();
        }

        public void InitWorkers(int workersA, int workersB, int workersC) {
            (AgentWorkersA.MyManager as ManagerWorkersA)?.InitWorkers(workersA);
            (AgentWorkersB.MyManager as ManagerWorkersB)?.InitWorkers(workersB);
            (AgentWorkersC.MyManager as ManagerWorkersC)?.InitWorkers(workersC);
        }

        public void InitWorkplaces(int workplaces) {
            (AgentCarpentry.MyManager as ManagerCarpentry)?.InitWorkplaces(workplaces);
        }

        public void InitSpeed(double speed) {
            Application.Current.Dispatcher.InvokeAsync(() => {
                Speed = speed;

                if (Speed > 0 && Speed != double.MaxValue) {
                    SetSimSpeed(Speed / Constants.FPS, 1.0 / Constants.FPS);
                } else if (Speed == double.MaxValue) {
                    SetMaxSimSpeed();
                }
            });
        }

        public void InitAnimator() {
            if (!AnimatorExists) return;

            Application.Current.Dispatcher.Invoke(() => {
                Animator.SetSynchronizedTime(false);

                var backgroundImage = new Bitmap(Util.GetFilePath("background.png"));
                Animator.SetBackgroundImage(backgroundImage);

                Animator.Canvas.VerticalAlignment = VerticalAlignment.Top;
                Animator.Canvas.HorizontalAlignment = HorizontalAlignment.Left;
                Animator.Canvas.Width = Constants.ANIMATION_WIDTH;
                Animator.Canvas.Height = Constants.ANIMATION_HEIGHT;
                Animator.Canvas.Margin = new Thickness(10);

                SetupQueues();

                AgentCarpentry.InitAnimator();
                AgentWorkersA.InitAnimator();
                AgentWorkersB.InitAnimator();
                AgentWorkersC.InitAnimator();

                if (animatorWindow == null) {
                    animatorWindow = new Window {
                        Title = "Animator",
                        Width = Constants.ANIMATION_WIDTH,
                        Height = Constants.ANIMATION_HEIGHT,
                        Content = Animator.Canvas
                    };
                    animatorWindow.Show();
                }
            });
        }

        private void SetupQueues() {
            Application.Current.Dispatcher.Invoke(() => {
                var colors = new[] { Colors.Blue, Colors.Red, Colors.Green, Colors.Purple };
                for (int i = 0; i < 4; i++) {
                    var shape = new AnimShapeItem(AnimShape.RECTANGLE, 120, 60) {
                        Color = colors[i],
                        Fill = true
                    };
                    shape.SetPosition(i * 212, 700);
                    Animator.Register(shape);

                    var label = new AnimTextItem($"Queue {(char)('A' + i)}", Colors.White, null, 20);
                    label.SetPosition(i * 212 + 24, 700);
                    Animator.Register(label);

                    var count = new AnimTextItem("0", Colors.White, null, 20);
                    count.SetPosition(i * 212 + 56, 730);
                    Animator.Register(1001 + i, count);
                }

                var wallShape = new AnimShapeItem(AnimShape.RECTANGLE, 10, 740) {
                    Color = Colors.Black,
                    Fill = true
                };
                wallShape.SetPosition(780, 10);
                Animator.Register(wallShape);

                var storageImage = new AnimImageItem(Util.GetFilePath("storage.png"));
                storageImage.SetPosition(0, 0);
                Animator.Register(storageImage);
            });
        }

        public void StartAnimation() {
            CreateAnimator();
            InitAnimator();
        }

        public void StopAnimation() {
            Application.Current.Dispatcher.Invoke(() => {
                if (animatorWindow != null) {
                    RemoveAnimator();
                    animatorWindow.Content = null;
                    animatorWindow.Close();
                    animatorWindow = null;
                }
            });
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
