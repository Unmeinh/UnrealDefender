using UnityEngine;

public class Minotaur : MonoBehaviour
{
    public int damage;
    public float range;

    private Animator animator;
    public LayerMask targetMask;
    private Defender myDefender;
    private RaycastHit2D[] oldHits;

    private void Start()
    {
        myDefender = gameObject.GetComponent<Defender>();
        animator = gameObject.GetComponent<Animator>();
        animator.SetTrigger("tgBeingIdle");
        transform.position += new Vector3(0, 0.12f, 0);
    }


    private void Update()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.right, range, targetMask);

        if (myDefender.hitCounter >= 5)
        {
            oldHits = hits;
            animator.SetTrigger("tgAttacking");
            myDefender.hitCounter = 0;
        }
    }

    public void OnHittingEnd()
    {
        OnHitting();
        animator.SetTrigger("tgBeingIdle");
    }

    public void Hitting(Human target)
    {
        if (!target)
            return;

        // Vector2 knockbackDir = (target.transform.position - transform.position).normalized;
        target.Hitted(damage, true);
    }

    public void OnHitting()
    {
        foreach (var hit in oldHits)
        {
            if (hit.collider)
            {
                Human target = hit.collider.GetComponent<Human>();
                Hitting(target);
            }
        }
    }
}
