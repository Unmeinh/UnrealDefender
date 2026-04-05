using UnityEngine;

public class Solder : MonoBehaviour
{
    public HumanType humanType;
    private Human myHuman;
    public GameObject bullet;
    public Transform bulletOrg;
    private Animator animator;

    public float cooldown;
    public bool canShoot;
    public float range;
    public int shootCounter;

    private void Start()
    {
        ResetCooldown();
        myHuman = gameObject.GetComponent<Human>();
        animator = gameObject.GetComponent<Animator>();
        animator.SetTrigger("tgWalking");
        cooldown = humanType.hitCooldown;
    }

    private void Update()
    {
        if (canShoot)
        {
            if (shootCounter >= 5)
            {
                OnRecharging();
                return;
            }
            OnShooting();
        }
    }

    void ResetCooldown()
    {
        canShoot = true;
    }

    public void Recharging()
    {
        shootCounter = 0;
        animator.SetTrigger("tgWalking");
        myHuman.spd = humanType.spd;
        Invoke(nameof(ResetCooldown), 3);
    }

    void OnRecharging()
    {
        myHuman.spd = 0f;
        canShoot = false;
        animator.SetTrigger("tgRecharging");
    }

    public void Shooting()
    {
        GameObject newBullet = Instantiate(bullet, bulletOrg.position, Quaternion.identity);
        newBullet.GetComponent<Bullet>().damage = humanType.damage;
        newBullet.GetComponent<Bullet>().myHuman = myHuman;
        if (myHuman.isCharmed)
            newBullet.GetComponent<Bullet>().isBetrayal = true;
        myHuman.spd = humanType.spd;
        animator.SetTrigger("tgWalking");
        shootCounter++;
    }

    void OnShooting()
    {
        Component myTarget = myHuman.GetCurTarget();
        if (canShoot && myTarget)
        {
            myHuman.spd = 0f;
            canShoot = false;
            Invoke(nameof(ResetCooldown), cooldown);
            animator.SetTrigger("tgShooting");
        }
    }

}
