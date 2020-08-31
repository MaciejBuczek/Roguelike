using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    public Sprite[] cursors;
    public GameObject PointingAt;
    private SpriteRenderer spriteRenderer;

    public static Cursor Instance;

    enum Cursors { defaultCursor, enemy, targetRanged, targetMagic};
    public Vector2 position;

    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        int x, y;
        x = Mathf.RoundToInt(mousePosition.x);
        y = Mathf.RoundToInt(mousePosition.y);
        position = new Vector2(x, y);
        transform.position = position;

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
