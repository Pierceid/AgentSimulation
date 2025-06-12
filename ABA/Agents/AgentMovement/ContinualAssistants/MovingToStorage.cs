using OSPABA;
using Simulation;

namespace Agents.AgentMovement.ContinualAssistants {
    //meta! id="110"
    public class MovingToStorage : OSPABA.Process {
        public MovingToStorage(int id, OSPABA.Simulation mySim, CommonAgent myAgent) : base(id, mySim, myAgent) {
        }

        override public void PrepareReplication() {
            base.PrepareReplication();
        }

        //meta! sender="AgentMovement", id="111", type="Start"
        public void ProcessStart(MessageForm message) {
            var myMessage = (MyMessage)message;
            var mySimulation = (MySimulation)MySim;
            myMessage.Code = Mc.Finish;
            double duration = mySimulation.Generators.WorkerMoveToStorageTime.Next();
            var worker = myMessage.GetAssignedWorker();

            if (worker != null && myMessage.Product != null && myMessage.Product.Workplace != null && mySimulation.AnimatorExists) {
                var (x, y) = worker.GetRandomPosition();
                worker.Image.MoveTo(mySimulation.CurrentTime, duration, new(x, y));
            }

            Hold(duration, message);
        }

        //meta! userInfo="Process messages defined in code", id="0"
        public void ProcessDefault(MessageForm message) {
            var myMessage = (MyMessage)message;
            myMessage.Workplace = myMessage.Product?.Workplace;
            myMessage.GetAssignedWorker()?.SetWorkplace(myMessage.Workplace);
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