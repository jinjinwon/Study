using UnityEngine;

[CreateAssetMenu(fileName = "DropFruitCommand", menuName = "AI/Commands/DropFruit")]
public class DropFruitCommand : Command
{
    public GameObject fruitPrefab;

    public override void Execute(Transform aiTransform, Transform target = null, Vector3? position = null)
    {
        Debug.Log($"{aiTransform.name}이(가) 열매를 떨어뜨립니다!");
        GameObject.Instantiate(fruitPrefab, aiTransform.position + Vector3.down, Quaternion.identity);
    }

    public override bool CanExecute(Transform aiTransform, Transform target = null)
    {
        return true; // 기본적으로 실행 가능
    }
}
