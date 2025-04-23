using AgentSimulation.Structures.Enums;
using AgentSimulation.Structures.Objects;
using OSPABA;
using OSPDataStruct;
using Simulation;

namespace Agents.AgentCarpentry {
    //meta! id="4"
    public class ManagerCarpentry : OSPABA.Manager {
        public SimQueue<MyMessage> QueueA { get; set; } = new();
        public SimQueue<MyMessage> QueueB { get; set; } = new();
        public SimQueue<MyMessage> QueueC { get; set; } = new();
        public SimQueue<MyMessage> QueueD { get; set; } = new();

        public ManagerCarpentry(int id, OSPABA.Simulation mySim, Agent myAgent) : base(id, mySim, myAgent) {
            Init();
        }

        public void Clear() {
            QueueA.Clear();
            QueueB.Clear();
            QueueC.Clear();
            QueueD.Clear();
        }

        override public void PrepareReplication() {
            base.PrepareReplication();

            PetriNet?.Clear();

            Clear();
        }

        //meta! sender="AgentModel", id="12", type="Request"
        public void ProcessProcessOrder(MessageForm message) {
            MyMessage myMessage = (MyMessage)message;

            if (myMessage.Order == null) return;


            foreach (var product in myMessage.Order.Products) {
                MyMessage productMessage = new(MySim) {
                    Order = myMessage.Order,
                    Product = product
                };

                GetQueueForProduct(product).AddLast(productMessage);
                CheckQueueAndProcess(productMessage);
            }
        }

        // Handles worker acquisition from AgentWorkers
        public void ProcessWorkerAcquired(MessageForm message) {
            MyMessage myMessage = (MyMessage)message;

            if (myMessage.Order == null || myMessage.Product == null) return;

            var queue = GetQueueForProduct(myMessage.Product);
            var queued = queue.FirstOrDefault(q => q.Product?.Id == myMessage.Product.Id && q.Worker == null);

            if (queued != null) {
                queued.Worker = myMessage.Worker;
                CheckQueueAndProcess(queued);
            }
        }

        // Handles workplace acquisition from AgentWorkplaces
        public void ProcessWorkplaceAcquired(MessageForm message) {
            MyMessage myMessage = (MyMessage)message;

            if (myMessage.Order == null || myMessage.Product == null) return;

            var queue = GetQueueForProduct(myMessage.Product);
            var queued = queue.FirstOrDefault(q => q.Product?.Id == myMessage.Product.Id && q.Workplace == null);

            if (queued != null) {
                queued.Workplace = myMessage.Workplace;
                CheckQueueAndProcess(queued);
            }
        }

        // Determines which queue to use based on product state
        private SimQueue<MyMessage> GetQueueForProduct(Product product) => product.State switch {
            ProductState.Raw => QueueA,
            ProductState.Cut => QueueC,
            ProductState.Painted => QueueC,
            ProductState.Pickled => QueueB,
            ProductState.Assembled => QueueD,
            _ => QueueA
        };

        // Determines correct worker request message code
        private int GetWorkerRequestCode(Product product) => product.State switch {
            ProductState.Raw => Mc.GetWorkerForCutting,
            ProductState.Cut => Mc.GetWorkerForPainting,
            ProductState.Painted => ((MySimulation)MySim).Generators.RNG.Next() < 0.15 ? Mc.GetWorkerForPickling : Mc.GetWorkerForAssembling,
            ProductState.Pickled => Mc.GetWorkerForAssembling,
            ProductState.Assembled => Mc.GetWorkerForMounting,
            _ => Mc.GetWorkerForCutting
        };

        // Central logic for checking readiness of worker and workplace for product
        private void CheckQueueAndProcess(MyMessage message) {
            if (message.Product == null) return;

            var queue = GetQueueForProduct(message.Product);

            // Proceed if both worker and workplace are assigned
            if (message.Worker != null && message.Workplace != null) {
                queue.Remove(message);

                message.Workplace.SetState(true);
                message.Product.Workplace = message.Workplace;
                message.Workplace.Product = message.Product;

                // Always move worker to the workplace before doing anything else
                message.Code = Mc.MoveToWorkplace;
                message.Addressee = MySim.FindAgent(SimId.AgentMovement);
                Request(message);
            } else {
                // Only request once per type to avoid flooding
                if (message.Worker == null && !message.WorkerRequested) {
                    message.WorkerRequested = true;
                    var workerReq = new MyMessage(message) {
                        Code = GetWorkerRequestCode(message.Product),
                        Addressee = MySim.FindAgent(SimId.AgentWorkers)
                    };
                    Request(workerReq);
                }

                if (message.Workplace == null && !message.WorkplaceRequested) {
                    message.WorkplaceRequested = true;
                    var workplaceReq = new MyMessage(message) {
                        Code = Mc.GetFreeWorkplace,
                        Addressee = MySim.FindAgent(SimId.AgentWorkplaces)
                    };
                    Request(workplaceReq);
                }
            }
        }

        //meta! sender="AgentModel", id="27", type="Notice"
        public void ProcessInit(MessageForm message) {

        }

        //meta! sender="AgentMovement", id="112", type="Response"
        public void ProcessMoveToStorage(MessageForm message) {
            Request(message);
        }

        //meta! sender="AgentWarehouse", id="140", type="Response"
        public void ProcessDoPreparing(MessageForm message) {
            Request(message);
        }

        //meta! userInfo="Removed from model"
        public void ProcessDeassignWorkplace(MessageForm message) {
            message.Code = Mc.DeassignWorkplace;
            message.Addressee = MySim.FindAgent(SimId.AgentWorkplaces);
            Notice(message);
        }

        //meta! userInfo="Process messages defined in code", id="0"
        public void ProcessDefault(MessageForm message) { }

        //meta! userInfo="Generated code: do not modify", tag="begin"
        public void Init() { }

        public override void ProcessMessage(MessageForm message) {
            switch (message.Code) {
                case Mc.ProcessOrder:
                    ProcessProcessOrder(message);
                    break;

                case Mc.GetWorkerForCutting:
                case Mc.GetWorkerForPainting:
                case Mc.GetWorkerForPickling:
                case Mc.GetWorkerForAssembling:
                case Mc.GetWorkerForMounting:
                    ProcessWorkerAcquired(message);
                    break;

                case Mc.GetFreeWorkplace:
                    ProcessWorkplaceAcquired(message);
                    break;

                case Mc.MoveToStorage:
                    ProcessMoveToStorage(message);
                    break;

                case Mc.DoPreparing:
                    ProcessDoPreparing(message);
                    break;

                case Mc.Init:
                    ProcessInit(message);
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

        public new AgentCarpentry MyAgent => (AgentCarpentry)base.MyAgent;
    }
}
