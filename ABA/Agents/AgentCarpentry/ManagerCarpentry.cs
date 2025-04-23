using Agents.AgentScope;
using AgentSimulation.Structures.Enums;
using AgentSimulation.Structures.Objects;
using OSPABA;
using OSPDataStruct;
using Simulation;

namespace Agents.AgentCarpentry {
    public class ManagerCarpentry : OSPABA.Manager {
        public SimQueue<MyMessage> QueueA { get; } = new();
        public SimQueue<MyMessage> QueueB { get; } = new();
        public SimQueue<MyMessage> QueueC { get; } = new();
        public SimQueue<MyMessage> QueueD { get; } = new();

        public ManagerCarpentry(int id, OSPABA.Simulation mySim, Agent myAgent) : base(id, mySim, myAgent) {
            Init();
        }

        public override void PrepareReplication() {
            base.PrepareReplication();
            PetriNet?.Clear();

            Clear();
        }

        public void Clear() {
            QueueA.Clear();
            QueueB.Clear();
            QueueC.Clear();
            QueueD.Clear();
        }

        // --- Core Processors ---
        public void ProcessProcessOrder(MessageForm message) {
            MyMessage myMessage = (MyMessage)message.CreateCopy();

            if (myMessage.Order == null) return;

            foreach (var product in myMessage.Order.Products) {
                var productMessage = new MyMessage(MySim) { Order = myMessage.Order, Product = product };
                var queue = GetQueueForProduct(product);
                queue.AddLast(productMessage);
                CheckQueueAndProcess(productMessage);
            }
        }

        public void ProcessResourceAcquired(MessageForm message) {
            MyMessage myMessage = (MyMessage)message.CreateCopy();

            if (myMessage.Order == null || myMessage.Product == null) return;

            var queue = GetQueueForProduct(myMessage.Product);
            var queued = queue.FirstOrDefault(q => q.Product?.Id == myMessage.Product.Id);

            if (queued == null) return;

            queued.Worker ??= myMessage.Worker;
            queued.Workplace ??= myMessage.Workplace;
            CheckQueueAndProcess(queued);
        }

        public void ProcessRequestWorker(MessageForm message) {
            MyMessage myMessage = (MyMessage)message.CreateCopy();

            int code = myMessage.Code switch {
                Mc.DoPreparing => Mc.DoPrepare,
                Mc.DoCutting => Mc.DoCut,
                Mc.DoPainting => Mc.DoPaint,
                Mc.DoPickling => Mc.DoPickle,
                Mc.DoAssembling => Mc.DoAssemble,
                Mc.DoMounting => Mc.DoMount,
                _ => -1
            };

            if (code != -1) {
                myMessage.Code = code;
                myMessage.Addressee = MySim.FindAgent(SimId.AgentWorkers);
                Request(message);
            }
        }

        public void ProcessResponseWorker(MessageForm message) {
            MyMessage myMessage = (MyMessage)message.CreateCopy();

            int code = myMessage.Code switch {
                Mc.DoPrepare => Mc.DoPreparing,
                Mc.DoCut => Mc.DoCutting,
                Mc.DoPaint => Mc.DoPainting,
                Mc.DoPickle => Mc.DoPickling,
                Mc.DoAssemble => Mc.DoAssembling,
                Mc.DoMount => Mc.DoMounting,
                _ => -1
            };

            if (code != -1) {
                myMessage.Code = code;
                myMessage.Addressee = MySim.FindAgent(SimId.AgentWorkplaces);
                Response(message);
            }
        }

        public void ProcessMoveToWorkplace(MessageForm message) {
            MyMessage myMessage = (MyMessage)message.CreateCopy();
            myMessage.Code = myMessage.Worker?.Workplace != null ? Mc.MoveToStorage : Mc.DoPreparing;
            myMessage.Addressee = MySim.FindAgent(myMessage.Code == Mc.MoveToStorage ? SimId.AgentMovement : SimId.AgentProcesses);
            Request(myMessage);
        }

        public void ProcessMoveToStorage(MessageForm message) => Request(message);

        public void ProcessDeassignWorkplace(MessageForm message) {
            MyMessage myMessage = (MyMessage)message.CreateCopy();
            myMessage.Code = Mc.DeassignWorkplace;
            myMessage.Addressee = MySim.FindAgent(SimId.AgentWorkplaces);
            Notice(myMessage);

            MyMessage copy = (MyMessage)myMessage.CreateCopy();
            copy.Code = copy.Worker?.Group switch {
                WorkerGroup.A => Mc.DeassignWorkerA,
                WorkerGroup.B => Mc.DeassignWorkerB,
                WorkerGroup.C => Mc.DeassignWorkerC,
                _ => -1
            };

            if (copy.Code != -1) {
                copy.Addressee = MySim.FindAgent(SimId.AgentWorkers);
                Notice(copy);
            }

            if (copy.Product == null) return;

            AdvanceProductState(copy.Product);

            if (copy.Product.State != ProductState.Finished) {
                var next = new MyMessage(MySim) { Order = copy.Order, Product = copy.Product };
                var queue = GetQueueForProduct(copy.Product);
                queue.AddLast(next);
                CheckQueueAndProcess(next);
            }
        }

        // --- Generalized Process Do Operations ---
        private void ProcessStartWorking(MessageForm message) {
            MyMessage myMessage = (MyMessage)message.CreateCopy();

            int code = myMessage.Code switch {
                Mc.DoPreparing => Mc.DoPrepare,
                Mc.DoCutting => Mc.DoCut,
                Mc.DoPainting => Mc.DoPaint,
                Mc.DoPickling => Mc.DoPickle,
                Mc.DoAssembling => Mc.DoAssemble,
                Mc.DoMounting => Mc.DoMount,
                _ => -1
            };

            if (code != -1) {
                myMessage.Code = code;
                myMessage.Addressee = MySim.FindAgent(SimId.AgentProcesses);
                Request(myMessage);
            }
        }

        private void ProcessFinishWorking(MessageForm message) {
            MyMessage myMessage = (MyMessage)message.CreateCopy();

            if (myMessage.Product == null) return;

            if (myMessage.Code != Mc.DoPreparing) AdvanceProductState(myMessage.Product);

            myMessage.Code = GetWorkerRequestCode(myMessage.Product);
            myMessage.Addressee = MySim.FindAgent(SimId.AgentWorkers);
            Request(myMessage);
        }

        // --- Helpers ---
        private SimQueue<MyMessage> GetQueueForProduct(Product product) => product.State switch {
            ProductState.Raw => QueueA,
            ProductState.Cut or ProductState.Painted => QueueC,
            ProductState.Pickled => QueueB,
            ProductState.Assembled => QueueD,
            _ => QueueA
        };

        private int GetWorkerRequestCode(Product product) => product.State switch {
            ProductState.Raw => Mc.GetWorkerForCutting,
            ProductState.Cut => Mc.GetWorkerForPainting,
            ProductState.Painted => Mc.GetWorkerForPickling,
            ProductState.Pickled => Mc.GetWorkerForAssembling,
            ProductState.Assembled => Mc.GetWorkerForMounting,
            _ => Mc.GetWorkerForCutting
        };

        private void CheckQueueAndProcess(MyMessage msg) {
            if (msg.Product == null) return;
            var queue = GetQueueForProduct(msg.Product);

            if (msg.Worker != null && msg.Workplace != null) {
                queue.Remove(msg);
                msg.Workplace.SetState(true);
                msg.Product.Workplace = msg.Workplace;
                msg.Workplace.Product = msg.Product;

                msg.Code = Mc.MoveToWorkplace;
                msg.Addressee = MySim.FindAgent(SimId.AgentMovement);
                Request(msg);
            } else {
                if (msg.Worker == null && !msg.WorkerRequested) {
                    msg.WorkerRequested = true;
                    Request(new MyMessage(msg) {
                        Code = GetWorkerRequestCode(msg.Product),
                        Addressee = MySim.FindAgent(SimId.AgentWorkers)
                    });
                }
                if (msg.Workplace == null && !msg.WorkplaceRequested) {
                    msg.WorkplaceRequested = true;
                    Request(new MyMessage(msg) {
                        Code = Mc.GetFreeWorkplace,
                        Addressee = MySim.FindAgent(SimId.AgentWorkplaces)
                    });
                }
            }
        }

        private void AdvanceProductState(Product product) {
            var managerScope = ((MySimulation)MySim).AgentScope.MyManager as ManagerScope;
            var matchProduct = managerScope?.Products.FirstOrDefault(p => p.Id == product.Id);

            if (matchProduct != null) {
                matchProduct.State = product.State switch {
                    ProductState.Raw => ProductState.Cut,
                    ProductState.Cut => ProductState.Painted,
                    ProductState.Painted => ProductState.Pickled,
                    ProductState.Pickled => ProductState.Assembled,
                    ProductState.Assembled => matchProduct.Type == ProductType.Wardrobe ? ProductState.Mounted : ProductState.Finished,
                    _ => matchProduct.State
                };
            }
        }

        public void ProcessDefault(MessageForm message) { }

        public void ProcessInit(MessageForm message) { }

        //meta! userInfo="Generated code: do not modify", tag="begin"
        public void Init() {
        }

        public override void ProcessMessage(MessageForm message) {
            switch (message.Code) {
                case Mc.ProcessOrder: ProcessProcessOrder(message); break;
                case Mc.MoveToWorkplace: ProcessMoveToWorkplace(message); break;
                case Mc.MoveToStorage: ProcessMoveToStorage(message); break;
                case Mc.Init: ProcessInit(message); break;

                case Mc.GetWorkerForCutting:
                case Mc.GetWorkerForPainting:
                case Mc.GetWorkerForPickling:
                case Mc.GetWorkerForAssembling:
                case Mc.GetWorkerForMounting:
                    ProcessRequestWorker(message); break;

                case Mc.GetWorkerToCut:
                case Mc.GetWorkerToPaint:
                case Mc.GetWorkerToPickle:
                case Mc.GetWorkerToAssemble:
                case Mc.GetWorkerToMount:
                    ProcessResponseWorker(message); break;

                case Mc.DoPreparing:
                case Mc.DoCutting:
                case Mc.DoPainting:
                case Mc.DoPickling:
                case Mc.DoAssembling:
                case Mc.DoMounting:
                    ProcessStartWorking(message); break;

                case Mc.DoPrepare:
                case Mc.DoCut:
                case Mc.DoPaint:
                case Mc.DoPickle:
                case Mc.DoAssemble:
                case Mc.DoMount:
                    ProcessFinishWorking(message); break;

                default:
                    ProcessDefault(message); break;
            }
        }

        public new AgentCarpentry MyAgent => (AgentCarpentry)base.MyAgent;
    }
}
