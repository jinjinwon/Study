using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MazeGenerator : MonoBehaviour
{
    [Header("�̷� ����")]
    public int width = 21;                                          // �̷��� ���� ũ�� (Ȧ�� ����)
    public int height = 21;                                         // �̷��� ���� ũ�� (Ȧ�� ����)
    public GameObject wallPrefab;                                   // ������ ����� ������
    public float wallHeight = 2f;                                   // ���� ����
    public float wallScale = 1f;                                    // ���� ������

    [Header("Ÿ�� ����")]
    public GameObject targetObject;                                 // ��ǥ�� �� ������Ʈ
    public Color exitPathColor = Color.blue;                        // Ż�ⱸ ��� ����
    public Color targetPathColor = Color.yellow;                    // Ÿ�� ������Ʈ ��� ����

    [Header("��� �ð�ȭ")]
    public Material pathMaterial;                                   // LineRenderer�� ����� ��Ƽ����

    private int[,] maze; // �̷� ������ �迭
    private System.Random rand = new System.Random();

    private List<Vector2Int> exitPath = new List<Vector2Int>();     // ���Ա����� Ż�ⱸ������ ���
    private List<Vector2Int> targetPath = new List<Vector2Int>();   // ���Ա����� Ÿ�ٱ����� ���

    private LineRenderer exitLineRenderer;                          // Ż�ⱸ ��� LineRenderer
    private LineRenderer targetLineRenderer;                        // Ÿ�� ������Ʈ ��� LineRenderer

    void Start()
    {
        // Ȧ���� ����
        if (width % 2 == 0) width += 1;
        if (height % 2 == 0) height += 1;

        GenerateMaze();
        CreateEntranceAndExit();
        PlaceTargetObject();

        CreateMaze();
        InitializeLineRenderers();
    }

    // �̷� ���� (DFS)
    void GenerateMaze()
    {
        maze = new int[width, height];

        // ��� ���� ������ �ʱ�ȭ
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                maze[x, y] = 0;

        // ������ ����
        Vector2Int start = new Vector2Int(1, 1);
        GenerateMazeDFS(start.x, start.y);
    }

    // (DFS) �̷� ���� ��� �Լ�
    void GenerateMazeDFS(int x, int y)
    {
        maze[x, y] = 1; // ��� ����

        // �����¿� ���� (2ĭ�� �̵�)
        List<Vector2Int> directions = new List<Vector2Int>() {
            new Vector2Int(0, 2),
            new Vector2Int(2, 0),
            new Vector2Int(0, -2),
            new Vector2Int(-2, 0)
        };

        // ������ �����ϰ� ����
        Shuffle(directions);

        foreach (var dir in directions)
        {
            int nx = x + dir.x;
            int ny = y + dir.y;

            if (IsInBounds(nx, ny) && maze[nx, ny] == 0)
            {
                // ���� �����Ͽ� ���� ����
                maze[x + dir.x / 2, y + dir.y / 2] = 1;
                GenerateMazeDFS(nx, ny);
            }
        }
    }

    // ����Ʈ �������� ����
    void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rand.Next(n + 1);
            T temp = list[k];
            list[k] = list[n];
            list[n] = temp;
        }
    }

    // ��ǥ üũ
    bool IsInBounds(int x, int y)
    {
        return x > 0 && x < width - 1 && y > 0 && y < height - 1;
    }

    // ���Ա��� Ż�ⱸ�� ����
    void CreateEntranceAndExit()
    {
        // ���Ա��� ���� ��ܿ� ��ġ (1,0)
        maze[1, 0] = 1;
        // ���Ա��� ���� (1,1)�� �̹� ��� ������

        // Ż�ⱸ�� ������ �ϴܿ� ��ġ (width-2, height-1)
        maze[width - 2, height - 1] = 1;
        // Ż�ⱸ�� ���� (width-2, height-2)�� �̹� ��� ������
    }

    // Ÿ�� ������Ʈ�� �̷� ���� �̵� ������ ��ο� ��ġ
    void PlaceTargetObject()
    {
        if (targetObject == null)
        {
            Debug.LogError("Target Object�� �Ҵ���� �ʾҽ��ϴ�.");
            return;
        }

        // ������ ��� ���� �����Ͽ� Ÿ�� ������Ʈ�� ��ġ
        List<Vector2Int> pathCells = new List<Vector2Int>();
        for (int x = 1; x < width - 1; x++)
        {
            for (int y = 1; y < height - 1; y++)
            {
                if (maze[x, y] == 1)
                {
                    pathCells.Add(new Vector2Int(x, y));
                }
            }
        }

        if (pathCells.Count == 0)
        {
            Debug.LogError("�̷ο� ��� ���� �������� �ʽ��ϴ�.");
            return;
        }

        // ������ ��� �� ����
        Vector2Int targetCell = pathCells[rand.Next(pathCells.Count)];
        targetObject.transform.position = new Vector3(targetCell.x, 0.5f, targetCell.y);
    }

    // �̷� ������ ���Ա��� ����� ���� ��ǥ�� ��ȯ
    Vector2Int GetStartCell()
    {
        return new Vector2Int(1, 1);
    }

    // Ż�ⱸ�� �� ��ǥ�� ��ȯ
    Vector2Int GetExitCell()
    {
        return new Vector2Int(width - 2, height - 2);
    }

    // Ÿ�� ������Ʈ�� �� ��ǥ�� ��ȯ
    Vector2Int GetTargetCell()
    {
        if (targetObject == null)
        {
            Debug.LogError("Target Object�� �Ҵ���� �ʾҽ��ϴ�.");
            return new Vector2Int(-1, -1);
        }

        Vector3 targetPos = targetObject.transform.position;
        int x = Mathf.RoundToInt(targetPos.x);
        int y = Mathf.RoundToInt(targetPos.z);
        return new Vector2Int(x, y);
    }

    // start�� end ���̿� ��ΰ� �����ϴ��� BFS�� Ȯ���ϰ�, ��θ� ����
    bool IsPathExists(Vector2Int start, Vector2Int end, List<Vector2Int> pathList)
    {
        if (end.x == -1 && end.y == -1) return false;

        bool[,] visited = new bool[width, height];
        Vector2Int[,] parent = new Vector2Int[width, height];
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(start);
        visited[start.x, start.y] = true;
        parent[start.x, start.y] = new Vector2Int(-1, -1); // �ʱ� �θ� ����

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();

            if (current == end)
            {
                // ��θ� �����Ͽ� pathList�� ����
                pathList.Clear();
                Vector2Int trace = end;
                while (trace != start)
                {
                    pathList.Add(trace);
                    trace = parent[trace.x, trace.y];
                }
                pathList.Add(start);
                pathList.Reverse();
                return true;
            }

            // �����¿� �̵�
            List<Vector2Int> directions = new List<Vector2Int>() {
                new Vector2Int(0, 1),
                new Vector2Int(1, 0),
                new Vector2Int(0, -1),
                new Vector2Int(-1, 0)
            };

            foreach (var dir in directions)
            {
                int nx = current.x + dir.x;
                int ny = current.y + dir.y;

                if (IsInBounds(nx, ny) && maze[nx, ny] == 1 && !visited[nx, ny])
                {
                    queue.Enqueue(new Vector2Int(nx, ny));
                    visited[nx, ny] = true;
                    parent[nx, ny] = current;
                }
            }
        }

        return false;
    }

    // �̷� ����
    void CreateMaze()
    {
        // ���� �̷� �θ� ������Ʈ�� ������ �����Ͽ� �ߺ� ������ ����
        GameObject existingMaze = GameObject.Find("Maze");
        if (existingMaze != null)
        {
            Destroy(existingMaze);
        }

        // �θ� ������Ʈ ���� (���� ���� ������ ����)
        GameObject mazeParent = new GameObject("Maze");

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (maze[x, y] == 0)
                {
                    Vector3 position = new Vector3(x, wallHeight / 2, y);
                    GameObject wall = Instantiate(wallPrefab, position, Quaternion.identity, mazeParent.transform);
                    wall.transform.localScale = new Vector3(wallScale, wallHeight, wallScale);
                }
            }
        }

        if (targetObject != null)
        {
            Renderer targetRenderer = targetObject.GetComponent<Renderer>();
            if (targetRenderer != null && pathMaterial != null)
            {
                targetRenderer.material = pathMaterial;
            }
        }
    }

    // ��� �ð�ȭ�� ���� LineRenderer �ʱ�ȭ
    void InitializeLineRenderers()
    {
        // Ż�ⱸ ���
        GameObject exitLineObj = new GameObject("ExitPathLine");
        exitLineRenderer = exitLineObj.AddComponent<LineRenderer>();
        exitLineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        exitLineRenderer.startColor = exitPathColor;
        exitLineRenderer.endColor = exitPathColor;
        exitLineRenderer.startWidth = 0.2f;
        exitLineRenderer.endWidth = 0.2f;
        exitLineRenderer.positionCount = 0;

        // Ÿ�� ������Ʈ ���
        GameObject targetLineObj = new GameObject("TargetPathLine");
        targetLineRenderer = targetLineObj.AddComponent<LineRenderer>();
        targetLineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        targetLineRenderer.startColor = targetPathColor;
        targetLineRenderer.endColor = targetPathColor;
        targetLineRenderer.startWidth = 0.2f;
        targetLineRenderer.endWidth = 0.2f;
        targetLineRenderer.positionCount = 0;
    }

    // ���Ա� -> Ż�ⱸ ��� �ð�ȭ
    void VisualizeExitPath()
    {
        if (exitLineRenderer == null || exitPath == null || exitPath.Count == 0)
            return;

        exitLineRenderer.positionCount = exitPath.Count;
        for (int i = 0; i < exitPath.Count; i++)
        {
            Vector2Int cell = exitPath[i];
            exitLineRenderer.SetPosition(i, new Vector3(cell.x, 0.5f, cell.y));
        }
    }

    // ���Ա� -> Ÿ�� ������Ʈ ��� �ð�ȭ
    void VisualizeTargetPath()
    {
        if (targetLineRenderer == null || targetPath == null || targetPath.Count == 0)
            return;

        targetLineRenderer.positionCount = targetPath.Count;
        for (int i = 0; i < targetPath.Count; i++)
        {
            Vector2Int cell = targetPath[i];
            targetLineRenderer.SetPosition(i, new Vector3(cell.x, 0.5f, cell.y));
        }
    }

    // ���Ա� -> Ż�ⱸ ��� ã��
    public void FindPathToExit()
    {
        Vector2Int start = GetStartCell();
        Vector2Int end = GetExitCell();

        bool pathExists = IsPathExists(start, end, exitPath);

        if (pathExists)
        {
            Debug.Log("Ż�ⱸ������ ��θ� ã�ҽ��ϴ�.");
            VisualizeExitPath();
        }
        else
        {
            Debug.LogError("���Ա����� Ż�ⱸ������ ��ΰ� �������� �ʽ��ϴ�.");
        }
    }

    // ���Ա� -> Ÿ�� ������Ʈ ��� ã��
    public void FindPathToTarget()
    {
        if (targetObject == null)
        {
            Debug.LogError("Target Object�� �Ҵ���� �ʾҽ��ϴ�.");
            return;
        }

        Vector2Int start = GetStartCell();
        Vector2Int end = GetTargetCell();

        if (end.x == -1 && end.y == -1)
        {
            Debug.LogError("Ÿ�� ������Ʈ�� ��ġ�� ��ȿ���� �ʽ��ϴ�.");
            return;
        }

        bool pathExists = IsPathExists(start, end, targetPath);

        if (pathExists)
        {
            Debug.Log("Ÿ�� ������Ʈ������ ��θ� ã�ҽ��ϴ�.");
            VisualizeTargetPath();
        }
        else
        {
            Debug.LogError("���Ա����� Ÿ�� ������Ʈ������ ��ΰ� �������� �ʽ��ϴ�.");
        }
    }

    void OnDrawGizmos()
    {
        if (maze == null) return;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (maze[x, y] == 0)
                {
                    Gizmos.color = Color.black;
                }
                else
                {
                    Gizmos.color = Color.white;
                }
                Vector3 position = new Vector3(x, 0, y);
                Gizmos.DrawCube(position, Vector3.one * 0.9f);
            }
        }

        // ���Ա��� Ż�ⱸ ���� ����
        Vector2Int entrance = new Vector2Int(1, 0);
        Vector2Int exit = new Vector2Int(width - 2, height - 1);

        Gizmos.color = Color.green;
        Gizmos.DrawCube(new Vector3(entrance.x, 0, entrance.y), Vector3.one * 0.9f);

        Gizmos.color = Color.red;
        Gizmos.DrawCube(new Vector3(exit.x, 0, exit.y), Vector3.one * 0.9f);

        // Ÿ�� ������Ʈ ���� ����
        if (targetObject != null)
        {
            Vector3 targetPos = targetObject.transform.position;
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(new Vector3(targetPos.x, 0, targetPos.z), Vector3.one * 0.9f);
        }

        // ��� �ð�ȭ
        if (exitPath != null && exitPath.Count > 0)
        {
            Gizmos.color = exitPathColor;
            foreach (var cell in exitPath)
            {
                Vector3 pathPosition = new Vector3(cell.x, 0.5f, cell.y);
                Gizmos.DrawSphere(pathPosition, 0.3f);
            }
        }

        if (targetPath != null && targetPath.Count > 0)
        {
            Gizmos.color = targetPathColor;
            foreach (var cell in targetPath)
            {
                Vector3 pathPosition = new Vector3(cell.x, 0.5f, cell.y);
                Gizmos.DrawSphere(pathPosition, 0.3f);
            }
        }
    }
}
