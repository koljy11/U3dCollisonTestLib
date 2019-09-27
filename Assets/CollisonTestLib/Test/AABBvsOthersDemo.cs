using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AABBvsOthersDemo : MonoBehaviour {

    [SerializeField]
    AABBGameObject sourcesTarget;

    [SerializeField]
    AABBGameObject target;

    [SerializeField]
    OBBThreeD targetOBB;
    // Use this for initialization

    [SerializeField]
    Sphere sphere;


    [SerializeField]
    Transform point;
    [SerializeField]
    private bool aabbClosion;
    [SerializeField]
    private bool obbClosion;

    [SerializeField]
    private bool sphereCollsion;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
       
        {
           // data.Update(this.transform);

            aabbClosion = false;
            if (target != null)
            {
                aabbClosion = sourcesTarget.data.Intersects(target.data);
                // col1 = data.Contains(target.data); 
            }

            obbClosion = false;
            if (targetOBB != null)
            {
                obbClosion = sourcesTarget.data.Intersects(targetOBB);

            }

            sphereCollsion = false;
            if (sphere != null)
            {
                sphereCollsion = sourcesTarget.data.Intersects(sphere);

            }

            //bool col2 = false;
            //if (point != null)
            //{
            //    col2 = data.Contains(point.position);

            //}

            if (obbClosion || aabbClosion || sphereCollsion)
            {

                sourcesTarget.GetComponent<MeshRenderer>().materials[0].SetColor("_Color", new Color(1, 0, 0));
            }
            else
            {
                sourcesTarget.GetComponent<MeshRenderer>().materials[0].SetColor("_Color", new Color(1, 1, 1));
            }

        }
    }
}
