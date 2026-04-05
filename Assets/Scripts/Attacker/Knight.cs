using UnityEngine;

public class Knight : MonoBehaviour
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
            myHuman.isArmored = true;
            return;
        }
        myHuman.isArmored = true;
        HittingEnemy(baseDamage);
        animator.SetTrigger("tgWalking");
    }

    public void HeavyAttacking()
    {
        GetMyTarget();
        if (!CheckingEnemy())
        {
            myHuman.isArmored = true;
            return;
        }
        myHuman.isArmored = true;
        HittingEnemy(baseDamage * (int)combatConfig.heavyAttackMultiplier);
        animator.SetTrigger("tgWalking");
    }

    void OnAttacking()
    {
        if (canAttack && (huTarget || defTarget))
        {
            canAttack = false;
            myHuman.isArmored = false;
            Invoke(nameof(ResetCooldown), cooldown);
            bool isHeavyAttacking = Random.value < combatConfig.heavyAttackChance;

            if (isHeavyAttacking)
                animator.SetTrigger("tgHeavyAttacking");
            else
                animator.SetTrigger("tgAttacking");
        }
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
