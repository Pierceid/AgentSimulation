using OSPABA;
using Simulation;

namespace Agents.AgentWorkersC {
    //meta! id="150"
    public class AgentWorkersC : OSPABA.Agent {
        public AgentWorkersC(int id, OSPABA.Simulation mySim, Agent parent) : base(id, mySim, parent) {
            Init();
        }

        override public void PrepareReplication() {
            base.PrepareReplication();
        }

        public void InitAnimator() {
            if (MySim.AnimatorExists) {
                var managerWorkersC = MyManager as ManagerWorkersC;
                managerWorkersC?.Workers.ForEach(w => {
                    MySim.Animator.Register(w.Image);
                    w.Image.SetPosition(w.X, w.Y);
                });
            }
        }

        //meta! userInfo="Generated code: do not modify", tag="begin"
        private void Init()
		{
			new ManagerWorkersC(SimId.ManagerWorkersC, MySim, this);
			AddOwnMessage(Mc.DeassignWorkerC);
			AddOwnMessage(Mc.GetWorkerC);
			AddOwnMessage(Mc.AssignWorkerC);
		}
		//meta! tag="end"
    }
}