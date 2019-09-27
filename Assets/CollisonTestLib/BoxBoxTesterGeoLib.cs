using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxBoxTesterGeoLib : MonoBehaviour {

    public GameObject box;

    public GameObject box1;

    [SerializeField]
    OBBThreeD obb;
    [SerializeField]
    OBBThreeD _box1;

    public GameObject u3dsphere;
    Sphere libSphere;

    [SerializeField]
    Transform view;
    void Start()

    {

        obb = new OBBThreeD();

        _box1 = new OBBThreeD();
        libSphere = new Sphere();

    }

    void Update()

    {
        var c = box.GetComponent<BoxCollider>();
        // _box.SetFrom(box.transform.position, c.bounds.min,c.bounds.max, box.transform.localToWorldMatrix, box.transform.rotation);
        //_box.SetFrom(box.transform.position, c.bounds.min, c.bounds.max, box.transform.rotation);
        obb.SetFrom(box.transform.position, box.transform.rotation, 0.5f* box.transform.localScale);

        c = box1.GetComponent<BoxCollider>();
        //    _box1.SetFrom(box1.transform.position, c.bounds.min, c.bounds.max, box1.transform.localToWorldMatrix, box.transform.rotation);
        // _box1.SetFrom(box1.transform.position, c.bounds.min, c.bounds.max, box1.transform.rotation);
        _box1.SetFrom(box1.transform.position, box1.transform.rotation, 0.5f * box1.transform.localScale);

        var u3dSphere = u3dsphere.GetComponent<UnityEngine.SphereCollider>();
        libSphere.pos = u3dSphere.transform.position;
        libSphere.r = u3dSphere.radius;
        //_box.rotation = box.transform.rotation;

        // _box.extents = 0.5f * box.transform.localScale;



        //_box1.center = box1.transform.position;

        // _box1.rotation = box1.transform.rotation;

        // _box1.extents = 0.5f * box1.transform.localScale;

        var s = obb.ClosestPointTo(libSphere.pos);
        view.position = s;

        bool col = false;

        //if (_box.Intersects(_box1,0))
        if(OBBThreeD.IntersectBoxBox(obb, _box1))
        {
            col = true;
            // box.GetComponent<MeshRenderer>().materials[0].SetColor("_Color", new Color(1, 0, 0));
        }
        else
        {
          //  box.GetComponent<MeshRenderer>().materials[0].SetColor("_Color", new Color(1, 1, 1));
        }

        //var s=_box.ClosestPoint(libSphere.pos,box.transform.localToWorldMatrix, box.transform.position, c.bounds.min,c.bounds.max);
        // view.position = s;

      
        if (obb.Intersects(libSphere))
        {
            col = true;
            //  box.GetComponent<MeshRenderer>().materials[0].SetColor("_Color", new Color(1, 0, 0));
        }
        else
        {

            //  box.GetComponent<MeshRenderer>().materials[0].SetColor("_Color", new Color(1, 1, 1));

        }

        if (col)
        {
           
             box.GetComponent<MeshRenderer>().materials[0].SetColor("_Color", new Color(1, 0, 0));
        }
        else
        {
              box.GetComponent<MeshRenderer>().materials[0].SetColor("_Color", new Color(1, 1, 1));
        }

    }
    void Update_()

    {
        // var c = box.GetComponent<BoxCollider>();
        // _box.SetFrom(box.transform.position, c.bounds.min,c.bounds.max, box.transform.localToWorldMatrix, box.transform.rotation);
        //_box.SetFrom(box.transform.position, c.bounds.min, c.bounds.max, box.transform.rotation);
        obb.SetFrom(box.transform.position, box.transform.rotation, 0.5f * box.transform.localScale);



        //var u3dSphere = u3dsphere.GetComponent<UnityEngine.SphereCollider>();
        libSphere.pos = u3dsphere.transform.position;
        libSphere.r = u3dsphere.transform.localScale.x * 0.5f;
        //_box.rotation = box.transform.rotation;

        // _box.extents = 0.5f * box.transform.localScale;





        var s = obb.ClosestPointTo(libSphere.pos);
        view.position = s;

        bool col = false;



        //var s=_box.ClosestPoint(libSphere.pos,box.transform.localToWorldMatrix, box.transform.position, c.bounds.min,c.bounds.max);
        // view.position = s;


        if (obb.Intersects(libSphere))
        {
            col = true;
            //  box.GetComponent<MeshRenderer>().materials[0].SetColor("_Color", new Color(1, 0, 0));
        }
        else
        {

            //  box.GetComponent<MeshRenderer>().materials[0].SetColor("_Color", new Color(1, 1, 1));

        }

        if (col)
        {

            box.GetComponent<MeshRenderer>().materials[0].SetColor("_Color", new Color(1, 0, 0));
        }
        else
        {
            box.GetComponent<MeshRenderer>().materials[0].SetColor("_Color", new Color(1, 1, 1));
        }

    }

    [ContextMenu("Test")]
    void Test()
    {
        obb.SetFrom(box.transform.position, box.transform.rotation, 0.5f * box.transform.localScale);



        //var u3dSphere = u3dsphere.GetComponent<UnityEngine.SphereCollider>();
        libSphere.pos = u3dsphere.transform.position;
        libSphere.r = u3dsphere.transform.localScale.x * 0.5f;

        Debug.Log(obb.Intersects(libSphere));
    }
  
}
