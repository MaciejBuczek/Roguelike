using System.Collections;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Animator animator;
    public Movement movement;
    private bool playIdle = false;
    private void Start()
    {
        movement.onMovement += PlayMove;
    }
    private void Update()
    {
        if (playIdle)
        {
            StartCoroutine(PlayIdle());
        }
    }
    private void PlayMove(bool state)
    {
        animator.SetBool("Move", state);
    }
    private void PlayAttack()
    {
        animator.SetTrigger("Attack");
    }
    private IEnumerator PlayIdle()
    {
        playIdle = false;
        yield return new WaitForSeconds(Random.Range(0, 4));
        animator.SetTrigger("IdleAnimation");
        playIdle = true;
    }
}
