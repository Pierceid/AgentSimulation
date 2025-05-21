using OSPABA;
using Simulation;

namespace Agents.AgentProcesses {
    //meta! id="77"
    public class ManagerProcesses : OSPABA.Manager {
        public ManagerProcesses(int id, OSPABA.Simulation mySim, Agent myAgent) : base(id, mySim, myAgent) {
            Init();
        }

        override public void PrepareReplication() {
            base.PrepareReplication();
            PetriNet?.Clear();
        }

        //meta! sender="AgentCarpentry", id="215", type="Request"
        public void ProcessDoCut(MessageForm message) {
            message.Addressee = MyAgent.FindAssistant(SimId.Cutting);
            message.Code = Mc.Start;
            StartContinualAssistant(message);
        }

        //meta! sender="AgentCarpentry", id="216", type="Request"
        public void ProcessDoPaint(MessageForm message) {
            message.Addressee = MyAgent.FindAssistant(SimId.Painting);
            message.Code = Mc.Start;
            StartContinualAssistant(message);
        }

        //meta! sender="AgentCarpentry", id="217", type="Request"
        public void ProcessDoPickle(MessageForm message) {
            message.Addressee = MyAgent.FindAssistant(SimId.Pickling);
            message.Code = Mc.Start;
            StartContinualAssistant(message);
        }

        public void ProcessDoDry(MessageForm message) {
            message.Addressee = MyAgent.FindAssistant(SimId.Drying);
            message.Code = Mc.Start;
            StartContinualAssistant(message);
        }

        //meta! sender="AgentCarpentry", id="218", type="Request"
        public void ProcessDoAssemble(MessageForm message) {
            message.Addressee = MyAgent.FindAssistant(SimId.Assembling);
            message.Code = Mc.Start;
            StartContinualAssistant(message);
        }

        //meta! sender="AgentCarpentry", id="219", type="Request"
        public void ProcessDoMount(MessageForm message) {
            message.Addressee = MyAgent.FindAssistant(SimId.Mounting);
            message.Code = Mc.Start;
            StartContinualAssistant(message);
        }

        //meta! sender="AgentCarpentry", id="140", type="Request"
        public void ProcessDoPrepare(MessageForm message) {
            message.Addressee = MyAgent.FindAssistant(SimId.Preparing);
            message.Code = Mc.Start;
            StartContinualAssistant(message);
        }

        //meta! sender="Cutting", id="223", type="Finish"
        public void ProcessFinishCutting(MessageForm message) {
            message.Addressee = MySim.FindAgent(SimId.AgentCarpentry);
            message.Code = Mc.DoCut;
            Response(message);
        }

        //meta! sender="Painting", id="225", type="Finish"
        public void ProcessFinishPainting(MessageForm message) {
            message.Addressee = MySim.FindAgent(SimId.AgentCarpentry);
            message.Code = Mc.DoPaint;
            Response(message);
        }

        //meta! sender="Pickling", id="231", type="Finish"
        public void ProcessFinishPickling(MessageForm message) {
            message.Addressee = MySim.FindAgent(SimId.AgentCarpentry);
            message.Code = Mc.DoPickle;
            Response(message);
        }

        public void ProcessFinishDrying(MessageForm message) {
            message.Addressee = MySim.FindAgent(SimId.AgentCarpentry);
            message.Code = Mc.DoDry;
            Response(message);
        }

        //meta! sender="Assembling", id="229", type="Finish"
        public void ProcessFinishAssembling(MessageForm message) {
            message.Addressee = MySim.FindAgent(SimId.AgentCarpentry);
            message.Code = Mc.DoAssemble;
            Response(message);
        }

        //meta! sender="Mounting", id="227", type="Finish"
        public void ProcessFinishMounting(MessageForm message) {
            message.Addressee = MySim.FindAgent(SimId.AgentCarpentry);
            message.Code = Mc.DoMount;
            Response(message);
        }

        //meta! sender="Preparing", id="80", type="Finish"
        public void ProcessFinishPreparing(MessageForm message) {
            message.Addressee = MySim.FindAgent(SimId.AgentCarpentry);
            message.Code = Mc.DoPrepare;
            Response(message);
        }

        //meta! userInfo="Process messages defined in code", id="0"
        public void ProcessDefault(MessageForm message) {
            switch (message.Code) {
                case Mc.Finish:
                    switch (message.Sender.Id) {
                        case SimId.Preparing:
                            ProcessFinishPreparing(message);
                            break;

                        case SimId.Assembling:
                            ProcessFinishAssembling(message);
                            break;

                        case SimId.Mounting:
                            ProcessFinishMounting(message);
                            break;

                        case SimId.Painting:
                            ProcessFinishPainting(message);
                            break;

                        case SimId.Pickling:
                            ProcessFinishPickling(message);
                            break;

                        case SimId.Cutting:
                            ProcessFinishCutting(message);
                            break;

                        case SimId.Drying:
                            ProcessFinishDrying(message);
                            break;
                    }
                    break;
            }
        }

        //meta! userInfo="Generated code: do not modify", tag="begin"
        public void Init() {
        }

        override public void ProcessMessage(MessageForm message) {
            switch (message.Code) {
                case Mc.DoPickle:
                    ProcessDoPickle(message);
                    break;

                case Mc.DoPrepare:
                    ProcessDoPrepare(message);
                    break;

                case Mc.DoCut:
                    ProcessDoCut(message);
                    break;

                case Mc.DoPaint:
                    ProcessDoPaint(message);
                    break;

                case Mc.DoAssemble:
                    ProcessDoAssemble(message);
                    break;

                case Mc.DoMount:
                    ProcessDoMount(message);
                    break;

                case Mc.DoDry:
                    ProcessDoDry(message);
                    break;

                default:
                    ProcessDefault(message);
                    break;
            }
        }
        //meta! tag="end"

        public new AgentProcesses MyAgent => (AgentProcesses)base.MyAgent;
    }
}