using UnityEngine;

public class Vampire : MonoBehaviour
{
    private Human target;
    public LayerMask attackMask;
    private Animator animator;
    private Defender myDefender;

    public float range;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        myDefender = gameObject.GetComponent<Defender>();
    }

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, range, attackMask);

        if (hit.collider)
        {
            target = hit.collider.GetComponent<Human>();
            animator.SetTrigger("tgAttacking");
        }
    }

    public void Attacking()
    {
        if (target)
        {
            Destroy(target.gameObject);
        }
        myDefender.ClearTile();
        Destroy(gameObject);
    }

}
