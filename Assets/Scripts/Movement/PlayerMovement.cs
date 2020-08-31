using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Movement
{
    // Update is called once per frame
    public IEnumerator StartMovement()
    {
        yield return new WaitUntil(() => isMoving == false);
        StartCoroutine(MoveToPosition(path[0]));
        yield return new WaitUntil(() => TurnManager.Instance.enemiesTurn == false);
        TurnManager.Instance.playerTurn = false;
        OnMovementEnd();
        Debug.Log("player turn end");
    }
    protected override bool CheckForInterupt()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            return true;
        }
        return false;
    }
    private void OnMovementEnd()
    {
        TurnManager.Instance.OnPlayerTurnEnd();
    }
    public void SetPath(List<Vector3> path)
    {
        this.path = path;
    }
}
