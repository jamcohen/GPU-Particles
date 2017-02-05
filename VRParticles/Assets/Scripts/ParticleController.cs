using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour {
    public Material SimulateMat;
	   
	// Update is called once per frame
	void Update () {
        SimulateMat.SetVector("_AttractPos", transform.position);

    }
}
