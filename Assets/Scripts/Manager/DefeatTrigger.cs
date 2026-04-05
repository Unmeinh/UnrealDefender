using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DefeatTrigger : MonoBehaviour
{
    private static WaitForSecondsRealtime _waitForSecondsRealtime = new(7f);
    public Animator animator;
    private AudioSource audioSource;
    public AudioSource bgmSource;
    public AudioClip screamSFX;
    public AudioClip loseSFX;
    public Vector3 doorPosition;

    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            StartCoroutine(GameOverSequence(collision.transform));
            // audioSource.PlayOneShot(loseSFX);
            // animator.Play("DeathTriggered");
        }
    }

    IEnumerator GameOverSequence(Transform target)
    {
        // 1. Pause game
        Time.timeScale = 0;

        // 2. Play lose SFX
        if (bgmSource)
            bgmSource.Stop();
        audioSource.PlayOneShot(loseSFX);

        // 3. Di chuyển nhân vật (phải dùng unscaled time)
        yield return StartCoroutine(MoveSequence(target, loseSFX.length));

        // 5. Play animation + scream
                if (target.TryGetComponent<Human>(out Human human))
                    human.animator.updateMode = AnimatorUpdateMode.Normal;
        animator.Play("DeathTriggered");
        audioSource.PlayOneShot(screamSFX);

        yield return new WaitForSecondsRealtime(screamSFX.length);

        // 6. Delay 5s
        yield return _waitForSecondsRealtime;

        RestartScene();
    }

    IEnumerator MoveSequence(Transform target, float duration)
    {
        if (target.TryGetComponent<BoxCollider2D>(out var col))
            Destroy(col);
        if (target.TryGetComponent<Rigidbody2D>(out var rb)) rb.simulated = false;

        Vector3 doorPos = doorPosition;

        bool reachedDoor = false;

        float t = 0;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;

            if (!reachedDoor)
            {
                float dist = Vector3.Distance(target.position, doorPos);

                if (dist > 0.05f)
                {
                    target.position = Vector3.MoveTowards(
                        target.position,
                        doorPos,
                        0.3f * Time.unscaledDeltaTime
                    );
                }
                else
                {
                    reachedDoor = true;
                }
            }
            else
            {
                if (target.TryGetComponent<Human>(out Human human))
                    human.animator.updateMode = AnimatorUpdateMode.UnscaledTime;
                
                target.Translate(Vector3.left * 0.3f * Time.unscaledDeltaTime, Space.World);
            }

            yield return null;
        }
    }

    void RestartScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
