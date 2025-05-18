using AgentSimulation.Structures.Enums;
using OSPABA;
using Simulation;
using System.Windows;

namespace Agents.AgentWorkers {
    public class ManagerWorkers : OSPABA.Manager {
        public ManagerWorkers(int id, OSPABA.Simulation mySim, Agent myAgent) : base(id, mySim, myAgent) {
            Init();
        }

        override public void PrepareReplication() {
            base.PrepareReplication();
            PetriNet?.Clear();
        }

        public void ProcessGetWorkerForCutting(MessageForm message) {
            message.Addressee = MySim.FindAgent(SimId.AgentWorkersA);
            message.Code = Mc.GetWorkerA;
            Request(message);
        }

        public void ProcessGetWorkerForPainting(MessageForm message) {
            message.Addressee = MySim.FindAgent(SimId.AgentWorkersC);
            message.Code = Mc.GetWorkerC;
            Request(message);
        }

        public void ProcessGetWorkerForMounting(MessageForm message) {
            message.Addressee = MySim.FindAgent(SimId.AgentWorkersC);
            message.Code = Mc.GetWorkerC;
            Request(message);
        }

        public void ProcessGetWorkerForAssembling(MessageForm message) {
            message.Addressee = MySim.FindAgent(SimId.AgentWorkersB);
            message.Code = Mc.GetWorkerB;
            Request(message);
        }

        public void ProcessGetWorkerForPickling(MessageForm message) {
            message.Addressee = MySim.FindAgent(SimId.AgentWorkersC);
            message.Code = Mc.GetWorkerC;
            Request(message);
        }

        public void ProcessGetWorkerA(MessageForm message) {
            var msg = new MyMessage(message);
            msg.Addressee = MySim.FindAgent(SimId.AgentCarpentry);
            msg.Code = msg.GetAssignedWorker()?.Product?.State == ProductState.Raw ? Mc.GetWorkerToCut : Mc.GetWorkerToMount;
            Response(msg);
        }

        public void ProcessGetWorkerB(MessageForm message) {
            var msg = new MyMessage(message);
            msg.Addressee = MySim.FindAgent(SimId.AgentCarpentry);
            msg.Code = Mc.GetWorkerToAssemble;
            Response(msg);
        }

        public void ProcessGetWorkerC(MessageForm message) {
            var msg = new MyMessage(message);
            msg.Addressee = MySim.FindAgent(SimId.AgentCarpentry);
            msg.Code = msg.GetAssignedWorker()?.Product?.State == ProductState.Cut ? Mc.GetWorkerToPaint : Mc.GetWorkerToMount;
            Response(msg);
        }

        public void ProcessDeassignWorkerA(MessageForm message) {
            message.Addressee = MySim.FindAgent(SimId.AgentWorkersA);
            Notice(message);
        }

        public void ProcessDeassignWorkerB(MessageForm message) {
            message.Addressee = MySim.FindAgent(SimId.AgentWorkersB);
            Notice(message);
        }

        public void ProcessDeassignWorkerC(MessageForm message) {
            message.Addressee = MySim.FindAgent(SimId.AgentWorkersC);
            Notice(message);
        }

        public void ProcessAssignWorkerA(MessageForm message) {
            message.Addressee = MySim.FindAgent(SimId.AgentWorkersA);
            Notice(message);
        }

        public void ProcessAssignWorkerB(MessageForm message) {
            message.Addressee = MySim.FindAgent(SimId.AgentWorkersB);
            Notice(message);
        }

        public void ProcessAssignWorkerC(MessageForm message) {
            message.Addressee = MySim.FindAgent(SimId.AgentWorkersC);
            Notice(message);
        }

        public void ProcessInit(MessageForm message) {

        }

        public void ProcessDefault(MessageForm message) {

        }

        public void Init() {

        }

        override public void ProcessMessage(MessageForm message) {
            try {
                switch (message.Code) {
                    case Mc.GetWorkerA: ProcessGetWorkerA(message); break;
                    case Mc.GetWorkerC: ProcessGetWorkerC(message); break;
                    case Mc.GetWorkerB: ProcessGetWorkerB(message); break;
                    case Mc.AssignWorkerA: ProcessAssignWorkerA(message); break;
                    case Mc.AssignWorkerB: ProcessAssignWorkerB(message); break;
                    case Mc.AssignWorkerC: ProcessAssignWorkerC(message); break;
                    case Mc.DeassignWorkerA: ProcessDeassignWorkerA(message); break;
                    case Mc.DeassignWorkerB: ProcessDeassignWorkerB(message); break;
                    case Mc.DeassignWorkerC: ProcessDeassignWorkerC(message); break;
                    case Mc.GetWorkerToCut: ProcessGetWorkerForCutting(message); break;
                    case Mc.GetWorkerToPaint: ProcessGetWorkerForPainting(message); break;
                    case Mc.GetWorkerToPickle: ProcessGetWorkerForPickling(message); break;
                    case Mc.GetWorkerToAssemble: ProcessGetWorkerForAssembling(message); break;
                    case Mc.GetWorkerToMount: ProcessGetWorkerForMounting(message); break;
                    default: ProcessDefault(message); break;
                }
            } catch (Exception ex) {
                MessageBox.Show($"Error: {ex.Message}");
                throw;
            }
        }

        public new AgentWorkers MyAgent => (AgentWorkers)base.MyAgent;
    }
}