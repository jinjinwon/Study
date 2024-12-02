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

    // 1. 풀링 사용
    public void Initialize(Transform character, Vector3 newPosition)
    {
        _character = character;
        _newPosition = newPosition;
    }

    public void Execute()
    {
        Debug.Log($"Moving from {_previousPosition} to {_newPosition}");
        _previousPosition = _character.position; // 이전 위치 저장
        _character.position = _newPosition;      // 새로운 위치로 이동
    }

    public void Undo()
    {
        Debug.Log($"Undoing move: returning to {_previousPosition}");
        _character.position = _previousPosition; // 이전 위치로 되돌림
    }
}
