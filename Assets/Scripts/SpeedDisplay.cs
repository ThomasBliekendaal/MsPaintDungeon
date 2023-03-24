using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;    

public class SpeedDisplay : MonoBehaviour
{
    public Rigidbody playerBody;
    public TextMeshProUGUI displayText;
    // Update is called once per frame
    void Update()
    {
        displayText.text = playerBody.velocity.magnitude.ToString();
    }
}
