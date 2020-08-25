using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    public Sprite[] cursors;
    public GameObject PointingAt;
    private SpriteRenderer spriteRenderer;

    enum Cursors { defaultCursor, enemy, targetRanged, targetMagic};

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3((int)Mathf.Round(mousePosition.x), (int)Mathf.Round(mousePosition.y), 0);

        RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hitInfo.collider != null)
            PointingAt = hitInfo.collider.gameObject;
        else
            PointingAt = null;
        if (hitInfo.collider != null && hitInfo.collider.CompareTag("Enemy"))
            spriteRenderer.sprite = cursors[(int)Cursors.enemy];
        else
            spriteRenderer.sprite = cursors[(int)Cursors.defaultCursor];
    }
}
