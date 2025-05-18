using AgentSimulation.Structures.Objects;
using OSPABA;

namespace Simulation {
    public class MyMessage : OSPABA.MessageForm {
        public Order? Order { get; set; }
        public Product? Product { get; set; }
        public Worker? WorkerToRelease { get; set; }
        public Workplace? Workplace { get; set; }

        public MyMessage(OSPABA.Simulation mySim) : base(mySim) {
            Order = null;
            Product = null;
            WorkerToRelease = null;
            Workplace = null;
        }

        public MyMessage(MyMessage original) : base(original) {
            Order = original.Order;
            Product = original.Product;
            WorkerToRelease = original.WorkerToRelease;
            Workplace = original.Workplace;
        }

        public MyMessage(MessageForm original) : base(original) {
            Order = ((MyMessage)original).Order;
            Product = ((MyMessage)original).Product;
            WorkerToRelease = ((MyMessage)original).WorkerToRelease;
            Workplace = ((MyMessage)original).Workplace;
        }

        public Worker? GetWorkerForCutting() => this.Product?.WorkerToCut;
        public Worker? GetWorkerForPainting() => this.Product?.WorkerToPaint;
        public Worker? GetWorkerForPickling() => this.Product?.WorkerToPickle;
        public Worker? GetWorkerForAssembling() => this.Product?.WorkerToAssemble;
        public Worker? GetWorkerForMounting() => this.Product?.WorkerToMount;
        public Worker? GetAssignedWorker() {
            if (GetWorkerForCutting() != null) return GetWorkerForCutting();
            if (GetWorkerForPainting() != null) return GetWorkerForPainting();
            if (GetWorkerForPickling() != null) return GetWorkerForPickling();
            if (GetWorkerForAssembling() != null) return GetWorkerForAssembling();
            if (GetWorkerForMounting() != null) return GetWorkerForMounting();
            return null;

        }

        override public MessageForm CreateCopy() {
            return new MyMessage(this);
        }

        override protected void Copy(MessageForm message) {
            base.Copy(message);
            MyMessage original = (MyMessage)message;

            Order = original.Order;
            Product = original.Product;
            WorkerToRelease = original.WorkerToRelease;
            Workplace = original.Workplace;
        }
    }
}