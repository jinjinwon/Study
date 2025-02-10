using System;
using UnityEngine;

public class TreeNode<T> where T : IComparable<T>
{
    public T data;
    public TreeNode<T> left;
    public TreeNode<T> right;

    // �ð�ȭ�� ���� ��ġ ����
    public Vector2 position;

    public TreeNode(T data)
    {
        this.data = data;
        left = null;
        right = null;
        position = Vector2.zero;
    }
}
