//meta! id="62"
using Agents.AgentWorkplaces;
using AgentSimulation.Structures.Enums;
using OSPABA;
using Simulation;

public class Cutting : OSPABA.Process {

    public Cutting(int id, OSPABA.Simulation mySim, CommonAgent myAgent) : base(id, mySim, myAgent) {
    }

    override public void PrepareReplication() {
        base.PrepareReplication();
    }

    //meta! sender="AgentWorkplaces", id="63", type="Start"
    public void ProcessStart(MessageForm message) {
        MyMessage myMessage = (MyMessage)message;
        MySimulation mySimulation = (MySimulation)MySim;

        if (myMessage.Product == null) return;

        double cuttingTime = myMessage.Product.Type switch {
            ProductType.Chair => mySimulation.Generators.ChairCuttingTime.Next(),
            ProductType.Table => mySimulation.Generators.TableCuttingTime.Next(),
            ProductType.Wardrobe => mySimulation.Generators.WardrobeCuttingTime.Next(),
            _ => 0
        };

        Hold(cuttingTime, message);
    }

    // This handles the end of the cutting process
    public void ProcessFinish(MessageForm message) {
        MyMessage myMessage = (MyMessage)message;

        myMessage.Code = Mc.Finish;
        myMessage.Addressee = MySim.FindAgent(SimId.AgentWorkplaces);
        AssistantFinished(myMessage);
    }

    //meta! userInfo="Process messages defined in code", id="0"
    public void ProcessDefault(MessageForm message) { }

    //meta! userInfo="Generated code: do not modify", tag="begin"
    override public void ProcessMessage(MessageForm message) {
        switch (message.Code) {
            case Mc.Start:
                ProcessStart(message);
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

    public new AgentWorkplaces MyAgent => (AgentWorkplaces)base.MyAgent;
}
