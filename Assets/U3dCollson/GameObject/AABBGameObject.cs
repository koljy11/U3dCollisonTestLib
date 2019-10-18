using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AABBGameObject : MonoBehaviour
{

    [SerializeField]
    public AABBThreeD data;
   

    void Start()
    {
        data = new AABBThreeD();

    }

    // Update is called once per frame
    void Update()
    {
        data.Update(this.transform);

      

    }
}
