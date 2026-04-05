using UnityEngine;

public class Explosion : MonoBehaviour
{
    public LayerMask targetLayer;
    public Vector2 explosionCenter;
    public void FireExplosion()
    {
        Destroy(gameObject);
        var hits = Physics2D.OverlapBoxAll(
            explosionCenter,
            new Vector2(3f, 3f),
            0f,
            targetLayer
        );

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<Human>(out var h))
            {
                h.Burned();
            }
        }
        Debug.Log("FireExplosion");
    }

    public void WaterExplosion()
    {
        Destroy(gameObject);
        Debug.Log("WaterExplosion");
        foreach (var human in Human.All)
        {
            human.Slowed(5);
        }
    }

}
