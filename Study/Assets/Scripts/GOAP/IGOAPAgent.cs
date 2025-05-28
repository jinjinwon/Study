using System.Collections.Generic;
using UnityEngine;

public interface IGOAPAgent
{
    // ���� ����(���� ����)�� ��ȯ
    public Dictionary<string, object> GetWorldState();
    // �޼��ϰ��� �ϴ� ��ǥ ���¸� ��ȯ
    public Dictionary<string, object> CreateGoalState();
    // �÷� ���� ���� �� ȣ��
    public void PlanFailed(Dictionary<string, object> failedGoal);
    // �÷� ���� ���� �� ȣ��
    public void PlanFound(Dictionary<string, object> goal, Queue<GOAPAction> actions);
    // ��� �׼��� �� �������� �� ȣ��
    public void ActionsFinished();
    // �׼� ���� �� ���� �� ȣ��
    public void PlanAborted(GOAPAction aborter);
    // ���� �׼��� ���� ���� �̵� ó��
    public bool MoveAgent(GOAPAction nextAction);
}
