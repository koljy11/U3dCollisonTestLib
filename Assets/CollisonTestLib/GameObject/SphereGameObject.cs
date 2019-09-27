using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereGameObject : MonoBehaviour {

   public Sphere data=new Sphere();
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        data.Update(transform);

    }
}
