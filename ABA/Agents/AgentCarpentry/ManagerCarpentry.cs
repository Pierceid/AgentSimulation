using OSPABA;
using Simulation;
namespace Agents.AgentCarpentry
{
	//meta! id="4"
	public class ManagerCarpentry : OSPABA.Manager
	{
		public ManagerCarpentry(int id, OSPABA.Simulation mySim, Agent myAgent) :
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

		//meta! sender="AgentWorkplaces", id="115", type="Request"
		public void ProcessGetWorkerForCuttingAgentWorkplaces(MessageForm message)
		{
		}

		//meta! sender="AgentWorkers", id="57", type="Response"
		public void ProcessGetWorkerForCuttingAgentWorkers(MessageForm message)
		{
		}

		//meta! sender="AgentWorkplaces", id="137", type="Response"
		public void ProcessDoPickling(MessageForm message)
		{
		}

		//meta! sender="AgentWorkplaces", id="119", type="Request"
		public void ProcessGetWorkerForMountingAgentWorkplaces(MessageForm message)
		{
		}

		//meta! sender="AgentWorkers", id="124", type="Response"
		public void ProcessGetWorkerForMountingAgentWorkers(MessageForm message)
		{
		}

		//meta! sender="AgentModel", id="12", type="Request"
		public void ProcessProcessOrder(MessageForm message)
		{
		}

		//meta! sender="AgentWorkplaces", id="138", type="Response"
		public void ProcessDoAssembling(MessageForm message)
		{
		}

		//meta! sender="AgentWorkers", id="122", type="Response"
		public void ProcessGetWorkerForAssemblingAgentWorkers(MessageForm message)
		{
		}

		//meta! sender="AgentWorkplaces", id="118", type="Request"
		public void ProcessGetWorkerForAssemblingAgentWorkplaces(MessageForm message)
		{
		}

		//meta! sender="AgentMovement", id="55", type="Response"
		public void ProcessMoveToWorkplace(MessageForm message)
		{
		}

		//meta! sender="AgentModel", id="27", type="Notice"
		public void ProcessInit(MessageForm message)
		{
		}

		//meta! sender="AgentWorkers", id="121", type="Response"
		public void ProcessGetWorkerForPaintingAgentWorkers(MessageForm message)
		{
		}

		//meta! sender="AgentWorkplaces", id="117", type="Request"
		public void ProcessGetWorkerForPaintingAgentWorkplaces(MessageForm message)
		{
		}

		//meta! sender="AgentWorkplaces", id="128", type="Response"
		public void ProcessGetFreeWorkplaceAgentWorkplaces(MessageForm message)
		{
		}

		//meta! sender="AgentWorkplaces", id="128", type="Response"
		public void ProcessGetFreeWorkplaceAgentWorkplaces(MessageForm message)
		{
		}

		//meta! sender="AgentWorkplaces", id="139", type="Response"
		public void ProcessDoMounting(MessageForm message)
		{
		}

		//meta! sender="AgentWorkplaces", id="135", type="Response"
		public void ProcessDoCutting(MessageForm message)
		{
		}

		//meta! sender="AgentWorkplaces", id="136", type="Response"
		public void ProcessDoPainting(MessageForm message)
		{
		}

		//meta! sender="AgentWorkers", id="75", type="Notice"
		public void ProcessAssignWorker(MessageForm message)
		{
		}

		//meta! sender="AgentWarehouse", id="140", type="Response"
		public void ProcessDoPreparing(MessageForm message)
		{
		}

		//meta! sender="AgentMovement", id="112", type="Response"
		public void ProcessMoveToStorage(MessageForm message)
		{
		}

		//meta! sender="AgentWorkplaces", id="120", type="Request"
		public void ProcessGetWorkerForPicklingAgentWorkplaces(MessageForm message)
		{
		}

		//meta! sender="AgentWorkers", id="123", type="Response"
		public void ProcessGetWorkerForPicklingAgentWorkers(MessageForm message)
		{
		}

		//meta! sender="AgentWorkplaces", id="73", type="Notice"
		public void ProcessDeassignWorkplace(MessageForm message)
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
			case Mc.DoPainting:
				ProcessDoPainting(message);
			break;

			case Mc.AssignWorker:
				ProcessAssignWorker(message);
			break;

			case Mc.DoMounting:
				ProcessDoMounting(message);
			break;

			case Mc.GetWorkerForMounting:
				switch (message.Sender.Id)
				{
				case SimId.AgentWorkplaces:
					ProcessGetWorkerForMountingAgentWorkplaces(message);
				break;

				case SimId.AgentWorkers:
					ProcessGetWorkerForMountingAgentWorkers(message);
				break;
				}
			break;

			case Mc.GetWorkerForAssembling:
				switch (message.Sender.Id)
				{
				case SimId.AgentWorkers:
					ProcessGetWorkerForAssemblingAgentWorkers(message);
				break;

				case SimId.AgentWorkplaces:
					ProcessGetWorkerForAssemblingAgentWorkplaces(message);
				break;
				}
			break;

			case Mc.DoAssembling:
				ProcessDoAssembling(message);
			break;

			case Mc.DoPickling:
				ProcessDoPickling(message);
			break;

			case Mc.Init:
				ProcessInit(message);
			break;

			case Mc.GetWorkerForPainting:
				switch (message.Sender.Id)
				{
				case SimId.AgentWorkers:
					ProcessGetWorkerForPaintingAgentWorkers(message);
				break;

				case SimId.AgentWorkplaces:
					ProcessGetWorkerForPaintingAgentWorkplaces(message);
				break;
				}
			break;

			case Mc.GetWorkerForPickling:
				switch (message.Sender.Id)
				{
				case SimId.AgentWorkplaces:
					ProcessGetWorkerForPicklingAgentWorkplaces(message);
				break;

				case SimId.AgentWorkers:
					ProcessGetWorkerForPicklingAgentWorkers(message);
				break;
				}
			break;

			case Mc.MoveToWorkplace:
				ProcessMoveToWorkplace(message);
			break;

			case Mc.MoveToStorage:
				ProcessMoveToStorage(message);
			break;

			case Mc.GetWorkerForCutting:
				switch (message.Sender.Id)
				{
				case SimId.AgentWorkplaces:
					ProcessGetWorkerForCuttingAgentWorkplaces(message);
				break;

				case SimId.AgentWorkers:
					ProcessGetWorkerForCuttingAgentWorkers(message);
				break;
				}
			break;

			case Mc.DoPreparing:
				ProcessDoPreparing(message);
			break;

			case Mc.DoCutting:
				ProcessDoCutting(message);
			break;

			case Mc.ProcessOrder:
				ProcessProcessOrder(message);
			break;

			case Mc.GetFreeWorkplace:
				switch (message.Sender.Id)
				{
				case SimId.AgentWorkplaces:
					ProcessGetFreeWorkplaceAgentWorkplaces(message);
				break;

				case SimId.AgentWorkplaces:
					ProcessGetFreeWorkplaceAgentWorkplaces(message);
				break;
				}
			break;

			case Mc.DeassignWorkplace:
				ProcessDeassignWorkplace(message);
			break;

			default:
				ProcessDefault(message);
			break;
			}
		}
		//meta! tag="end"
		public new AgentCarpentry MyAgent
		{
			get
			{
				return (AgentCarpentry)base.MyAgent;
			}
		}
	}
}