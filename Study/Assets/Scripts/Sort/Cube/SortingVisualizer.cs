using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingVisualizer : MonoBehaviour
{
    public GameObject cubePrefab; // 큐브 프리팹
    public int numberOfCubes = 10; // 큐브 개수
    public float width = 2f; // 큐브 간격
    public float startX = -5f; // 큐브가 시작하는 X 위치 (시작점)
    public float startZ = 0f;  // 큐브가 시작하는 Z 위치 (시작점)

    public List<GameObject> cubes = new List<GameObject>();
    public List<int> heights = new List<int>();

    void Start()
    {
        GenerateCubes();
    }

    // 큐브를 생성하고 높이를 랜덤으로 설정
    void GenerateCubes()
    {
        for (int i = 0; i < numberOfCubes; i++)
        {
            int height = Random.Range(1, 20); // 큐브의 높이는 1~20 범위
            heights.Add(height);

            // 각 큐브의 초기 X 위치를 startX + (i * width)로 설정
            float xPosition = startX + i * width;

            // 큐브 생성 위치: X는 일정하게, Y는 랜덤으로 높이 설정
            GameObject cube = Instantiate(cubePrefab, new Vector3(xPosition, height / 2f, startZ), Quaternion.identity);
            cube.transform.localScale = new Vector3(1, height, 1);

            // 큐브에 랜덤 색상 적용
            cube.GetComponent<Renderer>().material.color = new Color(Random.value, Random.value, Random.value);

            cubes.Add(cube);
        }
    }

    // 큐브들의 위치를 업데이트하는 함수
    public void UpdateCubePositions()
    {
        for (int i = 0; i < cubes.Count; i++)
        {
            cubes[i].transform.position = new Vector3(cubes[i].transform.position.x, heights[i] / 2f, startZ);
            cubes[i].transform.localScale = new Vector3(1, heights[i], 1);
        }
    }

    // 큐브들을 실제로 정렬하는 코루틴
    public IEnumerator PerformSort(IEnumerator sortAlgorithm)
    {
        yield return StartCoroutine(sortAlgorithm);
    }
}
