using UnityEngine;

public class MoveCommand : ICommand
{
    private Transform _character;
    private Vector3 _previousPosition;
    private Vector3 _newPosition;


    //public MoveCommand(Transform character, Vector3 newPosition)
    //{
    //    _character = character;
    //    _newPosition = newPosition;
    //}

    // 1. Ǯ�� ���
    public void Initialize(Transform character, Vector3 newPosition)
    {
        _character = character;
        _newPosition = newPosition;
    }

    public void Execute()
    {
        Debug.Log($"Moving from {_previousPosition} to {_newPosition}");
        _previousPosition = _character.position; // ���� ��ġ ����
        _character.position = _newPosition;      // ���ο� ��ġ�� �̵�
    }

    public void Undo()
    {
        Debug.Log($"Undoing move: returning to {_previousPosition}");
        _character.position = _previousPosition; // ���� ��ġ�� �ǵ���
    }
}
