using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShittyInputter : MonoBehaviour
{
    public bool input1;
    public bool input2;
    public bool input3;
    public bool input4;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            input1 = true;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            input2 = true;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            input3 = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            input4 = true;
            Debug.Log("rape");
        }

        if (input1 && input2 && input3 && input4 == true)
        {
            gameObject.SetActive(false);
        }
    }
}
