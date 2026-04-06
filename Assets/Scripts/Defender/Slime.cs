using System.Collections;
using UnityEngine;

public class Slime : MonoBehaviour
{
    public GameObject explosion;
    public Transform explosionOrg;
    private Defender myDefender;
    public AudioClip explosionSFX;

    private void Start()
    {
        myDefender = gameObject.GetComponent<Defender>();
        StartCoroutine(ScaleUp());
    }

    private void OnExplosion()
    {
        myDefender.ClearTile();
        Destroy(gameObject);
        GameObject explosionObject = Instantiate(explosion, explosionOrg.position, Quaternion.identity);
        explosionObject.GetComponent<Explosion>().explosionCenter = transform.position;
        if (explosionObject.TryGetComponent<AudioSource>(out var audio)) 
            audio.PlayOneShot(explosionSFX);
    }

    IEnumerator ScaleUp()
    {
        Vector3 start = new(1.5f, 1.5f, 1f);
        Vector3 target = new(3.5f, 3.5f, 1f);

        float duration = 0.5f;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            transform.localScale = Vector3.Lerp(start, target, t);

            yield return null;
        }

        transform.localScale = target;
        OnExplosion();
    }
}
