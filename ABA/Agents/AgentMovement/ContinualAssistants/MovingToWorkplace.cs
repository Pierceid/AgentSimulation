using AgentSimulation.Structures.Entities;
using OSPABA;
using Simulation;

namespace Agents.AgentMovement.ContinualAssistants {
    //meta! id="101"
    public class MovingToWorkplace : OSPABA.Process {
        public MovingToWorkplace(int id, OSPABA.Simulation mySim, CommonAgent myAgent) : base(id, mySim, myAgent) {
        }

        override public void PrepareReplication() {
            base.PrepareReplication();
        }

        //meta! sender="AgentMovement", id="102", type="Start"
        public void ProcessStart(MessageForm message) {
            var myMessage = (MyMessage)message;
            var mySimulation = (MySimulation)message.MySim;
            myMessage.Code = Mc.Finish;
            var worker = myMessage.GetAssignedWorker();
            double duration = worker?.Workplace == null ? mySimulation.Generators.WorkerMoveToStorageTime.Next() : mySimulation.Generators.WorkerMoveBetweenStationsTime.Next();

            if (worker != null && myMessage.Product != null && myMessage.Product.Workplace != null && mySimulation.AnimatorExists) {
                worker.Image.MoveTo(mySimulation.CurrentTime, duration, new(myMessage.Product.Workplace.X, myMessage.Product.Workplace.Y));
            }

            Hold(duration, myMessage);
        }

        //meta! userInfo="Process messages defined in code", id="0"
        public void ProcessDefault(MessageForm message) {
            var myMessage = (MyMessage)message;
            var mySimulation = (MySimulation)message.MySim;
            myMessage.Workplace = myMessage.Product?.Workplace;
            myMessage.GetAssignedWorker()?.SetWorkplace(myMessage.Workplace);

            if (myMessage.Workplace != null && mySimulation.AnimatorExists) {
                mySimulation.Animator.GetAnim(myMessage.Workplace.Image.ObjectID).SetToolTip($"Product: {myMessage.Product?.Id}\nType: {myMessage.Product?.Type}\nState: {myMessage.Product?.State}\nIsOccupied: {myMessage.Workplace.IsOccupied}");
            }

            AssistantFinished(myMessage);
        }

        //meta! userInfo="Generated code: do not modify", tag="begin"
        override public void ProcessMessage(MessageForm message) {
            switch (message.Code) {
                case Mc.Start:
                    ProcessStart(message);
                    break;

                default:
                    ProcessDefault(message);
                    break;
            }
        }
        //meta! tag="end"

        public new AgentMovement MyAgent {
            get { return (AgentMovement)base.MyAgent; }
        }
    }
}