using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketObject : MonoBehaviour {
    public GameObject ParticleSystemPrefab;
    public MeshRenderer MeshObject;


    public void Crash() {
        //Debug.Log("Crash");
        Instantiate(ParticleSystemPrefab, transform, false);
        MeshObject.enabled = false;
    }

    public void Restore() {
        //Debug.Log("Restore");
        MeshObject.enabled = true;
    }
}
