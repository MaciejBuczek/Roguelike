using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBars : CharacterBars
{
    public Canvas healthBarCanvas;
    private bool isHealthBarEnabled = true;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("ToggleHealthBars"))
        {
            isHealthBarEnabled = !isHealthBarEnabled;
            if (isHealthBarEnabled)
                healthBarCanvas.enabled = true;
            else
                healthBarCanvas.enabled = false;
        }
    }
    private void OnMouseOver()
    {
        if (!isHealthBarEnabled)
            healthBarCanvas.enabled = true;
    }
    private void OnMouseExit()
    {
        if (!isHealthBarEnabled)
            healthBarCanvas.enabled = false;
    }
}
