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

        if (!_priorityQueue[priority].Contains(command))
        {
            _priorityQueue[priority].Add(command);
        }
    }

    public void ExecuteNext(Transform aiTransform, Transform target = null)
    {
        if (_currentCommand != null)
        {
            _currentCommand.Cancel();
            _currentCommand = null;
        }

        foreach (var commandList in _priorityQueue.Values)
        {
            if (commandList.Count > 0)
            {
                _currentCommand = commandList[0];
                commandList.RemoveAt(0);
                _currentCommand.StartExecution(aiTransform, target);
                break;
            }
        }
    }
}
