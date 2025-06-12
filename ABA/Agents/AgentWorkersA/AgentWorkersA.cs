using OSPABA;
using Simulation;

namespace Agents.AgentWorkersA {
    //meta! id="149"
    public class AgentWorkersA : OSPABA.Agent {
        public AgentWorkersA(int id, OSPABA.Simulation mySim, Agent parent) : base(id, mySim, parent) {
            Init();
        }

        override public void PrepareReplication() {
            base.PrepareReplication();
        }

        public void InitAnimator() {
            if (MySim.AnimatorExists) {
                var managerWorkersA = MyManager as ManagerWorkersA;
                managerWorkersA?.Workers.ForEach(w => {
                    MySim.Animator.Register(w.Image);
                    w.Image.SetPosition(w.X, w.Y);
                });
            }
        }

        //meta! userInfo="Generated code: do not modify", tag="begin"
        private void Init()
		{
			new ManagerWorkersA(SimId.ManagerWorkersA, MySim, this);
			AddOwnMessage(Mc.DeassignWorkerA);
			AddOwnMessage(Mc.GetWorkerA);
			AddOwnMessage(Mc.AssignWorkerA);
		}
		//meta! tag="end"
    }
}