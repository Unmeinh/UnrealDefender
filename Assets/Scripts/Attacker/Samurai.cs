using UnityEngine;

public class Samurai : MonoBehaviour
{
    [SerializeField] CombatConfig combatConfig;

    public HumanType humanType;
    private Human myHuman;
    private Animator animator;
    private Defender defTarget;
    private Human huTarget;

    private int baseDamage;
    private float cooldown;
    private bool canAttack;
    private bool isTripleAttack;

    private void Start()
    {
        ResetCooldown();
        myHuman = gameObject.GetComponent<Human>();
        animator = gameObject.GetComponent<Animator>();
        animator.SetTrigger("tgWalking");
        baseDamage = humanType.damage;
        cooldown = humanType.hitCooldown;
    }

    private void Update()
    {
        if (canAttack)
        {
            GetMyTarget();
            OnAttacking();
        }
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
    }

    public void Attacking()
    {
        GetMyTarget();
        if (!CheckingEnemy())
        {
            ResetAttack();
            return;
        }
        HittingEnemy(baseDamage);
        if (isTripleAttack)
        {
            ResetAttack();
        }
        else
            animator.SetTrigger("tgShortAttacking");
    }

    public void HeavyAttacking()
    {
        GetMyTarget();
        if (!CheckingEnemy())
        {
            ResetAttack();
            return;
        }
        HittingEnemy(baseDamage * (int)combatConfig.heavyAttackMultiplier);
        if (isTripleAttack)
            animator.SetTrigger("tgLightAttacking");
        else
            animator.SetTrigger("tgShortAttacking");
    }

    public void FollowAttacking()
    {
        GetMyTarget();
        if (!CheckingEnemy())
        {
            ResetAttack();
            return;
        }
        HittingEnemy(baseDamage % 2);
        if (isTripleAttack)
            animator.SetTrigger("tgHeavyAttacking");
        else
        {
            ResetAttack();
        }
    }

    void OnAttacking()
    {
        if (canAttack && (huTarget || defTarget))
        {
            canAttack = false;
            myHuman.isArmored = false;
            Invoke(nameof(ResetCooldown), cooldown);
            // bool isHeavyAttacking = Random.value < combatConfig.heavyAttackChance;
            float roll = Random.value;

            if (roll < combatConfig.tripleAttackChance)
            {
                isTripleAttack = true;
                animator.SetTrigger("tgShortAttacking");
            }
            else if (roll < combatConfig.tripleAttackChance + combatConfig.heavyAttackChance)
            {
                animator.SetTrigger("tgHeavyAttacking");
            }
            else
            {
                animator.SetTrigger("tgLightAttacking");
            }
        }
    }

    void ResetAttack()
    {
        isTripleAttack = false;
        myHuman.isArmored = true;
        animator.SetTrigger("tgWalking");
    }

    bool CheckingEnemy()
    {
        if (huTarget || defTarget)
            return true;
        return false;
    }

    void HittingEnemy(int damage)
    {
        if (huTarget)
            huTarget.Hitted(damage);
        else if (defTarget)
            defTarget.Hitted(damage);
    }
}
