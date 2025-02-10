using System;
using System.Collections.Generic;
using UnityEngine;

public class BinarySearchTree<T> where T : IComparable<T>
{
    private TreeNode<T> root;

    public TreeNode<T> Root { get { return root; } }

    public BinarySearchTree()
    {
        root = null;
    }

    // ������ ����
    public void Insert(T data)
    {
        // ó������ ���� ���� ROOT�� ����
        if (root == null)
            root = new TreeNode<T>(data);
        // �� ���Ĵ� �������Բ� 
        else
            InsertRecursively(root, data);
    }

    private void InsertRecursively(TreeNode<T> node, T data)
    {
        if (data.CompareTo(node.data) < 0)
        {
            if (node.left == null)
                node.left = new TreeNode<T>(data);
            else
                InsertRecursively(node.left, data);
        }
        else if (data.CompareTo(node.data) > 0)
        {
            if (node.right == null)
                node.right = new TreeNode<T>(data);
            else
                InsertRecursively(node.right, data);
        }
        // �ߺ��� ���� �����մϴ�.
    }

    // Ž�� �������� �湮�� ������ ������� ��ȯ�ϴ� �޼���
    public List<TreeNode<T>> SearchWithPath(T data)
    {
        List<TreeNode<T>> path = new List<TreeNode<T>>();
        SearchWithPathRecursive(root, data, path);
        return path;
    }

    private TreeNode<T> SearchWithPathRecursive(TreeNode<T> node, T data, List<TreeNode<T>> path)
    {
        if (node == null)
            return null;

        // ���� ��带 ��ο� �߰�
        path.Add(node);
        int compareResult = data.CompareTo(node.data);
        if (compareResult == 0)
            return node;
        else if (compareResult < 0)
            return SearchWithPathRecursive(node.left, data, path);
        else
            return SearchWithPathRecursive(node.right, data, path);
    }

    // �ð�ȭ�� ���� �� ��忡 ��ġ�� �Ҵ��ϴ� �޼���  
    public void ComputeNodePositions(Vector2 startPos, float horizontalSpacing, float verticalSpacing)
    {
        ComputeNodePositions(root, startPos, horizontalSpacing, verticalSpacing);
    }

    private void ComputeNodePositions(TreeNode<T> node, Vector2 pos, float horizontalSpacing, float verticalSpacing)
    {
        if (node == null)
            return;
        // ���� ����� ��ġ�� �Ҵ�
        node.position = pos;
        // ���� �ڽ��� ���� ��ġ���� ���� �� �Ʒ������� �̵��� ��ġ�� �Ҵ�  
        ComputeNodePositions(node.left, new Vector2(pos.x - horizontalSpacing, pos.y - verticalSpacing), horizontalSpacing * 0.5f, verticalSpacing);
        // ������ �ڽ��� ���� ��ġ���� ������ �� �Ʒ������� �̵��� ��ġ�� �Ҵ�  
        ComputeNodePositions(node.right, new Vector2(pos.x + horizontalSpacing, pos.y - verticalSpacing), horizontalSpacing * 0.5f, verticalSpacing);
    }
}
