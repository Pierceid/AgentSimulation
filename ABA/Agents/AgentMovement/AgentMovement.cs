using OSPABA;
using Simulation;
using Agents.AgentMovement.ContinualAssistants;

namespace Agents.AgentMovement {
    //meta! id="43"
    public class AgentMovement : OSPABA.Agent {
        public AgentMovement(int id, OSPABA.Simulation mySim, Agent parent) : base(id, mySim, parent) {
            Init();
        }

        override public void PrepareReplication() {
            base.PrepareReplication();
        }

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			new ManagerMovement(SimId.ManagerMovement, MySim, this);
			new MovingToStorage(SimId.MovingToStorage, MySim, this);
			new MovingToWorkplace(SimId.MovingToWorkplace, MySim, this);
			AddOwnMessage(Mc.MoveToStorage);
			AddOwnMessage(Mc.MoveToWorkplace);
		}
		//meta! tag="end"
    }
}