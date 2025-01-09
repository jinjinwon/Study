using UnityEngine;

public class SearchableObject : MonoBehaviour
{
    public string objectName;

    private void Start()
    {
        gameObject.name = objectName;
    }
}