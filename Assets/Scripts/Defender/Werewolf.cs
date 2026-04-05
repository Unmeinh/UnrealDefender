using UnityEngine;

public class Werewolf : MonoBehaviour
{
    private Human target;
    public LayerMask attackMask;
    private Animator animator;

    public int damage;
    public float cooldown;
    public bool canAttack;
    public float range;

    private void Start()
    {
        ResetCooldown();
        animator = gameObject.GetComponent<Animator>();
        animator.SetTrigger("tgBeingIdle");
    }

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, range, attackMask);

        if (hit.collider)
        {
            target = hit.collider.GetComponent<Human>();
            OnAttacking();
        }
    }

    void ResetCooldown()
    {
        canAttack = true;
    }

    public void Attacking()
    {
        if (!target)
        {
            animator.SetTrigger("tgBeingIdle");
            return;
        }
        target.Hitted(damage);
        animator.SetTrigger("tgHeavyAttacking");
    }

    public void FollowAttacking()
    {
        if (!target)
        {
            animator.SetTrigger("tgBeingIdle");
            return;
        }
        target.Hitted(damage + damage % 3);
        animator.SetTrigger("tgBeingIdle");
    }

    void OnAttacking()
    {
        if (!canAttack)
            return;
        canAttack = false;
        Invoke(nameof(ResetCooldown), cooldown);
        animator.SetTrigger("tgAttacking");
    }
}
