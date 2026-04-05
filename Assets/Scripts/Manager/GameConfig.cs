using UnityEngine;

[CreateAssetMenu(fileName = "CombatConfig", menuName = "Game/Combat Config")]
public class CombatConfig : ScriptableObject
{
    public float heavyAttackChance = 0.35f;
    public float tripleAttackChance = 0.25f;
    public float heavyAttackMultiplier = 2f;
}