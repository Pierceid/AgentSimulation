using AgentSimulation.Structures.Enums;
using OSPABA;
using Simulation;

namespace Agents.AgentCarpentry {
    //meta! id="4"
    public class ManagerCarpentry : OSPABA.Manager {
        public ManagerCarpentry(int id, OSPABA.Simulation mySim, Agent myAgent) : base(id, mySim, myAgent) {
            Init();
        }

        override public void PrepareReplication() {
            base.PrepareReplication();
            if (PetriNet != null) {
                PetriNet.Clear();
            }
        }

        public void ProcessGetWorkerForCuttingAgentWorkplaces(MessageForm message) {
            // Request: Workplace needs a worker for cutting
            message.Addressee = MySim.FindAgent(SimId.AgentWorkers);
            message.Code = Mc.GetWorkerForCutting;
            Request(message);
        }

        public void ProcessGetWorkerForCuttingAgentWorkers(MessageForm message) {
            // Response: Got worker for cutting
            message.Addressee = MySim.FindAgent(SimId.AgentMovement);
            message.Code = Mc.MoveToWorkplace;
            Request(message);
        }

        public void ProcessGetWorkerForMountingAgentWorkplaces(MessageForm message) {
            // Request: Got worker for mounting
            message.Addressee = MySim.FindAgent(SimId.AgentWorkers);
            message.Code = Mc.GetWorkerForMounting;
            Request(message);
        }

        public void ProcessGetWorkerForMountingAgentWorkers(MessageForm message) {
            // Response: Got worker for mounting
            message.Addressee = MySim.FindAgent(SimId.AgentMovement);
            message.Code = Mc.MoveToWorkplace;
            Request(message);
        }

        public void ProcessProcessOrder(MessageForm message) {
            // Typically the first step in an order would be to request a free workplace
            message.Addressee = MySim.FindAgent(SimId.AgentWorkplaces);
            message.Code = Mc.GetFreeWorkplace;
            Request(message);
        }

        public void ProcessGetWorkerForAssemblingAgentWorkplaces(MessageForm message) {
            // Request: Got worker for assembling
            message.Addressee = MySim.FindAgent(SimId.AgentWorkers);
            message.Code = Mc.GetWorkerForAssembling;
            Request(message);
        }

        public void ProcessGetWorkerForAssemblingAgentWorkers(MessageForm message) {
            // Response: Got worker for assembling
            message.Addressee = MySim.FindAgent(SimId.AgentMovement);
            message.Code = Mc.MoveToWorkplace;
            Request(message);
        }

        public void ProcessMoveToWorkplace(MessageForm message) {
            // After reaching the workplace, send message to AgentWorkplaces to start task
            message.Addressee = MySim.FindAgent(SimId.AgentWorkplaces);
            message.Code = Mc.AssignWorkplace;
            Notice(message);
        }

        public void ProcessInit(MessageForm message) {

        }

        public void ProcessGetWorkerForPaintingAgentWorkers(MessageForm message) {
            message.Addressee = MySim.FindAgent(SimId.AgentMovement);
            message.Code = Mc.MoveToWorkplace;
            Request(message);
        }

        public void ProcessGetWorkerForPaintingAgentWorkplaces(MessageForm message) {
            message.Addressee = MySim.FindAgent(SimId.AgentWorkers);
            message.Code = Mc.GetWorkerForPainting;
            Request(message);
        }

        public void ProcessGetFreeWorkplace(MessageForm message) {
            // Got a workplace, request a worker for the order's current task
            var myMessage = (MyMessage)message;
            var state = myMessage.Order?.State;
            var rng = ((MySimulation)MySim).Generators.RNG.Next();

            switch (state) {
                case ProductState.Raw:
                    myMessage.Code = Mc.GetWorkerForCutting;
                    break;
                case ProductState.Cut:
                    myMessage.Code = Mc.GetWorkerForPainting;
                    break;
                case ProductState.Painted:
                    if (rng < 0.15) {
                        myMessage.Code = Mc.GetWorkerForPickling;
                    } else {
                        myMessage.Code = Mc.GetWorkerForAssembling;
                    }
                    break;
                case ProductState.Pickled:
                    myMessage.Code = Mc.GetWorkerForAssembling;
                    break;
                case ProductState.Assembled:
                    myMessage.Code = Mc.GetWorkerForMounting;
                    break;
                default:
                    break;
            }

            myMessage.Addressee = MySim.FindAgent(SimId.AgentWorkplaces);
            Request(myMessage);
        }

        public void ProcessAssignWorker(MessageForm message) {
            message.Addressee = MySim.FindAgent(SimId.AgentWorkplaces);
            message.Code = Mc.AssignWorker;
            Notice(message);
        }

        public void ProcessDoPreparing(MessageForm message) {
            var myMessage = (MyMessage)message;
            myMessage.Code = Mc.ProcessOrder;
            myMessage.Addressee = MyAgent;
            Notice(myMessage);
        }

        public void ProcessMoveToStorage(MessageForm message) {
            message.Addressee = MySim.FindAgent(SimId.AgentWarehouse);
            message.Code = Mc.MoveToStorage;
            Request(message);
        }

        public void ProcessGetWorkerForPicklingAgentWorkplaces(MessageForm message) {
            message.Addressee = MySim.FindAgent(SimId.AgentWorkers);
            message.Code = Mc.GetWorkerForPickling;
            Request(message);
        }

        public void ProcessGetWorkerForPicklingAgentWorkers(MessageForm message) {
            message.Addressee = MySim.FindAgent(SimId.AgentMovement);
            message.Code = Mc.MoveToWorkplace;
            Request(message);
        }

        public void ProcessDeassignWorkplace(MessageForm message) {
            var myMessage = (MyMessage)message;
            myMessage.Code = Mc.ProcessOrder;
            myMessage.Addressee = MyAgent;
            Notice(myMessage);
        }

        public void ProcessDefault(MessageForm message) {

        }

        //meta! userInfo="Generated code: do not modify", tag="begin"
        public void Init() {
        }

        override public void ProcessMessage(MessageForm message) {
            switch (message.Code) {
                case Mc.GetFreeWorkplace:
                    ProcessGetFreeWorkplace(message);
                    break;

                case Mc.AssignWorker:
                    ProcessAssignWorker(message);
                    break;

                case Mc.ProcessOrder:
                    ProcessProcessOrder(message);
                    break;

                case Mc.Init:
                    ProcessInit(message);
                    break;

                case Mc.GetWorkerForMounting:
                    switch (message.Sender.Id) {
                        case SimId.AgentWorkplaces:
                            ProcessGetWorkerForMountingAgentWorkplaces(message);
                            break;

                        case SimId.AgentWorkers:
                            ProcessGetWorkerForMountingAgentWorkers(message);
                            break;
                    }
                    break;

                case Mc.GetWorkerForAssembling:
                    switch (message.Sender.Id) {
                        case SimId.AgentWorkers:
                            ProcessGetWorkerForAssemblingAgentWorkers(message);
                            break;

                        case SimId.AgentWorkplaces:
                            ProcessGetWorkerForAssemblingAgentWorkplaces(message);
                            break;
                    }
                    break;

                case Mc.GetWorkerForPainting:
                    switch (message.Sender.Id) {
                        case SimId.AgentWorkers:
                            ProcessGetWorkerForPaintingAgentWorkers(message);
                            break;

                        case SimId.AgentWorkplaces:
                            ProcessGetWorkerForPaintingAgentWorkplaces(message);
                            break;
                    }
                    break;

                case Mc.GetWorkerForPickling:
                    switch (message.Sender.Id) {
                        case SimId.AgentWorkplaces:
                            ProcessGetWorkerForPicklingAgentWorkplaces(message);
                            break;

                        case SimId.AgentWorkers:
                            ProcessGetWorkerForPicklingAgentWorkers(message);
                            break;
                    }
                    break;

                case Mc.MoveToWorkplace:
                    ProcessMoveToWorkplace(message);
                    break;

                case Mc.MoveToStorage:
                    ProcessMoveToStorage(message);
                    break;

                case Mc.GetWorkerForCutting:
                    switch (message.Sender.Id) {
                        case SimId.AgentWorkplaces:
                            ProcessGetWorkerForCuttingAgentWorkplaces(message);
                            break;

                        case SimId.AgentWorkers:
                            ProcessGetWorkerForCuttingAgentWorkers(message);
                            break;
                    }
                    break;

                case Mc.DoPreparing:
                    ProcessDoPreparing(message);
                    break;

                case Mc.DeassignWorkplace:
                    ProcessDeassignWorkplace(message);
                    break;

                default:
                    ProcessDefault(message);
                    break;
            }
        }
        //meta! tag="end"

        public new AgentCarpentry MyAgent {
            get { return (AgentCarpentry)base.MyAgent; }
        }
    }
}