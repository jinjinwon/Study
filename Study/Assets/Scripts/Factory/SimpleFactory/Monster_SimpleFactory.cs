using UnityEngine;

public class Slime : IEnemy_SimpleFactory
{
    public void Attack()
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        cube.transform.position = new Vector3(0, 1, 0);
        cube.transform.localScale = new Vector3(2, 2, 2);

        Renderer renderer = cube.GetComponent<Renderer>();
        renderer.material.color = Color.red;

        cube.name = "Slime";

        Debug.Log("Slime attacks by jumping!");
    }
}

public class Goblin : IEnemy_SimpleFactory
{
    public void Attack()
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        cube.transform.position = new Vector3(0, -2, 0);
        cube.transform.localScale = new Vector3(2, 2, 2);

        Renderer renderer = cube.GetComponent<Renderer>();
        renderer.material.color = Color.blue;

        cube.name = "Goblin";

        Debug.Log("Goblin attacks with a club!");
    }
}

public class Dragon : IEnemy_SimpleFactory
{
    public void Attack()
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        cube.transform.position = new Vector3(0, 3, 0);
        cube.transform.localScale = new Vector3(2, 2, 2);

        Renderer renderer = cube.GetComponent<Renderer>();
        renderer.material.color = Color.green;

        cube.name = "Dragon";

        Debug.Log("Dragon attacks with fire breath!");
    }
}