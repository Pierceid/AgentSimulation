using OSPABA;
using Simulation;

namespace Agents.AgentModel {
    //meta! id="2"
    public class AgentModel : OSPABA.Agent {
        public AgentModel(int id, OSPABA.Simulation mySim, Agent parent) : base(id, mySim, parent) {
            Init();
        }

        override public void PrepareReplication() {
            base.PrepareReplication();

            var myMessage = new MyMessage(MySim) {
                Addressee = MySim.FindAgent(SimId.AgentScope),
                Code = Mc.Init
            };

            MyManager.Notice(new MyMessage(myMessage));
            myMessage.Addressee = MySim.FindAgent(SimId.AgentCarpentry);
            MyManager.Notice(new MyMessage(myMessage));
        }

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			new ManagerModel(SimId.ManagerModel, MySim, this);
			AddOwnMessage(Mc.ProcessOrder);
			AddOwnMessage(Mc.OrderEnter);
		}
		//meta! tag="end"
    }
}