using OSPABA;
using Simulation;

namespace Agents.AgentWorkersB {
    //meta! id="151"
    public class AgentWorkersB : OSPABA.Agent {
        public AgentWorkersB(int id, OSPABA.Simulation mySim, Agent parent) : base(id, mySim, parent) {
            Init();
        }

        override public void PrepareReplication() {
            base.PrepareReplication();
        }

        public void InitAnimator() {
            if (MySim.AnimatorExists) {
                var managerWorkersB = MyManager as ManagerWorkersB;
                managerWorkersB?.Workers.ForEach(w => {
                    MySim.Animator.Register(w.Image);
                    w.Image.SetPosition(w.X, w.Y);
                });
            }
        }

        //meta! userInfo="Generated code: do not modify", tag="begin"
        private void Init()
		{
			new ManagerWorkersB(SimId.ManagerWorkersB, MySim, this);
			AddOwnMessage(Mc.DeassignWorkerB);
			AddOwnMessage(Mc.GetWorkerB);
			AddOwnMessage(Mc.AssignWorkerB);
		}
		//meta! tag="end"
    }
}