using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingVisualizer : MonoBehaviour
{
    public GameObject cubePrefab; // ť�� ������
    public int numberOfCubes = 10; // ť�� ����
    public float width = 2f; // ť�� ����
    public float startX = -5f; // ť�갡 �����ϴ� X ��ġ (������)
    public float startZ = 0f;  // ť�갡 �����ϴ� Z ��ġ (������)

    public List<GameObject> cubes = new List<GameObject>();
    public List<int> heights = new List<int>();

    void Start()
    {
        GenerateCubes();
    }

    // ť�긦 �����ϰ� ���̸� �������� ����
    void GenerateCubes()
    {
        for (int i = 0; i < numberOfCubes; i++)
        {
            int height = Random.Range(1, 20); // ť���� ���̴� 1~20 ����
            heights.Add(height);

            // �� ť���� �ʱ� X ��ġ�� startX + (i * width)�� ����
            float xPosition = startX + i * width;

            // ť�� ���� ��ġ: X�� �����ϰ�, Y�� �������� ���� ����
            GameObject cube = Instantiate(cubePrefab, new Vector3(xPosition, height / 2f, startZ), Quaternion.identity);
            cube.transform.localScale = new Vector3(1, height, 1);

            // ť�꿡 ���� ���� ����
            cube.GetComponent<Renderer>().material.color = new Color(Random.value, Random.value, Random.value);

            cubes.Add(cube);
        }
    }

    // ť����� ��ġ�� ������Ʈ�ϴ� �Լ�
    public void UpdateCubePositions()
    {
        for (int i = 0; i < cubes.Count; i++)
        {
            cubes[i].transform.position = new Vector3(cubes[i].transform.position.x, heights[i] / 2f, startZ);
            cubes[i].transform.localScale = new Vector3(1, heights[i], 1);
        }
    }

    // ť����� ������ �����ϴ� �ڷ�ƾ
    public IEnumerator PerformSort(IEnumerator sortAlgorithm)
    {
        yield return StartCoroutine(sortAlgorithm);
    }
}
