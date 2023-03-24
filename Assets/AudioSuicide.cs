using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSuicide : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("BeGone");
    }

    private IEnumerator BeGone()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
