using UnityEngine;

public class DefendTrigger : MonoBehaviour
{
    public GameObject magicDefend;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Human>(out Human human))
        {
            human.spd = 0f;
            Destroy(gameObject);
            GameObject magicObject = Instantiate(magicDefend, transform.position, Quaternion.identity);
            // explosionObject.GetComponent<Explosion>().explosionCenter = transform.position;
        }
    }
}
