using UnityEngine;

public class Homeless : MonoBehaviour
{
    public HumanType humanType;
    private Human myHuman;
    private Animator animator;
    private Defender defTarget;
    private Human huTarget;

    private int baseDamage = 0;
    private float cooldown = 0;
    private bool canAttack;

    private void Start()
    {
        ResetCooldown();
        myHuman = gameObject.GetComponent<Human>();
        animator = gameObject.GetComponent<Animator>();
        animator.SetTrigger("tgWalking");
    }

    private void Awake()
    {
        if (humanType)
        {
            baseDamage = humanType.damage;
            cooldown = humanType.hitCooldown;
        }
    }

    private void Update()
    {
        if (canAttack)
            GetMyTarget();
    }

    void ResetCooldown()
    {
        canAttack = true;
    }

    void GetMyTarget()
    {
        Component myTarget = myHuman.GetCurTarget();
        if (!myTarget)
            return;
        Defender defenderTarget = myTarget.GetComponent<Defender>();
        Human humanTarget = myTarget.GetComponent<Human>();
        if (defenderTarget)
            defTarget = defenderTarget;
        if (humanTarget)
            huTarget = humanTarget;
        OnAttacking();
    }

    public void Attacking()
    {
        animator.SetTrigger("tgWalking");
    }

    void OnAttacking()
    {
        if (canAttack && (huTarget || defTarget))
        {
            canAttack = false;
            Invoke(nameof(ResetCooldown), cooldown);
            animator.SetTrigger("tgAttacking");
            if (huTarget)
                huTarget.Hitted(baseDamage);
            else if (defTarget)
                defTarget.Hitted(baseDamage);
        }
    }
}
