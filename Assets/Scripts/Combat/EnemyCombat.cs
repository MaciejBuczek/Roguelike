using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : Combat
{
    Animator anim;
    public override void SetAnimationAttack()
    {
        anim.SetTrigger("Attack");
    }

    public override void SetAnimationDie()
    {
        //throw new System.NotImplementedException();
    }

    public override void SetAnimationHit()
    {
        //anim.SetTrigger("Hit");
    }
}
