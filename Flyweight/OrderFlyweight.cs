using AgentSimulation.Structures.Enums;
using AgentSimulation.Structures.Objects;

namespace AgentSimulation.Flyweight {
    public class OrderFlyweight {
        private Product? instance = null;
        private static object objLock = new();
        private static int orderId = 0;

        public OrderFlyweight() { }

        public Product GetOrder(ProductType type, double time) {
            if (instance == null) {
                lock (objLock) {
                    instance ??= new Product(orderId++, type, time);
                }
            } else {
                lock (objLock) {
                    instance.Id = orderId++;
                    instance.Type = type;
                    instance.StartTime = time;
                }
            }
            return instance;
        }
    }
}
