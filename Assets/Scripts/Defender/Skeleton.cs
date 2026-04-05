using System.Collections;
using UnityEngine;

public class BasicSkeletonShooter : MonoBehaviour
{
    [SerializeField] CombatConfig combatConfig;
    public GameObject arrow;
    private GameObject target;
    public Transform shootOrg;
    public LayerMask shootMask;
    private Animator animator;

    public float cooldown;
    public bool canShoot;
    public float range;

    private void Start()
    {
        ResetCooldown();
        animator = gameObject.GetComponent<Animator>();
        animator.SetTrigger("tgBeingIdle");
    }

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, range, shootMask);

        if (hit.collider)
        {
            target = hit.collider.gameObject;

            OnShooting();
        }
    }

    void ResetCooldown()
    {
        canShoot = true;
    }


    public void Shooting()
    {
        GameObject newArrow = Instantiate(arrow, shootOrg.position, Quaternion.identity);
        animator.SetTrigger("tgBeingIdle");
    }

    public void HeavyShooting()
    {
        GameObject newArrow = Instantiate(arrow, shootOrg.position, Quaternion.identity);
        animator.SetTrigger("tgBeingIdle");
        newArrow.GetComponent<Arrow>().damage *= (int)combatConfig.heavyAttackMultiplier;
    }

    void OnShooting()
    {
        if (!canShoot)
            return;
        canShoot = false;
        Invoke(nameof(ResetCooldown), cooldown);
        bool isHeavyShooting = Random.value < combatConfig.heavyAttackChance;

        if (isHeavyShooting)
            animator.SetTrigger("tgHeavyShooting");
        else
            animator.SetTrigger("tgShooting");

    }

}
