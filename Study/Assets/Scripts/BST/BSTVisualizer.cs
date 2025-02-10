using System.Collections.Generic;
using UnityEngine;

public class BSTVisualizer : MonoBehaviour
{
    // 탐색할 값
    public int searchValue = 40;

    private BinarySearchTree<int> bst;
    private List<TreeNode<int>> searchPath;

    void Start()
    {
        // BST 생성 및 데이터 삽입
        bst = new BinarySearchTree<int>();
        bst.Insert(30);
        bst.Insert(55);
        bst.Insert(50);
        bst.Insert(32);
        bst.Insert(31);
        bst.Insert(18);
        bst.Insert(19);
        bst.Insert(5);
        bst.Insert(70);
        bst.Insert(20);
        bst.Insert(40);
        bst.Insert(60);
        bst.Insert(80);

        // 노드 위치 계산
        bst.ComputeNodePositions(new Vector2(0, 0), 10f, 3f);

        // 탐색 수행 후, 방문한 노드들을 리스트에 저장
        searchPath = bst.SearchWithPath(searchValue);
    }

    // 씬 뷰에 Gizmos로 트리와 연결선을 그림
    void OnDrawGizmos()
    {
        if (bst == null)
            return;

        DrawTree(bst.Root);
    }

    // 재귀적으로 트리 노드를 그리는 메서드
    private void DrawTree(TreeNode<int> node)
    {
        if (node == null)
            return;

        // 탐색 경로에 포함된 노드는 빨간색, 나머지는 흰색으로 표시
        if (searchPath != null && searchPath.Contains(node))
            Gizmos.color = Color.red;
        else
            Gizmos.color = Color.white;

        // 노드 위치에 구 형태로 표시 (반지름 0.5)
        Gizmos.DrawSphere(new Vector3(node.position.x, node.position.y, 0), 0.5f);

        // (선택 사항) 에디터 전용: 노드 위에 값 표시
#if UNITY_EDITOR
        UnityEditor.Handles.Label(new Vector3(node.position.x, node.position.y + 2f, 0), node.data.ToString());
#endif

        // 왼쪽 자식과의 연결선 그리기
        if (node.left != null)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(new Vector3(node.position.x, node.position.y, 0), new Vector3(node.left.position.x, node.left.position.y, 0));
            DrawTree(node.left);
        }

        // 오른쪽 자식과의 연결선 그리기
        if (node.right != null)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(new Vector3(node.position.x, node.position.y, 0), new Vector3(node.right.position.x, node.right.position.y, 0));
            DrawTree(node.right);
        }
    }
}
