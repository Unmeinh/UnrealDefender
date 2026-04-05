using UnityEngine;


[CreateAssetMenu(fileName = "HumanType", menuName = "Game/Human Type")]
public class HumanType : ScriptableObject
{
    public int hp;
    public int damage;
    public float spd;
    public float range;
    public float hitCooldown;
    public float shortRange;
    public bool isLongRange;
    public AudioClip[] hitClips;
    public AudioClip[] talkClips;
}
