using UnityEngine;

[CreateAssetMenu(fileName = "NewEventMessage", menuName = "AI/Event Message")]
public class EventMessage : ScriptableObject
{
    public string messageName; // 이벤트 이름
    public Command relatedCommand; // 연결된 Command

    public EventMessage(string messageName, Command relatedCommand)
    {
        this.messageName = messageName;
        this.relatedCommand = relatedCommand;
    }
}
