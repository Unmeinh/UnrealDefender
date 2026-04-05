using UnityEngine;

public class Arrow : MonoBehaviour
{
    public int damage;
    public float speed = 2f;
    public bool isFreeze = false;

    private void Update() {
        transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        if (transform.position.x >= 8f)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Human>(out Human human))
        {
            human.Hitted(damage);
            if (isFreeze)
                human.Slowed();
            Destroy(gameObject);
        }
    }
}
