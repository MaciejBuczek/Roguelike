using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Combat : MonoBehaviour
{
<<<<<<< Updated upstream
    // Start is called before the first frame update
    void Start()
=======
    public CharacterStats stats;

    public abstract void SetAnimationAttack();
    public abstract void SetAnimationHit();
    public abstract void SetAnimationDie();

    protected void AttackMeele(Combat enemy)
>>>>>>> Stashed changes
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Attack(Combat enemy)
    {

    }
    void ReceiveDamage(int Damage)
    {

    }
}
