using System.Collections.Generic;
using UnityEngine;

public enum StatusEffect
{
    None,
    Petrify,
    Charm,
    Freeze,
    Burn
}

public class Human : MonoBehaviour
{
    public HumanType humanType;
    private int hp;
    public float spd;
    private float range;
    private float shortRange;
    private bool isPetrified = false;
    public bool isCharmed = false;
    public bool isArmored = false;
    private bool isLongRange = false;
    private GameObject shortTarget;

    public LayerMask defenderMask;
    public LayerMask attackerMask;
    private Defender defenderTarget;
    private Human attackerTarget;
    private Human betrayalTarget;
    public Animator animator;
    private AudioSource audioSource;
    [SerializeField] SpriteRenderer spriteRenderer;

    Color stoneColor;
    Color iceColor;
    Color charmColor;
    Color burnColor;

    public static List<Human> All = new();

    void OnEnable()
    {
        All.Add(this);
    }

    void OnDisable()
    {
        All.Remove(this);
    }

    private void Start()
    {
        gameObject.layer = 7;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();
        audioSource = gameObject.GetComponent<AudioSource>();
        hp = humanType.hp;
        spd = humanType.spd;
        range = humanType.range;
        shortRange = humanType.shortRange;
        isLongRange = humanType.isLongRange;
        InvokeRepeating(nameof(Talking),10, 15);
    }

    void Awake()
    {
        ColorUtility.TryParseHtmlString("#A0A4AA", out stoneColor);
        ColorUtility.TryParseHtmlString("#3FA7D6", out iceColor);
        ColorUtility.TryParseHtmlString("#FF24F3", out charmColor);
        ColorUtility.TryParseHtmlString("#333333", out burnColor);
    }

    private void Update()
    {
        if (isPetrified)
            return;
        if (isLongRange)
            CheckingShortTarget();
        CheckingTarget();
    }

    private void FixedUpdate()
    {
        if (isCharmed)
        {
            if (attackerTarget && !isLongRange)
                return;
            else if (isLongRange && shortTarget)
                return;
            transform.position += new Vector3(spd, 0, 0);
            return;
        }
        if ((defenderTarget || betrayalTarget) && !isLongRange)
            return;
        else if (isLongRange && shortTarget)
            return; 
        transform.position -= new Vector3(spd, 0, 0);
    }

    void Talking()
    {
        if (!audioSource.isPlaying && humanType.talkClips.Length > 0)
            audioSource.PlayOneShot(humanType.talkClips[Random.Range(0, humanType.talkClips.Length)]);
    }

    void CheckingShortTarget()
    {
        if (isCharmed)
        {
            RaycastHit2D reverseHit = Physics2D.Raycast(transform.position, Vector2.right, shortRange, attackerMask);

            if (reverseHit.collider)
            {
                shortTarget = reverseHit.collider.gameObject;
            }
            return;
        }
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, shortRange, defenderMask);

        if (hit.collider)
        {
            shortTarget = hit.collider.gameObject;
        }
    }

    void CheckingTarget()
    {
        if (isCharmed)
        {
            RaycastHit2D reverseHit = Physics2D.Raycast(transform.position, Vector2.right, range, attackerMask);

            if (reverseHit.collider)
            {
                attackerTarget = reverseHit.collider.GetComponent<Human>();
            }
            return;
        }
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, range, defenderMask);

        if (hit.collider)
        {
            Defender defender = hit.collider.GetComponent<Defender>();
            Human human = hit.collider.GetComponent<Human>();
            if (defender)
            {
                defenderTarget = defender;
                betrayalTarget = null;
            }
            if (human)
            {
                if (!human.isCharmed)
                {
                    betrayalTarget = null;
                    return;
                }
                defenderTarget = null;
                betrayalTarget = human;
            }
        }
    }

    public Component GetCurTarget()
    {
        if (betrayalTarget)
            return betrayalTarget;
        if (defenderTarget)
            return defenderTarget;
        if (attackerTarget)
            return attackerTarget;
        return null;
    }

    public void ApplyStatus(StatusEffect status)
    {
        spriteRenderer.color = status switch
        {
            StatusEffect.Petrify => stoneColor,
            StatusEffect.Freeze => iceColor,
            StatusEffect.Charm => charmColor,
            StatusEffect.Burn => burnColor,
            _ => Color.white,
        };
    }

    public void ResetEffect()
    {
        ApplyStatus(StatusEffect.None);
        spd = humanType.spd;
        animator.speed = 1f;
        if (isCharmed)
        {
            gameObject.layer = 7;
            isCharmed = false;
            FlipObject();
            attackerTarget = null;
            if (shortTarget)
                shortTarget = null;
        }
    }

    public void Slowed(int durantion = 3)
    {
        ApplyStatus(StatusEffect.Freeze);
        spd = humanType.spd / 2;
        animator.speed = 0.25f;
        CancelInvoke(nameof(ResetEffect));
        Invoke(nameof(ResetEffect), durantion);
    }

    public void Petrified()
    {
        if (gameObject.TryGetComponent<BoxCollider2D>(out var col))
            Destroy(col);
        isPetrified = true;
        ApplyStatus(StatusEffect.Petrify);
        spd = 0f;
        animator.speed = 0f;
        Invoke(nameof(Dying), 2);
    }

    public void Charmed()
    {
        gameObject.layer = 9;
        isCharmed = true;
        defenderTarget = null;
        if (shortTarget)
            shortTarget = null;
        ApplyStatus(StatusEffect.Charm);
        Invoke(nameof(ResetEffect), 15);
        FlipObject();
    }

    public void Burned()
    {
        if (gameObject.TryGetComponent<BoxCollider2D>(out var col))
            Destroy(col);
        ApplyStatus(StatusEffect.Burn);
        spd = 0f;
        animator.speed = 0f;
        Invoke(nameof(Dying), 2);
    }

    void FlipObject()
    {
        gameObject.transform.localScale = new Vector2(
            gameObject.transform.localScale.x * -1,
            gameObject.transform.localScale.y
        );
    }

    void Dying()
    {
        Destroy(gameObject);
    }

    public void Hitted(int damage, bool? isKnockback = false)
    {
        audioSource.PlayOneShot(humanType.hitClips[Random.Range(0, humanType.hitClips.Length)]);
        if (isArmored)
            hp -= damage & 3;
        else
            hp -= damage;
        if (hp <= 0)
        {
            if (gameObject.TryGetComponent<BoxCollider2D>(out var col))
            {
                Destroy(col);
            }
            animator.SetTrigger("tgDying");
            animator.speed = 1f;
            spd = 0f;
        }
        else if (isKnockback.HasValue && isKnockback.Value)
        {
            transform.position += new Vector3(1.5f, 0, 0);
            defenderTarget = null;
        }
    }

}
