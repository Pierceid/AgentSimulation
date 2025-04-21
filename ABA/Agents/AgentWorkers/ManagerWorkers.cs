using AgentSimulation.Structures.Enums;
using OSPABA;
using Simulation;

namespace Agents.AgentWorkers {
    //meta! id="28"
    public class ManagerWorkers : OSPABA.Manager {
        public ManagerWorkers(int id, OSPABA.Simulation mySim, Agent myAgent) : base(id, mySim, myAgent) {
            Init();
        }

        override public void PrepareReplication() {
            base.PrepareReplication();
            if (PetriNet != null) {
                PetriNet.Clear();
            }
        }

        //meta! sender="AgentCarpentry", id="57", type="Request"
        public void ProcessGetWorkerForCutting(MessageForm message) {
            message.Addressee = MyAgent.FindAssistant(SimId.AgentWorkersA);
            message.Code = Mc.GetWorkerA;
            Request(message);
        }

        //meta! sender="AgentCarpentry", id="121", type="Request"
        public void ProcessGetWorkerForPainting(MessageForm message) {
            message.Addressee = MyAgent.FindAssistant(SimId.AgentWorkersB);
            message.Code = Mc.GetWorkerB;
            Request(message);
        }

        //meta! sender="AgentCarpentry", id="37", type="Notice"
        public void ProcessInit(MessageForm message) {

        }

        //meta! sender="AgentCarpentry", id="124", type="Request"
        public void ProcessGetWorkerForMounting(MessageForm message) {
            message.Addressee = MyAgent.FindAssistant(SimId.AgentWorkersC);
            message.Code = Mc.GetWorkerC;
            Request(message);
        }

        //meta! sender="AgentWorkersC", id="156", type="Response"
        public void ProcessGetWorkerC(MessageForm message) {
            message.Addressee = MySim.FindAgent(SimId.AgentCarpentry);
            message.Code = Mc.GetWorkerForMounting;
            Response(message);
        }

        //meta! sender="AgentWorkersB", id="157", type="Response"
        public void ProcessGetWorkerB(MessageForm message) {
            var myMessage = (MyMessage)message;
            myMessage.Code = Mc.GetWorkerForPainting;
            myMessage.Addressee = MySim.FindAgent(SimId.AgentCarpentry);
            Response(myMessage);
        }

        //meta! sender="AgentCarpentry", id="127", type="Notice"
        public void ProcessDeassignWorker(MessageForm message) {
            var myMessage = (MyMessage)message;
            switch (myMessage.Worker?.Group) {
                case WorkerGroup.A:
                    myMessage.Addressee = MyAgent.FindAssistant(SimId.AgentWorkersA);
                    break;
                case WorkerGroup.B:
                    myMessage.Addressee = MyAgent.FindAssistant(SimId.AgentWorkersB);
                    break;
                case WorkerGroup.C:
                    myMessage.Addressee = MyAgent.FindAssistant(SimId.AgentWorkersC);
                    break;
                default:
                    return;
            }
            Notice(myMessage);
        }

        //meta! sender="AgentWorkersA", id="159", type="Response"
        public void ProcessGetWorkerA(MessageForm message) {
            var myMessage = (MyMessage)message;
            switch (myMessage.Order?.State) {
                case ProductState.Raw:
                    myMessage.Code = Mc.GetWorkerForAssembling;
                    break;
                case ProductState.Assembled:
                    if (myMessage.Order.Type == ProductType.Wardrobe) myMessage.Code = Mc.GetWorkerForCutting;
                    break;
                default:
                    return;
            }
            myMessage.Addressee = MySim.FindAgent(SimId.AgentCarpentry);
            Response(myMessage);
        }

        //meta! sender="AgentCarpentry", id="122", type="Request"
        public void ProcessGetWorkerForAssembling(MessageForm message) {
            message.Addressee = MyAgent.FindAssistant(SimId.AgentWorkersA);
            message.Code = Mc.GetWorkerA;
            Request(message);
        }

        //meta! sender="AgentCarpentry", id="123", type="Request"
        public void ProcessGetWorkerForPickling(MessageForm message) {
            message.Addressee = MyAgent.FindAssistant(SimId.AgentWorkersB);
            message.Code = Mc.GetWorkerB;
            Request(message);
        }

        //meta! userInfo="Process messages defined in code", id="0"
        public void ProcessDefault(MessageForm message) {

        }

        //meta! userInfo="Generated code: do not modify", tag="begin"
        public void Init() {
        }

        override public void ProcessMessage(MessageForm message) {
            switch (message.Code) {
                case Mc.GetWorkerForAssembling:
                    ProcessGetWorkerForAssembling(message);
                    break;

                case Mc.GetWorkerA:
                    ProcessGetWorkerA(message);
                    break;

                case Mc.GetWorkerForPickling:
                    ProcessGetWorkerForPickling(message);
                    break;

                case Mc.GetWorkerForMounting:
                    ProcessGetWorkerForMounting(message);
                    break;

                case Mc.GetWorkerC:
                    ProcessGetWorkerC(message);
                    break;

                case Mc.GetWorkerForPainting:
                    ProcessGetWorkerForPainting(message);
                    break;

                case Mc.DeassignWorker:
                    ProcessDeassignWorker(message);
                    break;

                case Mc.Init:
                    ProcessInit(message);
                    break;

                case Mc.GetWorkerForCutting:
                    ProcessGetWorkerForCutting(message);
                    break;

                case Mc.GetWorkerB:
                    ProcessGetWorkerB(message);
                    break;

                default:
                    ProcessDefault(message);
                    break;
            }
        }
        //meta! tag="end"

        public new AgentWorkers MyAgent {
            get {
                return (AgentWorkers)base.MyAgent;
            }
        }
    }
}
