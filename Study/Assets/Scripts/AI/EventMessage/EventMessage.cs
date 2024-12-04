using UnityEngine;

[CreateAssetMenu(fileName = "NewEventMessage", menuName = "AI/Event Message")]
public class EventMessage : ScriptableObject
{
    public string messageName; // �̺�Ʈ �̸�
    public Command relatedCommand; // ����� Command

    public EventMessage(string messageName, Command relatedCommand)
    {
        this.messageName = messageName;
        this.relatedCommand = relatedCommand;
    }
}
