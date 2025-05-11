using AgentSimulation.Structures.Objects;
using OSPABA;

namespace Simulation {
    public class MyMessage : OSPABA.MessageForm {
        public Order? Order { get; set; }
        public Product? Product { get; set; }
        public Worker? Worker { get; set; }
        public Workplace? Workplace { get; set; }

        public MyMessage(OSPABA.Simulation mySim) : base(mySim) {
            Worker = null;
            Order = null;
            Product = null;
            Workplace = null;
        }

        public MyMessage(MyMessage original) : base(original) {
            Order = original.Order;
            Product = original.Product;
            Worker = original.Worker;
            Workplace = original.Workplace;
        }

        public MyMessage(MessageForm original) : base(original) {
            Worker = ((MyMessage)original).Worker;
            Order = ((MyMessage)original).Order;
            Product = ((MyMessage)original).Product;
            Workplace = ((MyMessage)original).Workplace;
        }

        override public MessageForm CreateCopy() {
            return new MyMessage(this);
        }

        override protected void Copy(MessageForm message) {
            base.Copy(message);
            MyMessage original = (MyMessage)message;

            Worker = original.Worker;
            Order = original.Order;
            Product = original.Product;
            Workplace = original.Workplace;
        }
    }
}