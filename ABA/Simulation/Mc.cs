using OSPABA;
namespace Simulation
{
	public class Mc : OSPABA.IdList
	{
		//meta! userInfo="Generated code: do not modify", tag="begin"
		public const int OrderExit = 1002;
		public const int ProcessOrder = 1003;
		public const int DoPreparing = 1054;
		public const int OrderEnter = 1004;
		public const int Init = 1006;
		public const int GetWorkerC = 1055;
		public const int GetWorkerB = 1056;
		public const int GetWorkerA = 1058;
		public const int GetFreeWorkplace = 1059;
		public const int MoveToWorkplace = 1012;
		public const int GetWorkerForCutting = 1013;
		public const int PlanOrderArrival = 1060;
		public const int AssignWorkplace = 1017;
		public const int DeassignWorkplace = 1018;
		public const int AssignWorker = 1019;
		public const int MoveToStorage = 1030;
		public const int GetWorkerForPainting = 1032;
		public const int GetWorkerForAssembling = 1033;
		public const int GetWorkerForMounting = 1034;
		public const int GetWorkerForPickling = 1035;
		public const int DeassignWorker = 1041;
		//meta! tag="end"

		// 1..1000 range reserved for user
	}
}