using Agents.AgentScope;
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

        public override void PrepareReplication() {
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

        // Handles both worker and workplace acquisitions
        public void ProcessResourceAcquired(MessageForm message) {
            MyMessage myMessage = (MyMessage)message;
            if (myMessage.Order == null || myMessage.Product == null) return;

            var queue = GetQueueForProduct(myMessage.Product);
            var queued = queue.FirstOrDefault(q => q.Product?.Id == myMessage.Product.Id);
            if (queued == null) return;

            if (myMessage.Worker != null) queued.Worker = myMessage.Worker;
            if (myMessage.Workplace != null) queued.Workplace = myMessage.Workplace;

            CheckQueueAndProcess(queued);
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
            ProductState.Painted => Mc.GetWorkerForPickling,
            ProductState.Pickled => Mc.GetWorkerForAssembling,
            ProductState.Assembled => Mc.GetWorkerForMounting,
            _ => Mc.GetWorkerForCutting
        };

        // Central logic for checking readiness of worker and workplace for product
        private void CheckQueueAndProcess(MyMessage message) {
            if (message.Product == null) return;

            var managerScope = ((MySimulation)MySim).AgentScope.MyManager as ManagerScope;
            var matchedProduct = managerScope?.Products.FirstOrDefault(p => p.Id == message.Order?.Id);

            if (matchedProduct != null) {
                matchedProduct.State = message.Product.State;
            }

            var queue = GetQueueForProduct(message.Product);

            if (message.Worker != null && message.Workplace != null) {
                queue.Remove(message);

                message.Workplace.SetState(true);
                message.Product.Workplace = message.Workplace;
                message.Workplace.Product = message.Product;

                message.Code = Mc.MoveToWorkplace;
                message.Addressee = MySim.FindAgent(SimId.AgentMovement);
                Request(message);
            } else {
                if (message.Worker == null && !message.WorkerRequested) {
                    message.WorkerRequested = true;
                    Request(new MyMessage(message) {
                        Code = GetWorkerRequestCode(message.Product),
                        Addressee = MySim.FindAgent(SimId.AgentWorkers)
                    });
                }

                if (message.Workplace == null && !message.WorkplaceRequested) {
                    message.WorkplaceRequested = true;
                    Request(new MyMessage(message) {
                        Code = Mc.GetFreeWorkplace,
                        Addressee = MySim.FindAgent(SimId.AgentWorkplaces)
                    });
                }
            }
        }

        // Advances product through the process based on current state
        private void AdvanceProductState(Product product) {
            switch (product.State) {
                case ProductState.Raw:
                    product.State = ProductState.Cut;
                    break;
                case ProductState.Cut:
                    product.State = ProductState.Painted;
                    break;
                case ProductState.Painted:
                    product.State = ProductState.Pickled;
                    break;
                case ProductState.Pickled:
                    product.State = ProductState.Assembled;
                    break;
                case ProductState.Assembled:
                    product.State = product.Type == ProductType.Wardrobe ? ProductState.Mounted : ProductState.Finished;
                    break;
            }
        }

        //meta! sender="AgentMovement", id="55", type="Response"
        public void ProcessMoveToWorkplace(MessageForm message) {
            MyMessage myMessage = (MyMessage)message;

            if (myMessage.Worker?.Workplace != null) {
                myMessage.Code = Mc.MoveToStorage;
                myMessage.Addressee = MySim.FindAgent(SimId.AgentMovement);
            } else {
                myMessage.Code = Mc.DoPreparing;
                myMessage.Addressee = MySim.FindAgent(SimId.AgentWarehouse);
            }

            Request(myMessage);
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

            MyMessage msg = (MyMessage)message.CreateCopy();

            int code = msg.Worker?.Group switch {
                WorkerGroup.A => Mc.DeassignWorkerA,
                WorkerGroup.B => Mc.DeassignWorkerB,
                WorkerGroup.C => Mc.DeassignWorkerC,
                _ => -1
            };

            if (code != -1) {
                msg.Code = code;
                msg.Addressee = MySim.FindAgent(SimId.AgentWorkers);
                Notice(msg);
            }

            // Advance product state and reprocess if needed
            if (msg.Product != null) {
                AdvanceProductState(msg.Product);

                if (msg.Product.State != ProductState.Finished) {
                    MyMessage newMsg = new(MySim) {
                        Order = msg.Order,
                        Product = msg.Product
                    };

                    var queue = GetQueueForProduct(msg.Product);
                    queue.AddLast(newMsg);
                    CheckQueueAndProcess(newMsg);
                }
            }
        }

        //meta! sender="AgentModel", id="27", type="Notice"
        public void ProcessInit(MessageForm message) { }

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
                case Mc.GetFreeWorkplace:
                    ProcessResourceAcquired(message);
                    break;

                case Mc.MoveToWorkplace:
                    ProcessMoveToWorkplace(message);
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
