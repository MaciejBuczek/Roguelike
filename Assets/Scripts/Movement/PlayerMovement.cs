using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Movement
{
    public CameraController cameraController;
    public GameObject focuesedEnemy;
    public Animator animatorHead, animatorBody;
    // Update is called once per frame
    public IEnumerator StartMovement()
    {
        yield return new WaitUntil(() => isMoving == false);
        if (focuesedEnemy != null)
        {
            path = PathFinder.Instance.GeneratePath(transform.position, focuesedEnemy.transform.position);
        }
        MoveCamera();
        if(path.Count > 0)
            StartCoroutine(MoveToPosition(path[0]));
        yield return new WaitUntil(() => TurnManager.Instance.enemiesTurn == false);
        TurnManager.Instance.playerTurn = false;
        if (focuesedEnemy != null && Vector2.Distance(transform.position, focuesedEnemy.transform.position) == 1)
            focuesedEnemy = null;
        OnTurnEnd();
    }
    protected override bool CheckForInterupt()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Input.ResetInputAxes();
            return true;
        }
        return false;
    }
    private void OnTurnEnd()
    {
        TurnManager.Instance.OnPlayerTurnEnd();
    }
    public void SetPath(List<Vector3> path)
    {
        this.path = path;
    }
    public void MoveCamera()
    {
        if((Vector2)cameraController.transform.position != (Vector2)transform.position)
            cameraController.LerpToPosition(transform.position, Time.time, 0.15f);
    }

    protected override void Flip(bool isRight)
    {
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        foreach(SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.flipX = isRight;
        }

    }
    protected override void OnMovementEnd()
    {
        if (path.Count == 0)
            SetMoveAnimation(false);
    }
    protected override void SetMoveAnimation(bool isMoving)
    {
        animatorBody.SetBool("isMoving", isMoving);
        animatorHead.SetBool("isMoving", isMoving);
    }
}
