using OSPABA;
using Simulation;
namespace Agents.AgentWorkplaces {
    //meta! id="39"
    public class ManagerWorkplaces : OSPABA.Manager {
        public ManagerWorkplaces(int id, OSPABA.Simulation mySim, Agent myAgent) : base(id, mySim, myAgent) {
            Init();
        }

        override public void PrepareReplication() {
            base.PrepareReplication();
            // Setup component for the next replication

            if (PetriNet != null) {
                PetriNet.Clear();
            }
        }

        //meta! sender="AgentCarpentry", id="115", type="Response"
        public void ProcessGetWorkerForCutting(MessageForm message) {
        }

        //meta! sender="AgentCarpentry", id="137", type="Request"
        public void ProcessDoPickling(MessageForm message) {
        }

        //meta! sender="AgentCarpentry", id="119", type="Response"
        public void ProcessGetWorkerForMounting(MessageForm message) {
        }

        //meta! sender="AgentCarpentry", id="72", type="Notice"
        public void ProcessAssignWorkplace(MessageForm message) {
        }

        //meta! sender="AgentCarpentry", id="138", type="Request"
        public void ProcessDoAssembling(MessageForm message) {
        }

        //meta! sender="AgentCarpentry", id="118", type="Response"
        public void ProcessGetWorkerForAssembling(MessageForm message) {
        }

        //meta! sender="AgentCarpentry", id="40", type="Notice"
        public void ProcessInit(MessageForm message) {
        }

        //meta! sender="AgentCarpentry", id="128", type="Request"
        public void ProcessGetFreeWorkplace(MessageForm message) {
        }

        //meta! sender="AgentCarpentry", id="117", type="Response"
        public void ProcessGetWorkerForPainting(MessageForm message) {
        }

        //meta! sender="AgentCarpentry", id="139", type="Request"
        public void ProcessDoMounting(MessageForm message) {
        }

        //meta! sender="AgentCarpentry", id="135", type="Request"
        public void ProcessDoCutting(MessageForm message) {
        }

        //meta! sender="AgentCarpentry", id="136", type="Request"
        public void ProcessDoPainting(MessageForm message) {
        }

        //meta! sender="Pickling", id="147", type="Finish"
        public void ProcessFinishPickling(MessageForm message) {
        }

        //meta! sender="Assembling", id="67", type="Finish"
        public void ProcessFinishAssembling(MessageForm message) {
        }

        //meta! sender="Painting", id="65", type="Finish"
        public void ProcessFinishPainting(MessageForm message) {
        }

        //meta! sender="Cutting", id="63", type="Finish"
        public void ProcessFinishCutting(MessageForm message) {
        }

        //meta! sender="Mounting", id="69", type="Finish"
        public void ProcessFinishMounting(MessageForm message) {
        }

        //meta! sender="AgentCarpentry", id="120", type="Response"
        public void ProcessGetWorkerForPickling(MessageForm message) {
        }

        //meta! userInfo="Process messages defined in code", id="0"
        public void ProcessDefault(MessageForm message) {
            switch (message.Code) {
            }
        }

        //meta! userInfo="Generated code: do not modify", tag="begin"
        public void Init() {
        }

        override public void ProcessMessage(MessageForm message) {
            switch (message.Code) {
                case Mc.DoPainting:
                    ProcessDoPainting(message);
                    break;

                case Mc.Init:
                    ProcessInit(message);
                    break;

                case Mc.DoMounting:
                    ProcessDoMounting(message);
                    break;

                case Mc.Finish:
                    switch (message.Sender.Id) {
                        case SimId.Pickling:
                            ProcessFinishPickling(message);
                            break;

                        case SimId.Assembling:
                            ProcessFinishAssembling(message);
                            break;

                        case SimId.Painting:
                            ProcessFinishPainting(message);
                            break;

                        case SimId.Cutting:
                            ProcessFinishCutting(message);
                            break;

                        case SimId.Mounting:
                            ProcessFinishMounting(message);
                            break;
                    }
                    break;

                case Mc.GetWorkerForCutting:
                    ProcessGetWorkerForCutting(message);
                    break;

                case Mc.GetWorkerForMounting:
                    ProcessGetWorkerForMounting(message);
                    break;

                case Mc.AssignWorkplace:
                    ProcessAssignWorkplace(message);
                    break;

                case Mc.DoAssembling:
                    ProcessDoAssembling(message);
                    break;

                case Mc.DoCutting:
                    ProcessDoCutting(message);
                    break;

                case Mc.DoPickling:
                    ProcessDoPickling(message);
                    break;

                case Mc.GetWorkerForPickling:
                    ProcessGetWorkerForPickling(message);
                    break;

                case Mc.GetFreeWorkplace:
                    switch (message.Sender.Id) {
                        case SimId.AgentCarpentry:
                            ProcessGetFreeWorkplace(message);
                            break;

                    }
                    break;

                case Mc.GetWorkerForAssembling:
                    ProcessGetWorkerForAssembling(message);
                    break;

                case Mc.GetWorkerForPainting:
                    ProcessGetWorkerForPainting(message);
                    break;

                default:
                    ProcessDefault(message);
                    break;
            }
        }
        //meta! tag="end"
        public new AgentWorkplaces MyAgent {
            get {
                return (AgentWorkplaces)base.MyAgent;
            }
        }
    }
}