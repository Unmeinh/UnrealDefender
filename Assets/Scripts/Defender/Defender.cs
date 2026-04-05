using UnityEngine;

public class Defender : MonoBehaviour
{
    public int hp;
    public int hitCounter;
    public bool useLastDance;
    public bool triggerLastDance;
    public LayerMask tileMask;
    private GameManager gmg;

    private void Start()
    {
        triggerLastDance = false;
        gmg = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameObject.layer = 9;
    }


    public void Hitted(int damage)
    {
        hp -= damage;
        hitCounter++;
        if (hp <= 0)
        {
            ClearTile();
            triggerLastDance = true;
            if (useLastDance)
                return;
            Destroy(gameObject);
            // change hasDefender
        }
    }

    public void ClearTile()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.001f, tileMask);
        if (hit.collider && hit.collider.TryGetComponent<Tile>(out Tile tile))
        {
            tile.hasDefender = false;
        }
    }
}
