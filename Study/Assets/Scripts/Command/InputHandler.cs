using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private Stack<ICommand> _commandHistory = new Stack<ICommand>();
    // 1. Ǯ�� ���
    private CommandPool<MoveCommand> _commandPool = new CommandPool<MoveCommand>();
    public Transform character;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            // 1. Ǯ�� ���
            Move(Vector3.up);
            //ExecuteCommand(new MoveCommand(character, character.position + Vector3.up));
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            // 1. Ǯ�� ���
            Move(Vector3.down);
            //ExecuteCommand(new MoveCommand(character, character.position + Vector3.down));
        }
        if (Input.GetKeyDown(KeyCode.Z)) // Undo ���
        {
            UndoCommand();
        }
    }

    // 1. Ǯ�� ���
    void Move(Vector3 direction)
    {
        MoveCommand command = _commandPool.Get();
        command.Initialize(character, character.position + direction);
        command.Execute();
        _commandHistory.Push(command);
    }

    void ExecuteCommand(ICommand command)
    {
        Debug.Log($"User Input Execute Comaand {command}");
        command.Execute();
        Debug.Log($"Push Comaand {command}");
        _commandHistory.Push(command); // ��� ����
    }

    void UndoCommand()
    {
        if (_commandHistory.Count > 0)
        {
            Debug.Log($"User Input Undo Comaand");
            ICommand lastCommand = _commandHistory.Pop();
            lastCommand.Undo();

            // 1. Ǯ�� ���
            _commandPool.Release((MoveCommand)lastCommand);
        }
    }
}
