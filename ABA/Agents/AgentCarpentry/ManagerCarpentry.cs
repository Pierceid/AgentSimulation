using Agents.AgentScope;
using AgentSimulation.Structures.Entities;
using AgentSimulation.Structures.Enums;
using OSPABA;
using Simulation;
using System.Windows;
using System.Windows.Interop;

namespace Agents.AgentCarpentry {
    public class ManagerCarpentry : OSPABA.Manager {
        public LinkedList<MyMessage> QueueA { get; } = new();
        public LinkedList<MyMessage> QueueB { get; } = new();
        public LinkedList<MyMessage> QueueC { get; } = new();
        public LinkedList<MyMessage> QueueD { get; } = new();
        public List<Workplace> Workplaces { get; set; } = new();

        public ManagerCarpentry(int id, OSPABA.Simulation mySim, Agent myAgent) : base(id, mySim, myAgent) {
            Init();
        }

        public override void PrepareReplication() {
            base.PrepareReplication();
            PetriNet?.Clear();
            Clear();
        }

        public void ProcessOrder(MessageForm message) {
            var msg = (MyMessage)message.CreateCopy();

            if (msg.Order == null) return;

            foreach (var product in msg.Order.Products) {
                var productMsg = new MyMessage(MySim) {
                    Order = msg.Order,
                    Product = product
                };
                QueueA.AddLast(productMsg);
            }

            if (QueueA.Count > 0) {
                PlanCutting(QueueA.First());
            }
        }

        public void PlanCutting(MyMessage message) {
            if (message.GetWorkerForCutting() != null) {
                var workplace = GetFreeWorkplace();

                if (workplace == null) return;

                if (message.Product != null) {
                    workplace.Product = message.Product;
                    workplace.IsOccupied = true;
                    message.Workplace = workplace;
                    message.Product.Workplace = workplace;
                    DoCutting(message);
                }

                if (QueueA.Count > 0) {
                    PlanCutting(QueueA.First());
                }
                return;
            }

            message.Code = Mc.GetWorkerToCut;
            message.Addressee = MySim.FindAgent(SimId.AgentWorkers);
            Request(message.CreateCopy());
        }

        public void PlanPainting(MyMessage message) {
            message.Code = Mc.GetWorkerToPaint;
            message.Addressee = MySim.FindAgent(SimId.AgentWorkers);
            Request(message.CreateCopy());
        }

        public void PlanAssembling(MyMessage message) {
            message.Code = Mc.GetWorkerToAssemble;
            message.Addressee = MySim.FindAgent(SimId.AgentWorkers);
            Request(message.CreateCopy());
        }

        public void PlanMounting(MyMessage message) {
            message.Code = Mc.GetWorkerToMount;
            message.Addressee = MySim.FindAgent(SimId.AgentWorkers);
            Request(message.CreateCopy());
        }

        private void DoCutting(MyMessage message) {
            if (message.GetWorkerForCutting() == null || message.Workplace == null) {
                MessageBox.Show("Missing worker or workplace for cutting");
                throw new Exception();
            }

            var queued = QueueA.FirstOrDefault(m => m.Product?.Id == message.Product?.Id);

            if (queued != null) QueueA.Remove(queued);

            if (message.Product != null) message.Product.Workplace = message.Workplace;

            if (message.Workplace != null) {
                message.Workplace.Worker = message.GetWorkerForCutting();
                UpdateWorkplace(message);
            }

            var msg = new MyMessage(message);
            msg.GetWorkerForCutting()?.SetState(WorkerState.WORKING);
            msg.GetWorkerForCutting()?.Utility.AddSample(MySim.CurrentTime, false);
            msg.GetWorkerForCutting()?.Utility.AddSample(MySim.CurrentTime, true);
            var currentWorkplace = msg.GetWorkerForCutting()?.Workplace?.Id;

            if (currentWorkplace == null) {
                msg.Code = Mc.DoPrepare;
                msg.Addressee = MySim.FindAgent(SimId.AgentProcesses);
            } else {
                msg.Code = Mc.MoveToStorage;
                msg.Addressee = MySim.FindAgent(SimId.AgentMovement);
            }

            Request(msg);
        }

        private void DoPainting(MyMessage message) {
            if (message.GetWorkerForPainting() == null || message.Workplace == null) {
                MessageBox.Show("Missing worker or workplace for painting");
                throw new Exception();
            }

            var queued = QueueC.FirstOrDefault(m => m.Product?.Id == message.Product?.Id);

            if (queued != null) QueueC.Remove(queued);

            if (message.Workplace != null) {
                message.Workplace.Worker = message.GetWorkerForPainting();
                UpdateWorkplace(message);
            }

            var msg = new MyMessage(message);
            msg.GetWorkerForPainting()?.SetState(WorkerState.WORKING);
            msg.GetWorkerForPainting()?.Utility.AddSample(MySim.CurrentTime, false);
            msg.GetWorkerForPainting()?.Utility.AddSample(MySim.CurrentTime, true);
            var currentWorkplace = msg.GetWorkerForPainting()?.Workplace?.Id;
            var targetWorkplace = msg.Product?.Workplace?.Id;

            if (currentWorkplace != targetWorkplace) {
                msg.Code = Mc.MoveToWorkplace;
                msg.Addressee = MySim.FindAgent(SimId.AgentMovement);
            } else {
                msg.Code = Mc.DoPaint;
                msg.Addressee = MySim.FindAgent(SimId.AgentProcesses);
            }

            Request(msg);
        }

        private void DoPickling(MyMessage message) {
            if (message.GetWorkerForPickling() == null || message.Workplace == null) {
                MessageBox.Show("Missing worker or workplace for pickling");
                throw new Exception();
            }

            var msg = new MyMessage(message);
            msg.GetWorkerForPickling()?.SetState(WorkerState.WORKING);
            msg.GetWorkerForPickling()?.Utility.AddSample(MySim.CurrentTime, false);
            msg.GetWorkerForPickling()?.Utility.AddSample(MySim.CurrentTime, true);
            msg.Code = Mc.DoPickle;
            msg.Addressee = MySim.FindAgent(SimId.AgentProcesses);
            Request(msg);
        }

        private void DoAssembling(MyMessage message) {
            if (message.GetWorkerForAssembling() == null || message.Workplace == null) {
                MessageBox.Show("Missing worker or workplace for assembling");
                throw new Exception();
            }

            var queued = QueueB.FirstOrDefault(m => m.Product?.Id == message.Product?.Id);

            if (queued != null) QueueB.Remove(queued);

            if (message.Workplace != null) {
                message.Workplace.Worker = message.GetWorkerForAssembling();
                UpdateWorkplace(message);
            }

            var msg = new MyMessage(message);
            msg.GetWorkerForAssembling()?.SetState(WorkerState.WORKING);
            msg.GetWorkerForAssembling()?.Utility.AddSample(MySim.CurrentTime, false);
            msg.GetWorkerForAssembling()?.Utility.AddSample(MySim.CurrentTime, true);
            var currentWorkplace = msg.GetWorkerForAssembling()?.Workplace?.Id;
            var targetWorkplace = msg.Product?.Workplace?.Id;

            if (currentWorkplace != targetWorkplace) {
                msg.Code = Mc.MoveToWorkplace;
                msg.Addressee = MySim.FindAgent(SimId.AgentMovement);
            } else {
                msg.Code = Mc.DoAssemble;
                msg.Addressee = MySim.FindAgent(SimId.AgentProcesses);
            }

            Request(msg);
        }

        private void DoMounting(MyMessage message) {
            if (message.GetWorkerForMounting() == null || message.Workplace == null) {
                MessageBox.Show("Missing worker or workplace for mounting");
                throw new Exception();
            }

            var queued = QueueD.FirstOrDefault(m => m.Product?.Id == message.Product?.Id);

            if (queued != null) QueueD.Remove(queued);

            if (message.Workplace != null) {
                message.Workplace.Worker = message.GetWorkerForMounting();
                UpdateWorkplace(message);
            }

            var msg = new MyMessage(message);
            msg.GetWorkerForMounting()?.SetState(WorkerState.WORKING);
            msg.GetWorkerForMounting()?.Utility.AddSample(MySim.CurrentTime, false);
            msg.GetWorkerForMounting()?.Utility.AddSample(MySim.CurrentTime, true);
            var currentWorkplace = msg.GetWorkerForMounting()?.Workplace?.Id;
            var targetWorkplace = msg.Product?.Workplace?.Id;

            if (currentWorkplace != targetWorkplace) {
                msg.Code = Mc.MoveToWorkplace;
                msg.Addressee = MySim.FindAgent(SimId.AgentMovement);
            } else {
                msg.Code = Mc.DoMount;
                msg.Addressee = MySim.FindAgent(SimId.AgentProcesses);
            }

            Request(msg);
        }

        private void ReassignWorkerA(Worker worker) {
            if (QueueD.Count > 0) {
                var queuedD = QueueD.First();
                if (queuedD.Product == null) return;
                queuedD.Product.WorkerToMount = worker;
                DoMounting(queuedD);
                return;
            }

            if (QueueA.Count > 0) {
                var queuedA = QueueA.First();
                if (queuedA.Product == null) return;
                var freeWorkplace = GetFreeWorkplace();

                if (freeWorkplace == null) {
                    var message = new MyMessage(MySim) {
                        Code = Mc.DeassignWorkerA,
                        Addressee = MySim.FindAgent(SimId.AgentWorkers),
                        WorkerToRelease = worker
                    };
                    Notice(message);
                    return;
                }

                queuedA.Product.WorkerToCut = worker;
                queuedA.Workplace = freeWorkplace;
                DoCutting(queuedA);
                return;
            }

            var msg = new MyMessage(MySim) {
                Code = Mc.DeassignWorkerA,
                Addressee = MySim.FindAgent(SimId.AgentWorkers),
                WorkerToRelease = worker
            };
            Notice(msg);
        }

        private void ReassignWorkerB(Worker worker) {
            if (QueueB.Count > 0) {
                var queuedB = QueueB.First();
                if (queuedB.Product == null) return;
                queuedB.Product.WorkerToAssemble = worker;
                DoAssembling(queuedB);
                return;
            }

            var msg = new MyMessage(MySim) {
                Code = Mc.DeassignWorkerB,
                Addressee = MySim.FindAgent(SimId.AgentWorkers),
                WorkerToRelease = worker
            };
            Notice(msg);
        }

        private void ReassignWorkerC(Worker worker) {
            if (QueueD.Count > 0) {
                var queuedD = QueueD.First();
                if (queuedD.Product == null) return;
                queuedD.Product.WorkerToMount = worker;
                DoMounting(queuedD);
                return;
            }

            if (QueueC.Count > 0) {
                var queuedC = QueueC.First();
                if (queuedC.Product == null) return;
                queuedC.Product.WorkerToPaint = worker;
                DoPainting(queuedC);
                return;
            }

            var msg = new MyMessage(MySim) {
                Code = Mc.DeassignWorkerC,
                Addressee = MySim.FindAgent(SimId.AgentWorkers),
                WorkerToRelease = worker
            };
            Notice(msg);
        }

        private void ProcessFinishWorking(MessageForm message) {
            var msg = new MyMessage(message);

            switch (msg.Code) {
                case Mc.DoPrepare:
                    msg.GetWorkerForCutting()?.Utility.AddSample(MySim.CurrentTime, false);
                    msg.GetWorkerForCutting()?.Utility.AddSample(MySim.CurrentTime, true);
                    msg.GetWorkerForCutting()?.SetWorkplace(null);
                    msg.Code = Mc.MoveToWorkplace;
                    msg.Addressee = MySim.FindAgent(SimId.AgentMovement);
                    Request(msg);
                    break;
                case Mc.DoCut:
                    if (msg.Product != null) {
                        AdvanceProductState(msg.Product);

                        var workerCut = msg.GetWorkerForCutting();
                        msg.Product.WorkerToCut = null;
                        QueueC.AddLast(msg);
                        PlanPainting(QueueC.First());
                        if (workerCut != null) ReassignWorkerA(workerCut);
                    }
                    break;
                case Mc.DoPaint:
                    if (msg.Product != null) {
                        AdvanceProductState(msg.Product);

                        var workerPaint = msg.GetWorkerForPainting();
                        msg.Product.WorkerToPaint = null;
                        if (msg.Product.IsPickled) {
                            msg.Product.WorkerToPickle = workerPaint;
                            DoPickling(msg);
                        } else {
                            QueueB.AddLast(msg);
                            PlanAssembling(QueueB.First());
                            if (workerPaint != null) ReassignWorkerC(workerPaint);
                        }
                    }
                    break;
                case Mc.DoPickle:
                    if (msg.Product != null) {
                        AdvanceProductState(msg.Product);

                        var workerPickle = msg.GetWorkerForPickling();
                        msg.Product.WorkerToPickle = null;
                        QueueB.AddLast(msg);
                        PlanAssembling(QueueB.First());
                        if (workerPickle != null) ReassignWorkerC(workerPickle);
                    }
                    break;
                case Mc.DoAssemble:
                    if (msg.Product != null) {
                        AdvanceProductState(msg.Product);

                        var workerAssemble = msg.GetWorkerForAssembling();
                        msg.Product.WorkerToAssemble = null;
                        if (msg.Product.Type == ProductType.Wardrobe) {
                            QueueD.AddLast(msg);
                            PlanMounting(QueueD.First());
                        } else {
                            AdvanceProductState(msg.Product);

                            if (msg.Workplace != null) {
                                msg.Workplace.Clear();
                                UpdateWorkplace(msg);
                                ReleaseWorkplace(msg.Workplace);
                            }
                        }
                        if (workerAssemble != null) ReassignWorkerB(workerAssemble);
                    }
                    break;
                case Mc.DoMount:
                    if (msg.Product != null) {
                        AdvanceProductState(msg.Product);

                        var workerMount = msg.GetWorkerForMounting();
                        msg.Product.WorkerToMount = null;
                        if (workerMount != null) {
                            if (msg.Workplace != null) {
                                msg.Workplace.Clear();
                                UpdateWorkplace(msg);
                                ReleaseWorkplace(msg.Workplace);
                            }
                            if (workerMount.Group == WorkerGroup.A) {
                                ReassignWorkerA(workerMount);
                            } else if (workerMount.Group == WorkerGroup.C) {
                                ReassignWorkerC(workerMount);
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private void ProcessResponseWorkerToMount(MessageForm message) {
            var msg = new MyMessage(message);

            if (msg.GetWorkerForMounting() == null) return;

            DoMounting(msg);
        }

        private void ProcessResponseWorkerToAssemble(MessageForm message) {
            var msg = new MyMessage(message);

            if (msg.GetWorkerForAssembling() == null) return;

            DoAssembling(msg);
        }

        private void ProcessResponseWorkerToPickle(MessageForm message) {
            var msg = new MyMessage(message);

            if (msg.GetWorkerForPickling() == null) return;

            DoPickling(msg);
        }

        private void ProcessResponseWorkerToPaint(MessageForm message) {
            var msg = new MyMessage(message);

            if (msg.GetWorkerForPainting() == null) return;

            DoPainting(msg);
        }

        private void ProcessResponseWorkerToCut(MessageForm message) {
            var msg = new MyMessage(message);

            if (msg.GetWorkerForCutting() == null) return;

            var freeWorkplace = GetFreeWorkplace();

            if (freeWorkplace == null) {
                var returnMsg = new MyMessage(MySim) {
                    WorkerToRelease = msg.GetWorkerForCutting(),
                    Code = Mc.DeassignWorkerA,
                    Addressee = MySim.FindAgent(SimId.AgentWorkers)
                };
                if (msg.Product != null) msg.Product.WorkerToCut = null;
                Notice(returnMsg);
                return;
            }

            msg.Workplace = freeWorkplace;
            DoCutting(msg);

            if (QueueA.Count > 0) {
                PlanCutting(QueueA.First());
            }
        }

        private void ProcessRequestWorker(MessageForm message) {
            var msg = new MyMessage(message);

            switch (msg.Code) {
                case Mc.GetWorkerToCut:
                    ProcessResponseWorkerToCut(msg);
                    break;
                case Mc.GetWorkerToPaint:
                    ProcessResponseWorkerToPaint(msg);
                    break;
                case Mc.GetWorkerToPickle:
                    ProcessResponseWorkerToPickle(msg);
                    break;
                case Mc.GetWorkerToAssemble:
                    ProcessResponseWorkerToAssemble(msg);
                    break;
                case Mc.GetWorkerToMount:
                    ProcessResponseWorkerToMount(msg);
                    break;
                default:
                    break;
            }
        }

        private void ProcessMoveToStorage(MessageForm message) {
            message.Code = Mc.DoPrepare;
            message.Addressee = MySim.FindAgent(SimId.AgentProcesses);
            Request(message);
        }

        private void ProcessMoveToWorkplace(MessageForm message) {
            var msg = new MyMessage(message);

            if (msg.Product == null) return;

            if (msg.GetWorkerForCutting() != null) {
                msg.Code = Mc.DoCut;
            } else if (msg.GetWorkerForPainting() != null) {
                msg.Code = Mc.DoPaint;
            } else if (msg.GetWorkerForPickling() != null) {
                msg.Code = Mc.DoPickle;
            } else if (msg.GetWorkerForAssembling() != null) {
                msg.Code = Mc.DoAssemble;
            } else if (msg.GetWorkerForMounting() != null) {
                msg.Code = Mc.DoMount;
            } else {
                return;
            }

            var assignMsg = new MyMessage(msg);
            AssignWorkplace(assignMsg);

            msg.Addressee = MySim.FindAgent(SimId.AgentProcesses);
            Request(msg);
        }

        public void ProcessDefault(MessageForm message) { }
        public void ProcessInit(MessageForm message) { }
        public void Init() { }

        public override void ProcessMessage(MessageForm message) {
            try {
                switch (message.Code) {
                    case Mc.ProcessOrder: ProcessOrder(message); break;
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

                    case Mc.DoPrepare:
                    case Mc.DoCut:
                    case Mc.DoPaint:
                    case Mc.DoPickle:
                    case Mc.DoAssemble:
                    case Mc.DoMount:
                        ProcessFinishWorking(message); break;

                    default: ProcessDefault(message); break;
                }
            } catch (Exception ex) {
                MessageBox.Show($"Error: {ex.Message}");
                throw;
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
                lock (Workplaces) {
                    Workplaces.Add(new Workplace(i));
                }
            }
        }

        private void AdvanceProductState(Product product) {
            var managerScope = ((MySimulation)MySim).AgentScope.MyManager as ManagerScope;
            var orderMatch = managerScope?.Orders.FirstOrDefault(o => o.Id == product.Order.Id);

            if (orderMatch == null) return;

            var productMatch = orderMatch.Products.FirstOrDefault(p => p.Id == product.Id);

            if (productMatch == null) return;

            productMatch.State = product.State switch {
                ProductState.Raw => ProductState.Cut,
                ProductState.Cut => ProductState.Painted,
                ProductState.Painted => product.IsPickled ? ProductState.Pickled : ProductState.Assembled,
                ProductState.Pickled => ProductState.Assembled,
                ProductState.Assembled => ProductState.Finished,
                _ => productMatch.State
            };

            orderMatch.UpdateProduct(productMatch);

            if (orderMatch.State == "Completed") {
                var msg = new MyMessage(MySim) {
                    Order = orderMatch,
                    Code = Mc.ProcessOrder,
                    Addressee = MySim.FindAgent(SimId.AgentModel)
                };
                Notice(msg);
            }
        }

        private Workplace? GetFreeWorkplace() {
            lock (Workplaces) {
                var freeWorkplace = Workplaces.FirstOrDefault(wp => wp.Product == null && wp.Worker == null && !wp.IsOccupied);
                freeWorkplace?.SetState(true);
                return freeWorkplace;
            }
        }

        private void UpdateWorkplace(MyMessage message) {
            lock (Workplaces) {
                var matchedWorkplace = Workplaces.FirstOrDefault(wp => wp.Id == message.Workplace?.Id);
                if (matchedWorkplace != null && message.Workplace != null) {
                    matchedWorkplace.IsOccupied = message.Workplace.IsOccupied;
                    matchedWorkplace.Worker = message.Workplace.Worker;
                }
            }
        }

        private void ReleaseWorkplace(Workplace workplace) {
            lock (Workplaces) {
                if (workplace == null) return;

                var matchedWorkplace = Workplaces.FirstOrDefault(wp => wp.Id == workplace.Id);
                matchedWorkplace?.Clear();

                if (QueueA.Count > 0) {
                    var queuedA = QueueA.First();

                    if (queuedA != null && queuedA.GetWorkerForCutting() != null) {
                        QueueA.RemoveFirst();
                        matchedWorkplace?.SetState(true);
                        queuedA.Workplace = matchedWorkplace;
                        DoCutting(queuedA);
                        return;
                    }
                }
            }
        }

        private void AssignWorkplace(MyMessage msg) {
            var worker = msg.GetAssignedWorker();

            if (msg.Product == null || worker == null) return;

            switch (worker.Group) {
                case WorkerGroup.A:
                    msg.Code = Mc.AssignWorkerA;
                    break;
                case WorkerGroup.B:
                    msg.Code = Mc.AssignWorkerB;
                    break;
                case WorkerGroup.C:
                    msg.Code = Mc.AssignWorkerC;
                    break;
            }

            msg.Addressee = MySim.FindAgent(SimId.AgentWorkers);
            Notice(msg);
        }

        public new AgentCarpentry MyAgent => (AgentCarpentry)base.MyAgent;
    }
}
