using UnityEngine;

public class Gordon : MonoBehaviour
{
    private Human target;
    public LayerMask shootMask;
    private Animator animator;
    private Defender myDefender;

    public int damage;
    public float cooldown;
    public float cooldownSpecial;
    public bool canAttack;
    public bool canSpecialAttack;
    public float range;

    private void Start()
    {
        canAttack = false;
        ResetCooldownSpecial();
        animator = gameObject.GetComponent<Animator>();
        myDefender = gameObject.GetComponent<Defender>();
        animator.SetTrigger("tgBeingIdle");
        myDefender.useLastDance = true;
        transform.position += new Vector3(0, 0.025f, 0);
    }

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, range, shootMask);

        if (hit.collider)
        {
            target = hit.collider.GetComponent<Human>();
            if (myDefender.triggerLastDance)
                animator.SetTrigger("tgSpecialSkill");
            else
                OnAttacking();
        }
    }

    void ResetCooldown()
    {
        canAttack = true;
    }

    void ResetCooldownSpecial()
    {
        canSpecialAttack = true;
    }


    public void Attacking()
    {
        target.Hitted(damage);
        animator.SetTrigger("tgBeingIdle");
    }

    public void SpecialSkill()
    {
        target.Petrified();
        if (myDefender.triggerLastDance)
        {
            myDefender.ClearTile();
            Destroy(gameObject);
            return;
        }
        animator.SetTrigger("tgBeingIdle");
        Invoke(nameof(ResetCooldown), cooldown);
    }

    void OnAttacking()
    {
        if (canSpecialAttack)
        {
            canSpecialAttack = false;
            Invoke(nameof(ResetCooldownSpecial), cooldownSpecial);
            animator.SetTrigger("tgSpecialSkill");
        }
        else if (canAttack)
        {
            canAttack = false;
            Invoke(nameof(ResetCooldown), cooldown);
            animator.SetTrigger("tgAttacking");
        }
    }
}
