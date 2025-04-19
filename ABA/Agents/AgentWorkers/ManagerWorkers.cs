using OSPABA;
using Simulation;
namespace Agents.AgentWorkers
{
	//meta! id="28"
	public class ManagerWorkers : OSPABA.Manager
	{
		public ManagerWorkers(int id, OSPABA.Simulation mySim, Agent myAgent) :
			base(id, mySim, myAgent)
		{
			Init();
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication

			if (PetriNet != null)
			{
				PetriNet.Clear();
			}
		}

		//meta! sender="AgentCarpentry", id="57", type="Request"
		public void ProcessGetWorkerForCutting(MessageForm message)
		{
		}

		//meta! sender="AgentCarpentry", id="121", type="Request"
		public void ProcessGetWorkerForPainting(MessageForm message)
		{
		}

		//meta! sender="AgentCarpentry", id="37", type="Notice"
		public void ProcessInit(MessageForm message)
		{
		}

		//meta! sender="AgentCarpentry", id="124", type="Request"
		public void ProcessGetWorkerForMounting(MessageForm message)
		{
		}

		//meta! sender="AgentWorkersC", id="156", type="Response"
		public void ProcessGetWorkerC(MessageForm message)
		{
		}

		//meta! sender="AgentWorkersB", id="157", type="Response"
		public void ProcessGetWorkerB(MessageForm message)
		{
		}

		//meta! sender="AgentCarpentry", id="127", type="Notice"
		public void ProcessDeassignWorker(MessageForm message)
		{
		}

		//meta! sender="AgentWorkersA", id="159", type="Response"
		public void ProcessGetWorkerA(MessageForm message)
		{
		}

		//meta! sender="AgentCarpentry", id="122", type="Request"
		public void ProcessGetWorkerForAssembling(MessageForm message)
		{
		}

		//meta! sender="AgentCarpentry", id="123", type="Request"
		public void ProcessGetWorkerForPickling(MessageForm message)
		{
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
			}
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		public void Init()
		{
		}

		override public void ProcessMessage(MessageForm message)
		{
			switch (message.Code)
			{
			case Mc.GetWorkerForAssembling:
				ProcessGetWorkerForAssembling(message);
			break;

			case Mc.GetWorkerB:
				ProcessGetWorkerB(message);
			break;

			case Mc.GetWorkerForPickling:
				ProcessGetWorkerForPickling(message);
			break;

			case Mc.GetWorkerForMounting:
				ProcessGetWorkerForMounting(message);
			break;

			case Mc.GetWorkerForPainting:
				ProcessGetWorkerForPainting(message);
			break;

			case Mc.DeassignWorker:
				ProcessDeassignWorker(message);
			break;

			case Mc.GetWorkerA:
				ProcessGetWorkerA(message);
			break;

			case Mc.GetWorkerForCutting:
				ProcessGetWorkerForCutting(message);
			break;

			case Mc.Init:
				ProcessInit(message);
			break;

			case Mc.GetWorkerC:
				ProcessGetWorkerC(message);
			break;

			default:
				ProcessDefault(message);
			break;
			}
		}
		//meta! tag="end"
		public new AgentWorkers MyAgent
		{
			get
			{
				return (AgentWorkers)base.MyAgent;
			}
		}
	}
}