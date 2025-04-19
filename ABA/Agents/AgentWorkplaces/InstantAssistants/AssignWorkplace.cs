using OSPABA;
namespace Agents.AgentWorkplaces.InstantAssistants {
    //meta! id="70"
    public class AssignWorkplace : OSPABA.Action {
        public AssignWorkplace(int id, OSPABA.Simulation mySim, CommonAgent myAgent) : base(id, mySim, myAgent) {
        }

        override public void Execute(MessageForm message) {

        }
        public new AgentWorkplaces MyAgent {
            get {
                return (AgentWorkplaces)base.MyAgent;
            }
        }
    }
}