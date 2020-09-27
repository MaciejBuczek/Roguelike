using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Combat : MonoBehaviour, IComparable<Combat>
{
    public bool isInCombat = false;
    protected AggressiveCreature creature;

    public int CompareTo(Combat other)
    {
        return other.creature.attackSpeedMelee.CompareTo(creature.attackSpeedMelee);
    }

    public AggressiveCreature getCreature()
    {
        return creature;
    }

}
