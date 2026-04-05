using UnityEngine;

public class Kitsune : MonoBehaviour
{
    private Human target;
    public LayerMask attackMask;
    private Animator animator;
    private Defender myDefender;

    public float range;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        myDefender = gameObject.GetComponent<Defender>();
        transform.position += new Vector3(0, 0.025f, 0);
    }

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, range, attackMask);

        if (hit.collider)
        {
            target = hit.collider.GetComponent<Human>();
            animator.SetTrigger("tgCharming");
        }
    }

    public void Charming()
    {
        if (!target)
            return;
        target.Charmed();
            myDefender.ClearTile();
        Destroy(gameObject);
    }

}
