using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MazeGenerator : MonoBehaviour
{
    [Header("미로 세팅")]
    public int width = 21;                                          // 미로의 가로 크기 (홀수 권장)
    public int height = 21;                                         // 미로의 세로 크기 (홀수 권장)
    public GameObject wallPrefab;                                   // 벽으로 사용할 프리팹
    public float wallHeight = 2f;                                   // 벽의 높이
    public float wallScale = 1f;                                    // 벽의 스케일

    [Header("타겟 세팅")]
    public GameObject targetObject;                                 // 목표로 할 오브젝트
    public Color exitPathColor = Color.blue;                        // 탈출구 경로 색상
    public Color targetPathColor = Color.yellow;                    // 타겟 오브젝트 경로 색상

    [Header("경로 시각화")]
    public Material pathMaterial;                                   // LineRenderer에 사용할 머티리얼

    private int[,] maze; // 미로 데이터 배열
    private System.Random rand = new System.Random();

    private List<Vector2Int> exitPath = new List<Vector2Int>();     // 출입구에서 탈출구까지의 경로
    private List<Vector2Int> targetPath = new List<Vector2Int>();   // 출입구에서 타겟까지의 경로

    private LineRenderer exitLineRenderer;                          // 탈출구 경로 LineRenderer
    private LineRenderer targetLineRenderer;                        // 타겟 오브젝트 경로 LineRenderer

    void Start()
    {
        // 홀수로 조정
        if (width % 2 == 0) width += 1;
        if (height % 2 == 0) height += 1;

        GenerateMaze();
        CreateEntranceAndExit();
        PlaceTargetObject();

        CreateMaze();
        InitializeLineRenderers();
    }

    // 미로 생성 (DFS)
    void GenerateMaze()
    {
        maze = new int[width, height];

        // 모든 셀을 벽으로 초기화
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                maze[x, y] = 0;

        // 시작점 설정
        Vector2Int start = new Vector2Int(1, 1);
        GenerateMazeDFS(start.x, start.y);
    }

    // (DFS) 미로 생성 재귀 함수
    void GenerateMazeDFS(int x, int y)
    {
        maze[x, y] = 1; // 길로 설정

        // 상하좌우 방향 (2칸씩 이동)
        List<Vector2Int> directions = new List<Vector2Int>() {
            new Vector2Int(0, 2),
            new Vector2Int(2, 0),
            new Vector2Int(0, -2),
            new Vector2Int(-2, 0)
        };

        // 방향을 랜덤하게 섞기
        Shuffle(directions);

        foreach (var dir in directions)
        {
            int nx = x + dir.x;
            int ny = y + dir.y;

            if (IsInBounds(nx, ny) && maze[nx, ny] == 0)
            {
                // 벽을 제거하여 길을 연결
                maze[x + dir.x / 2, y + dir.y / 2] = 1;
                GenerateMazeDFS(nx, ny);
            }
        }
    }

    // 리스트 랜덤으로 섞기
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

    // 좌표 체크
    bool IsInBounds(int x, int y)
    {
        return x > 0 && x < width - 1 && y > 0 && y < height - 1;
    }

    // 출입구와 탈출구를 생성
    void CreateEntranceAndExit()
    {
        // 출입구를 왼쪽 상단에 위치 (1,0)
        maze[1, 0] = 1;
        // 출입구와 연결 (1,1)은 이미 길로 설정됨

        // 탈출구를 오른쪽 하단에 위치 (width-2, height-1)
        maze[width - 2, height - 1] = 1;
        // 탈출구와 연결 (width-2, height-2)은 이미 길로 설정됨
    }

    // 타겟 오브젝트를 미로 내의 이동 가능한 경로에 배치
    void PlaceTargetObject()
    {
        if (targetObject == null)
        {
            Debug.LogError("Target Object가 할당되지 않았습니다.");
            return;
        }

        // 랜덤한 경로 셀을 선택하여 타겟 오브젝트를 배치
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
            Debug.LogError("미로에 경로 셀이 존재하지 않습니다.");
            return;
        }

        // 랜덤한 경로 셀 선택
        Vector2Int targetCell = pathCells[rand.Next(pathCells.Count)];
        targetObject.transform.position = new Vector3(targetCell.x, 0.5f, targetCell.y);
    }

    // 미로 내부의 출입구와 연결된 셀의 좌표를 반환
    Vector2Int GetStartCell()
    {
        return new Vector2Int(1, 1);
    }

    // 탈출구의 셀 좌표를 반환
    Vector2Int GetExitCell()
    {
        return new Vector2Int(width - 2, height - 2);
    }

    // 타겟 오브젝트의 셀 좌표를 반환
    Vector2Int GetTargetCell()
    {
        if (targetObject == null)
        {
            Debug.LogError("Target Object가 할당되지 않았습니다.");
            return new Vector2Int(-1, -1);
        }

        Vector3 targetPos = targetObject.transform.position;
        int x = Mathf.RoundToInt(targetPos.x);
        int y = Mathf.RoundToInt(targetPos.z);
        return new Vector2Int(x, y);
    }

    // start와 end 사이에 경로가 존재하는지 BFS로 확인하고, 경로를 저장
    bool IsPathExists(Vector2Int start, Vector2Int end, List<Vector2Int> pathList)
    {
        if (end.x == -1 && end.y == -1) return false;

        bool[,] visited = new bool[width, height];
        Vector2Int[,] parent = new Vector2Int[width, height];
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(start);
        visited[start.x, start.y] = true;
        parent[start.x, start.y] = new Vector2Int(-1, -1); // 초기 부모 설정

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();

            if (current == end)
            {
                // 경로를 추적하여 pathList에 저장
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

            // 상하좌우 이동
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

    // 미로 생성
    void CreateMaze()
    {
        // 기존 미로 부모 오브젝트가 있으면 제거하여 중복 생성을 방지
        GameObject existingMaze = GameObject.Find("Maze");
        if (existingMaze != null)
        {
            Destroy(existingMaze);
        }

        // 부모 오브젝트 생성 (계층 구조 정리를 위함)
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

    // 경로 시각화를 위한 LineRenderer 초기화
    void InitializeLineRenderers()
    {
        // 탈출구 경로
        GameObject exitLineObj = new GameObject("ExitPathLine");
        exitLineRenderer = exitLineObj.AddComponent<LineRenderer>();
        exitLineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        exitLineRenderer.startColor = exitPathColor;
        exitLineRenderer.endColor = exitPathColor;
        exitLineRenderer.startWidth = 0.2f;
        exitLineRenderer.endWidth = 0.2f;
        exitLineRenderer.positionCount = 0;

        // 타겟 오브젝트 경로
        GameObject targetLineObj = new GameObject("TargetPathLine");
        targetLineRenderer = targetLineObj.AddComponent<LineRenderer>();
        targetLineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        targetLineRenderer.startColor = targetPathColor;
        targetLineRenderer.endColor = targetPathColor;
        targetLineRenderer.startWidth = 0.2f;
        targetLineRenderer.endWidth = 0.2f;
        targetLineRenderer.positionCount = 0;
    }

    // 출입구 -> 탈출구 경로 시각화
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

    // 출입구 -> 타겟 오브젝트 경로 시각화
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

    // 출입구 -> 탈출구 경로 찾기
    public void FindPathToExit()
    {
        Vector2Int start = GetStartCell();
        Vector2Int end = GetExitCell();

        bool pathExists = IsPathExists(start, end, exitPath);

        if (pathExists)
        {
            Debug.Log("탈출구까지의 경로를 찾았습니다.");
            VisualizeExitPath();
        }
        else
        {
            Debug.LogError("출입구에서 탈출구까지의 경로가 존재하지 않습니다.");
        }
    }

    // 출입구 -> 타겟 오브젝트 경로 찾기
    public void FindPathToTarget()
    {
        if (targetObject == null)
        {
            Debug.LogError("Target Object가 할당되지 않았습니다.");
            return;
        }

        Vector2Int start = GetStartCell();
        Vector2Int end = GetTargetCell();

        if (end.x == -1 && end.y == -1)
        {
            Debug.LogError("타겟 오브젝트의 위치가 유효하지 않습니다.");
            return;
        }

        bool pathExists = IsPathExists(start, end, targetPath);

        if (pathExists)
        {
            Debug.Log("타겟 오브젝트까지의 경로를 찾았습니다.");
            VisualizeTargetPath();
        }
        else
        {
            Debug.LogError("출입구에서 타겟 오브젝트까지의 경로가 존재하지 않습니다.");
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

        // 출입구와 탈출구 색상 구분
        Vector2Int entrance = new Vector2Int(1, 0);
        Vector2Int exit = new Vector2Int(width - 2, height - 1);

        Gizmos.color = Color.green;
        Gizmos.DrawCube(new Vector3(entrance.x, 0, entrance.y), Vector3.one * 0.9f);

        Gizmos.color = Color.red;
        Gizmos.DrawCube(new Vector3(exit.x, 0, exit.y), Vector3.one * 0.9f);

        // 타겟 오브젝트 색상 구분
        if (targetObject != null)
        {
            Vector3 targetPos = targetObject.transform.position;
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(new Vector3(targetPos.x, 0, targetPos.z), Vector3.one * 0.9f);
        }

        // 경로 시각화
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
