using System.Collections.Generic;
using UnityEngine;

public class BehaviorQueue
{
    private Command _currentCommand;
    private SortedDictionary<int, List<Command>> _priorityQueue = new SortedDictionary<int, List<Command>>();

    public void AddCommand(Command command, int priority)
    {
        if (!_priorityQueue.ContainsKey(priority))
        {
            _priorityQueue[priority] = new List<Command>();
        }

        // 중복 명령 방지 로직 추가
        if (!_priorityQueue[priority].Contains(command))
        {
            _priorityQueue[priority].Add(command);
        }
    }

    public void ExecuteNext(Transform aiTransform, Transform target = null)
    {
        if (_currentCommand != null) return;

        foreach (var commandList in _priorityQueue.Values)
        {
            if (commandList.Count > 0)
            {
                _currentCommand = commandList[0];
                commandList.RemoveAt(0);
                _currentCommand.Execute(aiTransform, target);
                _currentCommand = null;
                break;
            }
        }
    }
}
