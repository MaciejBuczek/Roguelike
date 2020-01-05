using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 a = new Vector3();
        a.Set(0, 0, transform.rotation.z + 1);
        transform.position = new Vector3();
    }
}
