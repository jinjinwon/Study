using UnityEngine;

public class Character : MonoBehaviour
{
    public void Move(Vector3 position)
    {
        transform.position = position;
    }
}
