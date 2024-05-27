using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed = 150f;
    public float damage = 0f;

    public void SetDirection(Vector3 dir)
    {
        transform.right = dir;
    }

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.gameObject.CompareTag("Enemy"))
        {
            DamageFlash damageFlash = hitInfo.GetComponent<DamageFlash>();
            if (damageFlash != null)
            {
                damageFlash.CallDamageFlash();
                damageFlash.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}
