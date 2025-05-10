using System.Windows;
using OSPABA;
using Simulation;

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
            MessageBox.Show("ProcessGetWorkerForCutting");
            message.Addressee = MySim.FindAgent(SimId.AgentWorkersA);
            message.Code = Mc.GetWorkerA;
            Request(message);
        }

        public void ProcessGetWorkerForPainting(MessageForm message) {
            MessageBox.Show("ProcessGetWorkerForPainting");
            message.Addressee = MySim.FindAgent(SimId.AgentWorkersB);
            message.Code = Mc.GetWorkerC;
            Request(message);
        }

        public void ProcessGetWorkerForMounting(MessageForm message) {
            MessageBox.Show("ProcessGetWorkerForMounting");
            message.Addressee = MySim.FindAgent(SimId.AgentWorkersC);
            message.Code = Mc.GetWorkerC;
            Request(message);
        }

        public void ProcessGetWorkerForAssembling(MessageForm message) {
            MessageBox.Show("ProcessGetWorkerForAssembling");
            message.Addressee = MySim.FindAgent(SimId.AgentWorkersA);
            message.Code = Mc.GetWorkerB;
            Request(message);
        }

        public void ProcessGetWorkerForPickling(MessageForm message) {
            MessageBox.Show("ProcessGetWorkerForPickling");
            message.Addressee = MySim.FindAgent(SimId.AgentWorkersB);
            message.Code = Mc.GetWorkerC;
            Request(message);
        }

        public void ProcessGetWorkerA(MessageForm message) {
            MessageBox.Show("ProcessGetWorkerA");
            message.Addressee = MySim.FindAgent(SimId.AgentCarpentry);
            message.Code = Mc.GetWorkerToCut;
            Response(message);
        }

        public void ProcessGetWorkerB(MessageForm message) {
            MessageBox.Show("ProcessGetWorkerB");
            message.Addressee = MySim.FindAgent(SimId.AgentCarpentry);
            message.Code = Mc.GetWorkerToPaint;
            Response(message);
        }

        public void ProcessGetWorkerC(MessageForm message) {
            MessageBox.Show("ProcessGetWorkerC");
            message.Addressee = MySim.FindAgent(SimId.AgentCarpentry);
            message.Code = Mc.GetWorkerToMount;
            Response(message);
        }

        public void ProcessDeassignWorkerA(MessageForm message) {
            MessageBox.Show("ProcessDeassignWorkerA");
            message.Addressee = MySim.FindAgent(SimId.AgentWorkersA);
            message.Code = Mc.DeassignWorkerA;
            Notice(message);
        }

        public void ProcessDeassignWorkerB(MessageForm message) {
            MessageBox.Show("ProcessDeassignWorkerB");
            message.Addressee = MySim.FindAgent(SimId.AgentWorkersB);
            message.Code = Mc.DeassignWorkerB;
            Notice(message);
        }

        public void ProcessDeassignWorkerC(MessageForm message) {
            MessageBox.Show("ProcessDeassignWorkerC");
            message.Addressee = MySim.FindAgent(SimId.AgentWorkersC);
            message.Code = Mc.DeassignWorkerC;
            Notice(message);
        }

        public void ProcessInit(MessageForm message) {

        }

        public void ProcessDefault(MessageForm message) {

        }

        public void Init() {

        }

        override public void ProcessMessage(MessageForm message) {
            MessageBox.Show($"ProcessMessage - Code: {message.Code}");
            switch (message.Code) {
                case Mc.GetWorkerB: ProcessGetWorkerB(message); break;
                case Mc.GetWorkerForPickling: ProcessGetWorkerForPickling(message); break;
                case Mc.GetWorkerC: ProcessGetWorkerC(message); break;
                case Mc.GetWorkerA: ProcessGetWorkerA(message); break;
                case Mc.GetWorkerForCutting: ProcessGetWorkerForCutting(message); break;
                case Mc.DeassignWorkerB: ProcessDeassignWorkerB(message); break;
                case Mc.DeassignWorkerA: ProcessDeassignWorkerA(message); break;
                case Mc.GetWorkerForAssembling: ProcessGetWorkerForAssembling(message); break;
                case Mc.DeassignWorkerC: ProcessDeassignWorkerC(message); break;
                case Mc.GetWorkerForPainting: ProcessGetWorkerForPainting(message); break;
                case Mc.GetWorkerForMounting: ProcessGetWorkerForMounting(message); break;
                default: ProcessDefault(message); break;
            }
        }

        public new AgentWorkers MyAgent => (AgentWorkers)base.MyAgent;
    }
}
