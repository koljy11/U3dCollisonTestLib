using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBBGameObject : MonoBehaviour {


    public OBBThreeD data;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        data.Update(transform);

    }
}
