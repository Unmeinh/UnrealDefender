using UnityEngine;

public class Swordman : MonoBehaviour
{
    public HumanType humanType;
    private Human myHuman;
    private Animator animator;
    private Defender defTarget;
    private Human huTarget;
    public AudioClip enableSFX, disableSFX;
    private AudioSource audioSource;

    private int baseDamage;
    private float cooldown;
    private bool canAttack;
    private bool isEnable = true;
    private int hitCounter = 0;

    private void Start()
    {
        ResetCooldown();
        myHuman = gameObject.GetComponent<Human>();
        animator = gameObject.GetComponent<Animator>();
        audioSource = gameObject.GetComponent<AudioSource>();
        animator.SetTrigger("tgWalking");
        baseDamage = humanType.damage;
        cooldown = humanType.hitCooldown;
    }

    private void Update()
    {
        if (canAttack && isEnable)
        {
            GetMyTarget();
            OnAttacking();
        }
        if (hitCounter >= 2)
        {
            audioSource.PlayOneShot(disableSFX);
            animator.SetTrigger("tgShuttingDown");
            hitCounter = 0;
            isEnable = false;
            myHuman.spd = 0f;
            Invoke(nameof(OnEnabling), 5);
        }
    }

    public void Enabling()
    {
        isEnable = true;
        myHuman.spd = humanType.spd;
        animator.SetTrigger("tgWalking");
    }

    void OnEnabling()
    {
        audioSource.PlayOneShot(enableSFX);
        animator.SetTrigger("tgEnabling");
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
            return;
        HittingEnemy(baseDamage);
        animator.SetTrigger("tgHeavyAttacking");
    }

    public void FollowAttacking()
    {
        GetMyTarget();
        if (!CheckingEnemy())
            return;
        HittingEnemy(baseDamage + baseDamage % 4);
        animator.SetTrigger("tgWalking");
        hitCounter++;
    }

    void OnAttacking()
    {
        if (canAttack && (huTarget || defTarget))
        {
            canAttack = false;
            Invoke(nameof(ResetCooldown), cooldown);
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
