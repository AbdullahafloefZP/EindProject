using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed = 150f;

    public void SetDirection(Vector3 dir)
    {
        transform.right = dir;
    }

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        Destroy(gameObject, 3f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
