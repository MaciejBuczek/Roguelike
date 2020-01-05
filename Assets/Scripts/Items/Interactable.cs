using UnityEngine;
using UnityEngine.EventSystems;

public class Interactable : MonoBehaviour
{
    public float radius = 1.5f;

    private bool isFocused = false;
    private Vector3 playerPosition;

    public virtual void Interact()
    {
        Debug.Log("Interactig with " + transform.name);
    }
    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        Debug.Log("clicked on " + transform.name);
        isFocused = true;
    }
    private void Start()
    {
        //playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
    }
    private void Update()
    {
        if (isFocused)
        {
            playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
            if (Vector3.Distance(transform.position, playerPosition) <= radius)
            {
                Interact();
                isFocused = false;
            }
        }
    }
}
