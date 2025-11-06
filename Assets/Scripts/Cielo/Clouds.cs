using UnityEngine;

public class Clouds : MonoBehaviour
{

    public Vector3 moveDirection = Vector3.forward;
    public float speed = 0.7f;
    public float LIMIT = 80000;

    void Update()
    {
        transform.position += moveDirection * speed * Time.deltaTime;
        if(transform.position.z > LIMIT)
        {
            Destroy(gameObject);
        }
    }
}
