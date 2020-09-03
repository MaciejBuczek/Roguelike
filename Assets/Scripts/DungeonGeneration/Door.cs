using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Collider2D lastCollision;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        lastCollision = collision;
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        Debug.Log("a");
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == lastCollision)
        {
            gameObject.layer = LayerMask.NameToLayer("Background");
            Debug.Log("b");
        }
    }
}
