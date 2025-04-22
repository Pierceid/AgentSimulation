using AgentSimulation.Structures.Enums;
using OSPABA;
using OSPDataStruct;
using Simulation;

namespace Agents.AgentCarpentry {
    //meta! id="4"
    public class ManagerCarpentry : OSPABA.Manager {
        public SimQueue<MyMessage> QueueA { get; set; }
        public SimQueue<MyMessage> QueueB { get; set; }
        public SimQueue<MyMessage> QueueC { get; set; }
        public SimQueue<MyMessage> QueueD { get; set; }

        public ManagerCarpentry(int id, OSPABA.Simulation mySim, Agent myAgent) : base(id, mySim, myAgent) {
            Init();
        }

        override public void PrepareReplication() {
            base.PrepareReplication();
            PetriNet?.Clear();
        }

        // Worker request forwarding (like ObsluhaZakaznika)
        public void ForwardWorkerRequest(MessageForm message, int code) {
            message.Addressee = MySim.FindAgent(SimId.AgentWorkers);
            message.Code = code;
            Request(message);
        }

        // Worker acquired → forward response (like ObsluhaZakaznikaJedalen)
        public void ProcessWorkerAcquired(MessageForm message, int code) {
            message.Code = code;
            Response(message);
        }

        //meta! sender="AgentWorkplaces", id="115", type="Response"
        public void ProcessGetWorkerForCuttingAgentWorkplaces(MessageForm message) {
            ForwardWorkerRequest(message, Mc.GetWorkerForCutting);
        }

        //meta! sender="AgentWorkers", id="57", type="Response"
        public void ProcessGetWorkerForCuttingAgentWorkers(MessageForm message) {
            ProcessWorkerAcquired(message, Mc.GetWorkerForCutting);
        }

        //meta! sender="AgentWorkplaces", id="119", type="Response"
        public void ProcessGetWorkerForMountingAgentWorkplaces(MessageForm message) {
            ForwardWorkerRequest(message, Mc.GetWorkerForMounting);
        }

        //meta! sender="AgentWorkers", id="124", type="Response"
        public void ProcessGetWorkerForMountingAgentWorkers(MessageForm message) {
            ProcessWorkerAcquired(message, Mc.GetWorkerForMounting);
        }

        //meta! sender="AgentWorkplaces", id="118", type="Response"
        public void ProcessGetWorkerForAssemblingAgentWorkplaces(MessageForm message) {
            ForwardWorkerRequest(message, Mc.GetWorkerForAssembling);
        }

        //meta! sender="AgentWorkers", id="122", type="Response"
        public void ProcessGetWorkerForAssemblingAgentWorkers(MessageForm message) {
            ProcessWorkerAcquired(message, Mc.GetWorkerForAssembling);
        }

        //meta! sender="AgentWorkplaces", id="117", type="Response"
        public void ProcessGetWorkerForPaintingAgentWorkplaces(MessageForm message) {
            ForwardWorkerRequest(message, Mc.GetWorkerForPainting);
        }

        //meta! sender="AgentWorkers", id="121", type="Response"
        public void ProcessGetWorkerForPaintingAgentWorkers(MessageForm message) {
            ProcessWorkerAcquired(message, Mc.GetWorkerForPainting);
        }

        //meta! sender="AgentWorkplaces", id="120", type="Response"
        public void ProcessGetWorkerForPicklingAgentWorkplaces(MessageForm message) {
            ForwardWorkerRequest(message, Mc.GetWorkerForPickling);
        }

        //meta! sender="AgentWorkers", id="123", type="Response"
        public void ProcessGetWorkerForPicklingAgentWorkers(MessageForm message) {
            message.Addressee = MySim.FindAgent(SimId.AgentMovement);
            message.Code = Mc.MoveToWorkplace;
            Request(message);
        }

        //meta! sender="AgentModel", id="12", type="Request"
        public void ProcessProcessOrder(MessageForm message) {

        }

        //meta! sender="AgentWorkplaces", id="170", type="Request"
        public void ProcessGetFreeWorkplace(MessageForm message) {
            var myMessage = (MyMessage)message;
            var state = myMessage.Product?.State;
            var rng = ((MySimulation)MySim).Generators.RNG.Next();

            switch (state) {
                case ProductState.Raw:
                    myMessage.Code = Mc.GetWorkerForCutting;
                    break;
                case ProductState.Cut:
                    myMessage.Code = Mc.GetWorkerForPainting;
                    break;
                case ProductState.Painted:
                    myMessage.Code = rng < 0.15 ? Mc.GetWorkerForPickling : Mc.GetWorkerForAssembling;
                    break;
                case ProductState.Pickled:
                    myMessage.Code = Mc.GetWorkerForAssembling;
                    break;
                case ProductState.Assembled:
                    myMessage.Code = Mc.GetWorkerForMounting;
                    break;
                default:
                    return;
            }

            myMessage.Addressee = MySim.FindAgent(SimId.AgentWorkplaces);
            Request(myMessage);
        }

        //meta! sender="AgentModel", id="27", type="Notice"
        public void ProcessInit(MessageForm message) {

        }

        //meta! userInfo="Removed from model"
        public void ProcessAssignWorker(MessageForm message) {

        }

        //meta! sender="AgentWarehouse", id="140", type="Response"
        public void ProcessDoPreparing(MessageForm message) {
            message.Addressee = MySim.FindAgent(SimId.AgentWarehouse);
            message.Code = Mc.DoPreparing;
            Request(message);
        }

        //meta! sender="AgentMovement", id="112", type="Response"
        public void ProcessMoveToStorage(MessageForm message) {
            message.Addressee = MySim.FindAgent(SimId.AgentWarehouse);
            message.Code = Mc.MoveToStorage;
            Request(message);
        }

        //meta! sender="AgentMovement", id="55", type="Response"
        public void ProcessMoveToWorkplace(MessageForm message) {
            message.Code = Mc.MoveToWorkplace;
            Response(message);
        }

        //meta! userInfo="Removed from model"
        public void ProcessDeassignWorkplace(MessageForm message) {
            message.Code = Mc.DeassignWorkplace;
            message.Addressee = MySim.FindAgent(SimId.AgentWorkplaces);
            Notice(message);
        }

        //meta! userInfo="Process messages defined in code", id="0"
        public void ProcessDefault(MessageForm message) {

        }

        //meta! userInfo="Generated code: do not modify", tag="begin"
        public void Init() {
        }

        override public void ProcessMessage(MessageForm message) {
            switch (message.Code) {
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

                case Mc.GetFreeWorkplace:
                    ProcessGetFreeWorkplace(message);
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

                case Mc.MoveToWorkplace:
                    ProcessMoveToWorkplace(message);
                    break;

                case Mc.GetWorkerForAssembling:
                    switch (message.Sender.Id) {
                        case SimId.AgentWorkplaces:
                            ProcessGetWorkerForAssemblingAgentWorkplaces(message);
                            break;

                        case SimId.AgentWorkers:
                            ProcessGetWorkerForAssemblingAgentWorkers(message);
                            break;
                    }
                    break;

                case Mc.DoPreparing:
                    ProcessDoPreparing(message);
                    break;

                case Mc.GetWorkerForMounting:
                    switch (message.Sender.Id) {
                        case SimId.AgentWorkers:
                            ProcessGetWorkerForMountingAgentWorkers(message);
                            break;

                        case SimId.AgentWorkplaces:
                            ProcessGetWorkerForMountingAgentWorkplaces(message);
                            break;
                    }
                    break;

                case Mc.Init:
                    ProcessInit(message);
                    break;

                case Mc.GetWorkerForPainting:
                    switch (message.Sender.Id) {
                        case SimId.AgentWorkplaces:
                            ProcessGetWorkerForPaintingAgentWorkplaces(message);
                            break;

                        case SimId.AgentWorkers:
                            ProcessGetWorkerForPaintingAgentWorkers(message);
                            break;
                    }
                    break;

                case Mc.ProcessOrder:
                    ProcessProcessOrder(message);
                    break;

                default:
                    ProcessDefault(message);
                    break;
            }
        }
        //meta! tag="end"

        public new AgentCarpentry MyAgent => (AgentCarpentry)base.MyAgent;
    }
}