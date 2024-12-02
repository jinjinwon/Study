using System.Collections.Generic;
using UnityEngine;

public class CommandPool<T> where T : ICommand, new()
{
    private Stack<T> _pool = new Stack<T>();

    public T Get()
    {
        if (_pool.Count > 0)
        {
            return _pool.Pop();
        }
        return new T();
    }

    public void Release(T command)
    {
        _pool.Push(command);
    }
}
