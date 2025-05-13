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
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

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

        public void InitAnimator(ContentControl contentControl) {
            if (AnimatorExists) {
                Animator.ClearAll();
            } else {
                CreateAnimator();
            }

            var backGroundImage = new Bitmap(Util.GetFilePath("background.png"));
            Animator.SetBackgroundImage(backGroundImage);
            Animator.Canvas.Width = Constants.ANIMATION_WIDTH;
            Animator.Canvas.Height = Constants.ANIMATION_HEIGHT;
            Animator.Canvas.Margin = new Thickness(10);

            var storageImage = new AnimImageItem(Util.GetFilePath("storage.png"));
            storageImage.SetPosition(0, 0);
            Animator.Register(storageImage);

            var wallShape = new AnimShapeItem(AnimShape.RECTANGLE, 10, 740) { Color = Colors.Black, Fill = true };
            wallShape.SetPosition(780, 10);
            Animator.Register(wallShape);

            var queueShape1 = new AnimShapeItem(AnimShape.RECTANGLE, 120, 60) { Color = Colors.Blue, Fill = true };
            var queueShape2 = new AnimShapeItem(AnimShape.RECTANGLE, 120, 60) { Color = Colors.Red, Fill = true };
            var queueShape3 = new AnimShapeItem(AnimShape.RECTANGLE, 120, 60) { Color = Colors.Green, Fill = true };
            var queueShape4 = new AnimShapeItem(AnimShape.RECTANGLE, 120, 60) { Color = Colors.Purple, Fill = true };

            var queueText1 = new AnimTextItem("Queue A", Colors.White, null, 20);
            var queueText2 = new AnimTextItem("Queue B", Colors.White, null, 20);
            var queueText3 = new AnimTextItem("Queue C", Colors.White, null, 20);
            var queueText4 = new AnimTextItem("Queue D", Colors.White, null, 20);

            var queueCountText1 = new AnimTextItem("0", Colors.White, null, 20);
            var queueCountText2 = new AnimTextItem("0", Colors.White, null, 20);
            var queueCountText3 = new AnimTextItem("0", Colors.White, null, 20);
            var queueCountText4 = new AnimTextItem("0", Colors.White, null, 20);

            queueShape1.SetPosition(0, 700);
            queueText1.SetPosition(24, 700);
            queueCountText1.SetPosition(56, 730);
            Animator.Register(queueShape1);
            Animator.Register(queueText1);
            Animator.Register(queueCountText1);

            queueShape2.SetPosition(212, 700);
            queueText2.SetPosition(236, 700);
            queueCountText2.SetPosition(268, 730);
            Animator.Register(queueShape2);
            Animator.Register(queueText2);
            Animator.Register(queueCountText2);

            queueShape3.SetPosition(428, 700);
            queueText3.SetPosition(452, 700);
            queueCountText3.SetPosition(484, 730);
            Animator.Register(queueShape3);
            Animator.Register(queueText3);
            Animator.Register(queueCountText3);

            queueShape4.SetPosition(644, 700);
            queueText4.SetPosition(668, 700);
            queueCountText4.SetPosition(700, 730);
            Animator.Register(queueShape4);
            Animator.Register(queueText4);
            Animator.Register(queueCountText4);

            var managerCarpentry = AgentCarpentry.MyManager as ManagerCarpentry;
            managerCarpentry?.Workplaces.ForEach(wp => {
                Animator.Register(wp.Id, wp.Image);
                wp.Image.SetPosition(wp.X, wp.Y);
            });

            contentControl.Content = Animator.Canvas;
        }

        public void StartAnimation(ContentControl contentControl) {
            InitAnimator(contentControl);

            Animator.SetSynchronizedTime(false);
        }

        public void StopAnimation() {
            //if (AnimatorExists) {
            //    Animator.ClearAll();
            //}
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