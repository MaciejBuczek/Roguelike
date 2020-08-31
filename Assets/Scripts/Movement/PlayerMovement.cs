using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Movement
{

    // Update is called once per frame
    public IEnumerator StartMovement()
    {
        yield return new WaitUntil(() => isMoving == false);
        if (path.Count == 0)
        {
            while(!Input.GetKey(KeyCode.Mouse0))
            {
                yield return null;
            }
            Input.ResetInputAxes();
            if (Cursor.Instance.position == (Vector2)transform.position)
            {
                TurnManager.Instance.playerTurn = false;
                yield break;
            }
            path = PathFinder.Instance.GeneratePath(transform.position, Cursor.Instance.position);
            StartCoroutine(MoveToPosition(path[0]));
            //path.Remove(path[0]);
            yield return new WaitUntil(() => TurnManager.Instance.enemiesTurn == false);
            TurnManager.Instance.playerTurn = false;
            OnMovementEnd();
            Debug.Log("player turn end");
        }
        else
        {
            StartCoroutine(MoveToPosition(path[0]));
            //path.Remove(path[0]);
            yield return new WaitUntil(() => TurnManager.Instance.enemiesTurn == false);
            TurnManager.Instance.playerTurn = false;
            OnMovementEnd();
            Debug.Log("player turn end");
        }
    }
    private void OnMovementEnd()
    {
        TurnManager.Instance.OnPlayerTurnEnd();
    }
}
