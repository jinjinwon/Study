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

    // 데이터 삽입
    public void Insert(T data)
    {
        // 처음으로 들어온 값을 ROOT로 설정
        if (root == null)
            root = new TreeNode<T>(data);
        // 그 이후는 갈라지게끔 
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
        // 중복된 값은 무시합니다.
    }

    // 탐색 과정에서 방문한 노드들을 순서대로 반환하는 메서드
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

        // 현재 노드를 경로에 추가
        path.Add(node);
        int compareResult = data.CompareTo(node.data);
        if (compareResult == 0)
            return node;
        else if (compareResult < 0)
            return SearchWithPathRecursive(node.left, data, path);
        else
            return SearchWithPathRecursive(node.right, data, path);
    }

    // 시각화를 위해 각 노드에 위치를 할당하는 메서드  
    public void ComputeNodePositions(Vector2 startPos, float horizontalSpacing, float verticalSpacing)
    {
        ComputeNodePositions(root, startPos, horizontalSpacing, verticalSpacing);
    }

    private void ComputeNodePositions(TreeNode<T> node, Vector2 pos, float horizontalSpacing, float verticalSpacing)
    {
        if (node == null)
            return;
        // 현재 노드의 위치를 할당
        node.position = pos;
        // 왼쪽 자식은 현재 위치에서 왼쪽 및 아래쪽으로 이동한 위치를 할당  
        ComputeNodePositions(node.left, new Vector2(pos.x - horizontalSpacing, pos.y - verticalSpacing), horizontalSpacing * 0.5f, verticalSpacing);
        // 오른쪽 자식은 현재 위치에서 오른쪽 및 아래쪽으로 이동한 위치를 할당  
        ComputeNodePositions(node.right, new Vector2(pos.x + horizontalSpacing, pos.y - verticalSpacing), horizontalSpacing * 0.5f, verticalSpacing);
    }
}
