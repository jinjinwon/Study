using UnityEngine;

[CreateAssetMenu(fileName = "DropFruitCommand", menuName = "AI/Commands/DropFruit")]
public class DropFruitCommand : Command
{
    public GameObject fruitPrefab;

    public override void Execute(Transform aiTransform, Transform target = null, Vector3? position = null)
    {
        Debug.Log($"{aiTransform.name}��(��) ���Ÿ� ����߸��ϴ�!");
        GameObject.Instantiate(fruitPrefab, aiTransform.position + Vector3.down, Quaternion.identity);
    }

    public override bool CanExecute(Transform aiTransform, Transform target = null)
    {
        return true; // �⺻������ ���� ����
    }
}
