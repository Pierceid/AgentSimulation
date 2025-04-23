using AgentSimulation.Structures;
using AgentSimulation.Structures.Enums;
using AgentSimulation.Structures.Objects;
using OSPABA;
using Simulation;
namespace Agents.AgentScope {
    //meta! id="3"
    public class ManagerScope : OSPABA.Manager {
        public List<Order> Orders { get; set; } = new();
        public List<Product> Products { get; set; } = new();

        private static int orderId = 0;
        private static int productId = 0;

        public ManagerScope(int id, OSPABA.Simulation mySim, Agent myAgent) : base(id, mySim, myAgent) {
            Init();
        }

        public void Clear() {
            Orders.Clear();
            Products.Clear();
            orderId = 0;
            productId = 0;
        }

        override public void PrepareReplication() {
            base.PrepareReplication();

            PetriNet?.Clear();
        }

        //meta! sender="AgentModel", id="240", type="Notice"
        public void ProcessInit(MessageForm message) {
            message.Addressee = MyAgent.FindAssistant(SimId.OrderArrival);
            StartContinualAssistant(message);
        }

        //meta! sender="OrderArrival", id="164", type="Finish"
        public void ProcessFinish(MessageForm message) {
        }

        //meta! sender="AgentModel", id="10", type="Notice"
        public void ProcessOrderExit(MessageForm message) {
            var myMessage = (MyMessage)message.CreateCopy();

            if (myMessage.Product == null) return;

            myMessage.Product.EndTime = MySim.CurrentTime;

            ((MySimulation)MySim).AverageOrderTime.AddSample(myMessage.Product.EndTime - myMessage.Product.StartTime);

            if (MySim.CurrentTime >= Constants.SIMULATION_TIME) {
                MySim.StopReplication();
            }
        }

        //meta! userInfo="Process messages defined in code", id="0"
        public void ProcessDefault(MessageForm message) {
            MyMessage myMessage = (MyMessage)message.CreateCopy();
            MySimulation mySimulation = (MySimulation)MySim;

            Order order = new(orderId++, mySimulation.CurrentTime);
            int count = mySimulation.Generators.ProductCount.Next();
            List<Product> products = new(count);

            for (int i = 0; i < count; i++) {
                double rng = mySimulation.Generators.RNG.Next();
                ProductType productType = rng < 0.5 ? ProductType.Table : rng < 0.65 ? ProductType.Chair : ProductType.Wardrobe;
                Product product = new(productId++, productType, order);

                products.Add(product);
            }

            order.AddProducts(products);
            Orders.Add(order);
            Products.AddRange(products);

            myMessage.Order = order;
            myMessage.Addressee = mySimulation.FindAgent(SimId.AgentModel);
            myMessage.Code = Mc.OrderEnter;
            Notice(myMessage);

            myMessage = new(mySimulation);
            myMessage.Addressee = MyAgent.FindAssistant(SimId.OrderArrival);
            StartContinualAssistant(myMessage);
        }

        //meta! userInfo="Generated code: do not modify", tag="begin"
        public void Init() {
        }

        override public void ProcessMessage(MessageForm message) {
            switch (message.Code) {
                case Mc.OrderExit:
                    ProcessOrderExit(message);
                    break;

                case Mc.Init:
                    ProcessInit(message);
                    break;

                case Mc.Finish:
                    ProcessFinish(message);
                    break;

                default:
                    ProcessDefault(message);
                    break;
            }
        }
        //meta! tag="end"
        public new AgentScope MyAgent {
            get {
                return (AgentScope)base.MyAgent;
            }
        }
    }
}