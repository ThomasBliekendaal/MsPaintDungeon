using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiddlKillsYou : MonoBehaviour
{
    public SceneLoader sceneLoadinator;

    private void OnCollisionEnter(Collision collision)
    {
        sceneLoadinator.LoadScene(sceneLoadinator.firstLevel);
    }
}
