using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        gameObject.layer = LayerMask.NameToLayer("Background");
    }
}
