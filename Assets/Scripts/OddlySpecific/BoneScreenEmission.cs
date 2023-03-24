using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class BoneScreenEmission : MonoBehaviour
{
    /*public MeshRenderer[] boneScreenMats;*/
    public float emissionChange;
    public Material[] mattie;
    private float lastChange; 

    //private void Awake()
    //{
    //    FindBoneScreens();
    //}
    private void Update()
    {
        if(lastChange != emissionChange)
        {
            ChangeScreenEm(emissionChange);
        }
    }

    //public void FindBoneScreens()
    //{
    //    GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("BoneScreen");

    //    boneScreenMats = new MeshRenderer[gameObjects.Length];

    //    for (int i = 0; i < gameObjects.Length; i++)
    //    {
    //        boneScreenMats[i] = gameObjects[i].GetComponent<MeshRenderer>();
    //    }
    //}

    public void ChangeScreenEm(float screenEm)
    {
/*        for (int i = 0; i < boneScreenMats.Length; i++)
        {
            //boneScreenMats[i].material.SetFloat("_EmmissionStrength", screenEm);
            
        }*/
        foreach(Material matName in mattie)
        {
            matName.SetFloat("_EmmissionStrength", screenEm);
        }
        lastChange = screenEm;
    }
}
