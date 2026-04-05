using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public float speed = 2f;
    public bool isBetrayal = false;
    public Human myHuman;

    private void Start() {
        if (isBetrayal)
            FlipObject();
    }

    private void Update()
    {
        if (isBetrayal)
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        else
            transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
        if (transform.position.x >= 8f || transform.position.x <= -9f)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isBetrayal && collision.TryGetComponent<Human>(out Human human))
        {
            if (human == myHuman)
                return;
            human.Hitted(damage);
            Destroy(gameObject);
        }

        if (!isBetrayal && collision.TryGetComponent<Defender>(out Defender defender))
        {
            defender.Hitted(damage);
            Destroy(gameObject);
        }

        if (!isBetrayal && collision.TryGetComponent<Arrow>(out Arrow arrow))
        {
            Destroy(arrow.gameObject);
            Destroy(gameObject);
        }
    }

    
    void FlipObject()
    {
        gameObject.transform.localScale = new Vector2(
            gameObject.transform.localScale.x * -1,
            gameObject.transform.localScale.y
        );
    }

}
