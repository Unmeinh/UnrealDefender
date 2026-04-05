using UnityEngine;

public class Shaman : MonoBehaviour
{
    public GameObject goldObject;
    private Animator animator;

    public float cooldown;
    public bool canSpawn;

    private void Start() {
        animator = gameObject.GetComponent<Animator>();
        animator.SetTrigger("tgBeingIdle");
        InvokeRepeating(nameof(OnSpawnGold), cooldown, cooldown);
    }

    public void OnBeingIdle()
    {
        animator.SetTrigger("tgBeingIdle");
        SpawnGold();
    }

   public void OnSpawnGold()
    {
        animator.SetTrigger("tgUsingMagic");
    }

    void SpawnGold()
    {
        GameObject newGold = Instantiate(goldObject, new Vector3(transform.position.x + Random.Range(-.3f, .3f), transform.position.y + Random.Range(0f, 0.75f), 0), Quaternion.identity);
        newGold.GetComponent<Gold>().dropToYPos = transform.position.y - 0.65f;
    }
}
