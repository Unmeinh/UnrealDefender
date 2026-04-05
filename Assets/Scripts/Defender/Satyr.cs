using UnityEngine;

public class Satyr : MonoBehaviour
{
    public GameObject magic;
    private GameObject target;
    public Transform magicOrg;
    public LayerMask shootMask;
    private Animator animator;

    public float cooldown;
    public bool canShoot;
    public float range;

    private void Start() {
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
        GameObject newArrow = Instantiate(magic, magicOrg.position, Quaternion.identity);
        animator.SetTrigger("tgBeingIdle");
    }

    void OnShooting()
    {
        if (!canShoot)
            return;
        canShoot = false;
        Invoke(nameof(ResetCooldown), cooldown);
        animator.SetTrigger("tgShooting");
    }
}
