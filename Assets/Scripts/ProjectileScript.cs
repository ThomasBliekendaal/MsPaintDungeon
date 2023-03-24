using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public GameObject projectileExplosion;
    public GameObject audioToSpawn;

    private void OnCollisionEnter(Collision collision)
    {
        GameObject mySplosion = Instantiate(projectileExplosion, gameObject.transform.position, Quaternion.identity);
        mySplosion.transform.parent = null;

        Instantiate(audioToSpawn, gameObject.transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
