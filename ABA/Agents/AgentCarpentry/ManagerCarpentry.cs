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

        public void ProcessProcessOrder(MessageForm message) {
            var msg = (MyMessage)message;

            if (msg.Order == null) return;

            foreach (var product in msg.Order.Products) {
                var productMsg = new MyMessage(MySim) {
                    Order = msg.Order,
                    Product = product
                };

                QueueA.AddLast(productMsg);
                AssignOrderToWorkplace(productMsg);
            }
        }

        public void AssignOrderToWorkplace(MyMessage msg) {
            if (msg.Product == null || msg.Workplace != null) return;

            var freeWorkplace = GetFreeWorkplace();

            if (freeWorkplace == null) return;

            freeWorkplace.AssignProduct(msg.Product);
            msg.Workplace = freeWorkplace;
            msg.Product.Workplace = freeWorkplace;

            msg.Code = Mc.GetWorkerToCut;
            msg.Addressee = MySim.FindAgent(SimId.AgentWorkers);
            Request(msg);
        }

        public void ContinueWorkingOnProduct(MyMessage msg) {
            if (msg.Product == null) return;

            int code = GetWorkerRequestCode(msg.Product);
            if (code == -1) return;

            msg.Code = code;
            msg.Addressee = MySim.FindAgent(SimId.AgentWorkers);
            Request(msg);
        }

        public void ProcessRequestWorker(MessageForm message) {
            var msg = (MyMessage)message;
            int code = msg.Code switch {
                Mc.GetWorkerForCutting => Mc.GetWorkerToCut,
                Mc.GetWorkerForPainting => Mc.GetWorkerToPaint,
                Mc.GetWorkerForPickling => Mc.GetWorkerToPickle,
                Mc.GetWorkerForAssembling => Mc.GetWorkerToAssemble,
                Mc.GetWorkerForMounting => Mc.GetWorkerToMount,
                _ => -1
            };

            if (code != -1) {
                msg.Code = code;
                msg.Addressee = MySim.FindAgent(SimId.AgentWorkers);
                Request(msg);
            }
        }

        public void ProcessResponseWorkerToCut(MessageForm msg) => ProcessResponseWorker(msg, QueueA, Mc.MoveToStorage);

        public void ProcessResponseWorkerToPaint(MessageForm msg) => ProcessResponseWorker(msg, QueueC, Mc.MoveToWorkplace);

        public void ProcessResponseWorkerToPickle(MessageForm msg) => ProcessResponseWorker(msg, QueueC, Mc.MoveToWorkplace);

        public void ProcessResponseWorkerToAssemble(MessageForm msg) => ProcessResponseWorker(msg, QueueB, Mc.MoveToWorkplace);

        public void ProcessResponseWorkerToMount(MessageForm msg) => ProcessResponseWorker(msg, QueueD, Mc.MoveToWorkplace);

        private void ProcessResponseWorker(MessageForm message, SimQueue<MyMessage> queue, int moveCode) {
            var msg = (MyMessage)message;

            if (msg.Worker == null) return;

            queue.Remove(msg);
            msg.Worker.State = WorkerState.MOVING;

            AssignWorkplace(msg.CreateCopy());

            msg.Code = moveCode;
            msg.Addressee = MySim.FindAgent(SimId.AgentMovement);
            Request(msg);
        }

        public void ProcessMoveToStorage(MessageForm message) {
            var msg = (MyMessage)message;

            if (msg.Worker != null) msg.Worker.State = WorkerState.WORKING;

            AssignWorkplace(msg.CreateCopy());

            msg.Code = Mc.DoPrepare;
            msg.Addressee = MySim.FindAgent(SimId.AgentProcesses);
            Request(msg.CreateCopy());
        }

        public void ProcessMoveToWorkplace(MessageForm message) {
            var msg = (MyMessage)message;

            if (msg.Product == null) return;

            int code = msg.Product.State switch {
                ProductState.Raw => Mc.DoCut,
                ProductState.Cut => Mc.DoPaint,
                ProductState.Painted => msg.Product.IsPickled ? Mc.DoPickle : Mc.DoAssemble,
                ProductState.Pickled => Mc.DoAssemble,
                ProductState.Assembled => msg.Product.Type == ProductType.Wardrobe ? Mc.DoMount : Mc.Finish,
                _ => -1
            };

            if (msg.Worker != null) msg.Worker.State = WorkerState.WORKING;

            AssignWorkplace(msg.CreateCopy());

            if (code == Mc.Finish) {
                AdvanceOrderState(msg.CreateCopy());
                DeassignWorkplace(msg.CreateCopy());
            } else if (code != -1) {
                msg.Code = code;
                msg.Addressee = MySim.FindAgent(SimId.AgentProcesses);
                Request(msg.CreateCopy());
            }
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
            for (int i = 0; i < workplaces; i++) {
                Workplaces.Add(new Workplace(i));
            }
        }

        private void ProcessStartWorking(MessageForm message) {
            var msg = (MyMessage)message;

            int code = msg.Code switch {
                Mc.DoPreparing => Mc.DoPrepare,
                Mc.DoCutting => Mc.DoCut,
                Mc.DoPainting => Mc.DoPaint,
                Mc.DoPickling => Mc.DoPickle,
                Mc.DoAssembling => Mc.DoAssemble,
                Mc.DoMounting => Mc.DoMount,
                _ => -1
            };

            if (code != -1) {
                msg.Code = code;
                msg.Addressee = MySim.FindAgent(SimId.AgentProcesses);
                Request(msg);
            }
        }

        private void ProcessFinishWorking(MessageForm message) {
            var msg = (MyMessage)message;

            if (msg.Product == null) return;

            if (msg.Code == Mc.DoPrepare) {
                if (QueueA.Count > 0) {
                    var queued = QueueA.FirstOrDefault(m => m.Product?.Id == msg.Product?.Id);
                    if (queued != null) QueueA.Remove(queued);
                }

                msg.Code = Mc.MoveToWorkplace;
                msg.Addressee = MySim.FindAgent(SimId.AgentMovement);
                Request(msg.CreateCopy());
                return;
            }

            if (QueueA.Count > 0) {
                AssignOrderToWorkplace(QueueA.First());
            }

            AdvanceProductState(msg.Product);

            if (msg.Product.Type != ProductType.Wardrobe && msg.Product.State == ProductState.Assembled) {
                AdvanceProductState(msg.Product);
            }

            DeassignWorkplace(msg.CreateCopy());

            if (msg.Product.State == ProductState.Finished) {
                AdvanceOrderState(msg.CreateCopy());
                if (msg.Workplace != null) ReleaseWorkplace(msg.Workplace);
            }

            var nextMessage = new MyMessage(msg);
            var currentQueue = GetQueueForProduct(msg.Product);
            var nextQueue = GetQueueForProduct(msg.Product);

            if (nextQueue != null) {
                nextQueue.AddLast(nextMessage);
                if (currentQueue != null) RemoveMessageFromQueue(currentQueue, msg);
                ContinueWorkingOnProduct(nextMessage);
            }
        }

        private void RemoveMessageFromQueue(SimQueue<MyMessage> queue, MyMessage message) {
            var match = queue.FirstOrDefault(m => m.Product?.Id == message.Product?.Id);
            if (match != null) queue.Remove(match);
        }

        private SimQueue<MyMessage>? GetQueueForProduct(Product product) => product.State switch {
            ProductState.Raw => QueueA,
            ProductState.Cut => QueueC,
            ProductState.Painted => product.IsPickled ? QueueC : QueueB,
            ProductState.Pickled => QueueB,
            ProductState.Assembled => product.Type == ProductType.Wardrobe ? QueueD : null,
            _ => null
        };

        private int GetWorkerRequestCode(Product product) => product.State switch {
            ProductState.Raw => Mc.GetWorkerToCut,
            ProductState.Cut => Mc.GetWorkerToPaint,
            ProductState.Painted => product.IsPickled ? Mc.GetWorkerToPickle : Mc.GetWorkerToAssemble,
            ProductState.Pickled => Mc.GetWorkerToAssemble,
            ProductState.Assembled => Mc.GetWorkerToMount,
            _ => -1
        };

        private void AdvanceProductState(Product product) {
            var managerScope = ((MySimulation)MySim).AgentScope.MyManager as ManagerScope;
            var match = managerScope?.Products.FirstOrDefault(p => p.Id == product.Id);

            if (match == null) return;

            match.State = product.State switch {
                ProductState.Raw => ProductState.Cut,
                ProductState.Cut => ProductState.Painted,
                ProductState.Painted => product.IsPickled ? ProductState.Pickled : ProductState.Assembled,
                ProductState.Pickled => ProductState.Assembled,
                ProductState.Assembled => ProductState.Finished,
                _ => match.State
            };
        }

        private void AdvanceOrderState(MessageForm message) {
            var msg = (MyMessage)message;

            if (msg.Order == null || msg.Product == null) return;

            var managerScope = ((MySimulation)MySim).AgentScope.MyManager as ManagerScope;
            var match = managerScope?.Orders.FirstOrDefault(o => o.Id == msg.Order.Id);
            if (match == null) return;

            match.UpdateProduct(msg.Product);

            if (match.State == "Completed") {
                msg.Code = Mc.ProcessOrder;
                msg.Addressee = MySim.FindAgent(SimId.AgentModel);
                Notice(msg);
            }
        }

        private Workplace? GetFreeWorkplace() {
            lock (Workplaces) {
                var free = Workplaces.FirstOrDefault(w => !w.IsOccupied && w.Product == null && w.Worker == null);
                free?.SetState(true);
                return free;
            }
        }

        private void AssignWorkplace(MessageForm message) {
            var msg = (MyMessage)message;

            if (msg.Workplace != null) {
                var workplace = Workplaces.FirstOrDefault(w => w.Id == msg.Workplace.Id);
                if (workplace != null) workplace.Worker = msg.Worker;

                msg.Workplace.Worker = msg.Worker;
            }

            int code = msg.Worker?.Group switch {
                WorkerGroup.A => Mc.AssignWorkerA,
                WorkerGroup.B => Mc.AssignWorkerB,
                WorkerGroup.C => Mc.AssignWorkerC,
                _ => -1
            };

            if (code != -1) {
                msg.Code = code;
                msg.Addressee = MySim.FindAgent(SimId.AgentWorkers);
                Notice(msg);
            }
        }

        private void DeassignWorkplace(MessageForm message) {
            var msg = (MyMessage)message;

            if (msg.Workplace != null) {
                FreeUpWorkPlace(msg.Workplace);
                msg.Workplace.Product = null;
                msg.Workplace = null;
            }

            if (msg.Worker != null) msg.Worker.State = WorkerState.WAITING;

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
        }

        private void FreeUpWorkPlace(Workplace workplace) {
            lock (Workplaces) {
                workplace.IsOccupied = false;
                workplace.AssignWorker(null);
            }
        }

        private void ReleaseWorkplace(Workplace workplace) {
            lock (Workplaces) {
                workplace.SetState(false);
            }
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

                    case Mc.GetWorkerToCut: ProcessResponseWorkerToCut(message); break;
                    case Mc.GetWorkerToPaint: ProcessResponseWorkerToPaint(message); break;
                    case Mc.GetWorkerToPickle: ProcessResponseWorkerToPickle(message); break;
                    case Mc.GetWorkerToAssemble: ProcessResponseWorkerToAssemble(message); break;
                    case Mc.GetWorkerToMount: ProcessResponseWorkerToMount(message); break;

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
                MessageBox.Show($"Error: {ex.Message}");
                throw;
            }
        }

        public new AgentCarpentry MyAgent => (AgentCarpentry)base.MyAgent;
    }
}
