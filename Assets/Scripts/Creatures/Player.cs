using UnityEngine;

[CreateAssetMenu(fileName = "New Creature", menuName = "Creatures/Player")]

public class Player : AggressiveCreature
{
    public Stat strength, dexterity, intelligence;
    public AnimatorOverrideController headAnimatorOverride;
}
