using UnityEngine;

public class DefMagic : MonoBehaviour
{
    
    private void Update() {
        transform.position += new Vector3(1f * Time.deltaTime, 0, 0);
        if (transform.position.x >= 8f)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Human>(out Human human))
        {
            human.spd = 0f;
            human.Hitted(999);
        }
    }
}
