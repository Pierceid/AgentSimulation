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

            PetriNet?.Clear();
        }

        //meta! sender="AgentCarpentry", id="57", type="Request"
        public void ProcessGetWorkerForCutting(MessageForm message) {
            message.Addressee = MySim.FindAgent(SimId.AgentWorkersA);
            message.Code = Mc.GetWorkerA;
            Request(message);
        }

        //meta! sender="AgentCarpentry", id="121", type="Request"
        public void ProcessGetWorkerForPainting(MessageForm message) {
            message.Addressee = MySim.FindAgent(SimId.AgentWorkersB);
            message.Code = Mc.GetWorkerC;
            Request(message);
        }

        //meta! sender="AgentCarpentry", id="124", type="Request"
        public void ProcessGetWorkerForMounting(MessageForm message) {
            message.Addressee = MySim.FindAgent(SimId.AgentWorkersC);
            message.Code = Mc.GetWorkerC;
            Request(message);
        }

        //meta! sender="AgentCarpentry", id="122", type="Request"
        public void ProcessGetWorkerForAssembling(MessageForm message) {
            message.Addressee = MySim.FindAgent(SimId.AgentWorkersA);
            message.Code = Mc.GetWorkerB;
            Request(message);
        }

        //meta! sender="AgentCarpentry", id="123", type="Request"
        public void ProcessGetWorkerForPickling(MessageForm message) {
            message.Addressee = MySim.FindAgent(SimId.AgentWorkersB);
            message.Code = Mc.GetWorkerC;
            Request(message);
        }

        //meta! sender="AgentWorkersA", id="159", type="Response"
        public void ProcessGetWorkerA(MessageForm message) {
            message.Addressee = MySim.FindAgent(SimId.AgentCarpentry);
            message.Code = Mc.GetWorkerForCutting;
            Response(message);
        }

        //meta! sender="AgentWorkersB", id="157", type="Response"
        public void ProcessGetWorkerB(MessageForm message) {
            message.Addressee = MySim.FindAgent(SimId.AgentCarpentry);
            message.Code = Mc.GetWorkerForPainting;
            Response(message);
        }

        //meta! sender="AgentWorkersC", id="156", type="Response"
        public void ProcessGetWorkerC(MessageForm message) {
            message.Addressee = MySim.FindAgent(SimId.AgentCarpentry);
            message.Code = Mc.GetWorkerForMounting;
            Response(message);
        }

        //meta! sender="AgentCarpentry", id="127", type="Notice"
        public void ProcessDeassignWorkerA(MessageForm message) {
            message.Addressee = MySim.FindAgent(SimId.AgentWorkersA);
            message.Code = Mc.DeassignWorkerA;
            Notice(message);
        }

        //meta! sender="AgentCarpentry", id="201", type="Notice"
        public void ProcessDeassignWorkerB(MessageForm message) {
            message.Addressee = MySim.FindAgent(SimId.AgentWorkersB);
            message.Code = Mc.DeassignWorkerB;
            Notice(message);
        }

        //meta! sender="AgentCarpentry", id="202", type="Notice"
        public void ProcessDeassignWorkerC(MessageForm message) {
            message.Addressee = MySim.FindAgent(SimId.AgentWorkersC);
            message.Code = Mc.DeassignWorkerC;
            Notice(message);
        }

        //meta! sender="AgentCarpentry", id="37", type="Notice"
        public void ProcessInit(MessageForm message) {
            // Initialization logic if needed
        }

        //meta! userInfo="Process messages defined in code", id="0"
        public void ProcessDefault(MessageForm message) {
            // Optional logging or handling unknown codes
        }

        //meta! userInfo="Generated code: do not modify", tag="begin"
        public void Init() {
        }

        override public void ProcessMessage(MessageForm message) {
            switch (message.Code) {
                case Mc.GetWorkerForAssembling:
                    ProcessGetWorkerForAssembling(message);
                    break;

                case Mc.GetWorkerForCutting:
                    ProcessGetWorkerForCutting(message);
                    break;

                case Mc.GetWorkerForPainting:
                    ProcessGetWorkerForPainting(message);
                    break;

                case Mc.GetWorkerForPickling:
                    ProcessGetWorkerForPickling(message);
                    break;

                case Mc.GetWorkerForMounting:
                    ProcessGetWorkerForMounting(message);
                    break;

                case Mc.GetWorkerA:
                    ProcessGetWorkerA(message);
                    break;

                case Mc.GetWorkerB:
                    ProcessGetWorkerB(message);
                    break;

                case Mc.GetWorkerC:
                    ProcessGetWorkerC(message);
                    break;

                case Mc.DeassignWorkerA:
                    ProcessDeassignWorkerA(message);
                    break;

                case Mc.DeassignWorkerB:
                    ProcessDeassignWorkerB(message);
                    break;

                case Mc.DeassignWorkerC:
                    ProcessDeassignWorkerC(message);
                    break;

                case Mc.Init:
                    ProcessInit(message);
                    break;

                default:
                    ProcessDefault(message);
                    break;
            }
        }
        //meta! tag="end"

        public new AgentWorkers MyAgent => (AgentWorkers)base.MyAgent;
    }
}
