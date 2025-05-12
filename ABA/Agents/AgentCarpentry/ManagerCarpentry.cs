using Agents.AgentScope;
using AgentSimulation.Structures.Enums;
using AgentSimulation.Structures.Objects;
using OSPABA;
using OSPDataStruct;
using Simulation;
using System.Windows;

namespace Agents.AgentCarpentry {
    public class ManagerCarpentry : OSPABA.Manager {
        public SimQueue<MyMessage> QueueA { get; } = new();
        public SimQueue<MyMessage> QueueB { get; } = new();
        public SimQueue<MyMessage> QueueC { get; } = new();
        public SimQueue<MyMessage> QueueD { get; } = new();
        public List<Workplace> Workplaces { get; set; } = new();

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
            InitWorkplaces(Workplaces.Count);
        }

        public void InitWorkplaces(int workplaces) {
            Workplaces.Clear();

            Parallel.For(0, workplaces, w => {
                lock (Workplaces) {
                    Workplaces.Add(new Workplace(w));
                }
            });
        }

        private SimQueue<MyMessage> GetQueueForProduct(Product product) => product.State switch {
            ProductState.Raw => QueueA,
            ProductState.Cut or ProductState.Painted => QueueC,
            ProductState.Pickled => QueueB,
            ProductState.Assembled => QueueD,
            _ => QueueA
        };

        private int GetWorkerRequestCode(Product product) => product.State switch {
            ProductState.Raw => Mc.GetWorkerToCut,
            ProductState.Cut => Mc.GetWorkerToPaint,
            ProductState.Painted => Mc.GetWorkerToPickle,
            ProductState.Pickled => Mc.GetWorkerToAssemble,
            ProductState.Assembled => Mc.GetWorkerToMount,
            _ => Mc.GetWorkerToCut
        };

        private void AdvanceProductState(Product product) {
            var managerScope = ((MySimulation)MySim).AgentScope.MyManager as ManagerScope;
            var match = managerScope?.Products.FirstOrDefault(p => p.Id == product.Id);
            if (match == null) return;

            match.State = product.State switch {
                ProductState.Raw => ProductState.Cut,
                ProductState.Cut => ProductState.Painted,
                ProductState.Painted => ProductState.Pickled,
                ProductState.Pickled => ProductState.Assembled,
                ProductState.Assembled => match.Type == ProductType.Wardrobe ? ProductState.Mounted : ProductState.Finished,
                ProductState.Mounted => ProductState.Finished,
                _ => match.State
            };
        }

        private Workplace? GetFreeWorkplace() {
            lock (Workplaces) {
                Workplace? workplace = Workplaces.FirstOrDefault(w => !w.IsOccupied);
                workplace?.SetState(true);
                return workplace;
            }
        }

        private void ReleaseWorkplace(Workplace workplace) {
            lock (Workplaces) {
                workplace.SetState(false);
            }
        }

        private void CheckQueueAndProcess(MyMessage message) {
            if (message.Product == null) return;

            var queue = GetQueueForProduct(message.Product);

            if (!queue.Contains(message)) {
                queue.AddLast(message);
            }

            if (message.Workplace == null) {
                var freeWorkplace = GetFreeWorkplace();
                if (freeWorkplace != null) {
                    message.Workplace = freeWorkplace;
                    message.Product.Workplace = freeWorkplace;
                }
            }

            if (message.Worker == null) {
                Request(new MyMessage(message) {
                    Code = GetWorkerRequestCode(message.Product),
                    Addressee = MySim.FindAgent(SimId.AgentWorkers)
                });
            }

            if (message.Workplace != null && message.Worker != null) {
                queue.Remove(message);
                message.Workplace.SetState(true);
                message.Workplace.Product = message.Product;
                message.Product.Workplace = message.Workplace;

                message.Code = Mc.MoveToWorkplace;
                message.Addressee = MySim.FindAgent(SimId.AgentMovement);
                Request(message);
            }
        }

        public void ProcessProcessOrder(MessageForm message) {
            var myMessage = (MyMessage)message;
            if (myMessage.Order == null) return;

            foreach (var product in myMessage.Order.Products) {
                var productMessage = new MyMessage(MySim) {
                    Order = myMessage.Order,
                    Product = product
                };
                CheckQueueAndProcess(productMessage);
            }
        }

        public void ProcessResourceAcquired(MessageForm message) {
            var myMessage = (MyMessage)message;

            if (myMessage.Product == null) return;

            var queue = GetQueueForProduct(myMessage.Product);
            var queued = queue.FirstOrDefault(q => q.Product?.Id == myMessage.Product.Id);

            if (queued == null) return;

            if (myMessage.Worker != null)
                queued.Worker = myMessage.Worker;

            if (myMessage.Workplace != null)
                queued.Workplace = myMessage.Workplace;

            CheckQueueAndProcess(queued);
        }

        public void ProcessRequestWorker(MessageForm message) {
            var myMessage = (MyMessage)message;
            int code = myMessage.Code switch {
                Mc.GetWorkerForCutting => Mc.GetWorkerToCut,
                Mc.GetWorkerForPainting => Mc.GetWorkerToPaint,
                Mc.GetWorkerForPickling => Mc.GetWorkerToPickle,
                Mc.GetWorkerForAssembling => Mc.GetWorkerToAssemble,
                Mc.GetWorkerForMounting => Mc.GetWorkerToMount,
                _ => -1
            };

            if (myMessage.Code != -1) {
                myMessage.Code = code;
                myMessage.Addressee = MySim.FindAgent(SimId.AgentWorkers);
                Request(myMessage);
            }
        }

        public void ProcessResponseWorker(MessageForm message) {
            var myMessage = (MyMessage)message;

            if (myMessage.Worker == null || myMessage.Product == null) return;

            myMessage.Code = Mc.MoveToStorage;
            myMessage.Addressee = MySim.FindAgent(SimId.AgentMovement);
            Request(myMessage);
        }

        public void ProcessMoveToWorkplace(MessageForm message) {
            AssignWorkplace(message);
            var myMessage = (MyMessage)message.CreateCopy();

            if (myMessage.Product == null) return;

            int code = myMessage.Product.State switch {
                ProductState.Raw => Mc.DoCut,
                ProductState.Cut => Mc.DoPaint,
                ProductState.Painted => Mc.DoPickle,
                ProductState.Pickled => Mc.DoAssemble,
                ProductState.Assembled => Mc.DoMount,
                _ => -1
            };

            if (code != -1) {
                myMessage.Code = code;
                myMessage.Addressee = MySim.FindAgent(SimId.AgentProcesses);
                Request(myMessage);
            }
        }

        public void ProcessMoveToStorage(MessageForm message) {
            var myMessage = (MyMessage)message.CreateCopy();
            myMessage.Workplace = null;
            myMessage.Addressee = MySim.FindAgent(SimId.AgentProcesses);
            myMessage.Code = Mc.DoPrepare;
            Request(myMessage);
        }

        public void AssignWorkplace(MessageForm message) {
            var myMessage = (MyMessage)message.CreateCopy();
            int code = myMessage.Worker?.Group switch {
                WorkerGroup.A => Mc.AssignWorkerA,
                WorkerGroup.B => Mc.AssignWorkerB,
                WorkerGroup.C => Mc.AssignWorkerC,
                _ => -1
            };

            if (code != -1) {
                myMessage.Code = code;
                myMessage.Addressee = MySim.FindAgent(SimId.AgentWorkers);
                Notice(myMessage);
            }
        }

        public void DeassignWorkplace(MessageForm message) {
            var myMessage = (MyMessage)message;
            if (myMessage.Workplace != null) {
                ReleaseWorkplace(myMessage.Workplace);
            }

            var code = myMessage.Worker?.Group switch {
                WorkerGroup.A => Mc.DeassignWorkerA,
                WorkerGroup.B => Mc.DeassignWorkerB,
                WorkerGroup.C => Mc.DeassignWorkerC,
                _ => -1
            };

            if (code != -1) {
                Notice(new MyMessage(myMessage) {
                    Code = code,
                    Addressee = MySim.FindAgent(SimId.AgentWorkers)
                });
            }

            if (myMessage.Product == null) return;

            var msg = (MyMessage)myMessage.CreateCopy();

            if (msg.Product == null) return;

            if (msg.Product.State != ProductState.Finished) {
                var next = new MyMessage(MySim) {
                    Order = msg.Order,
                    Product = msg.Product
                };
                CheckQueueAndProcess(next);
            }
        }

        private void ProcessStartWorking(MessageForm message) {
            var myMessage = (MyMessage)message;
            int code = myMessage.Code switch {
                Mc.DoPreparing => Mc.DoPrepare,
                Mc.DoCutting => Mc.DoCut,
                Mc.DoPainting => Mc.DoPaint,
                Mc.DoPickling => Mc.DoPickle,
                Mc.DoAssembling => Mc.DoAssemble,
                Mc.DoMounting => Mc.DoMount,
                _ => -1
            };

            if (myMessage.Code != -1) {
                myMessage.Code = code;
                myMessage.Addressee = MySim.FindAgent(SimId.AgentProcesses);
                Request(myMessage);
            }
        }

        private void ProcessFinishWorking(MessageForm message) {
            var myMessage = (MyMessage)message;
            if (myMessage.Product == null) return;

            if (myMessage.Code == Mc.DoPrepare) {
                myMessage.Code = Mc.MoveToWorkplace;
                myMessage.Addressee = MySim.FindAgent(SimId.AgentMovement);
                Request(myMessage);
                return;
            }

            AdvanceProductState(myMessage.Product);

            var next = new MyMessage(myMessage);
            CheckQueueAndProcess(next);

            DeassignWorkplace(myMessage.CreateCopy());
        }

        public void ProcessDefault(MessageForm message) { }
        public void ProcessInit(MessageForm message) { }
        public void Init() { }

        public override void ProcessMessage(MessageForm message) {
            try {
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
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        public new AgentCarpentry MyAgent => (AgentCarpentry)base.MyAgent;
    }
}
