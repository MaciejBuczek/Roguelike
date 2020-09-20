using UnityEngine;

[CreateAssetMenu(fileName = "New Creature", menuName = "Creatures/Passive")]
public class PassiveCreature : ScriptableObject
{
    new public string name;
    public Sprite icon;
    public int expGain;
    public int sightDistance;
    public AnimatorOverrideController mainAnimatorOverride;
    public Stat armor, health;  
}
