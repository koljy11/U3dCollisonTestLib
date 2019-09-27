﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Sphere : ShapeThreeD
{

   

    /// The center point of this sphere.
    public Vector3 pos;
    /// The radius of this sphere.
    /** [similarOverload: pos] */
    public float r;
  

    public void Update(Transform transform)
    {
       pos = transform.position;
        r = transform.localScale.x*0.5f;
    }


    public  bool SphereVsObb(OBBThreeD obb)
    {
        return obb.Intersects(this);

    }

    internal void OnObbEnter(OBBThreeD obb)
    {
        Debug.Log("OnObbEnter");
    }
}











