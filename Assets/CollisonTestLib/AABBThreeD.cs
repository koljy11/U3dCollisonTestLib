using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AABBThreeD
{
    [SerializeField]
    public Vector3 minPoint;
    [SerializeField]
    public Vector3 maxPoint;
    [SerializeField]
    public Vector3 halfSize;
    [SerializeField]
    public Vector3 center;
    [SerializeField]
    public Vector3 size;



    // Update is called once per frame
    public void Update(Transform transform)
    {
        center = transform.position;
        size = transform.localScale;

        halfSize = 0.5f * size;
        minPoint = center - halfSize;
        maxPoint = center + halfSize;

        // AABBTransformAsAABB(this, transform.localToWorldMatrix);


        //if (target != null)
        //{
        //    var col = Intersects(target);
        //    if (col)
        //    {

        //        this.GetComponent<MeshRenderer>().materials[0].SetColor("_Color", new Color(1, 0, 0));
        //    }
        //    else
        //    {
        //        this.GetComponent<MeshRenderer>().materials[0].SetColor("_Color", new Color(1, 1, 1));
        //    }
        //}
    }

    public bool Intersects(AABBThreeD aabb)
    {
        // If any of the cardinal X,Y,Z axes is a separating axis, then
        // there is no intersection.
        return minPoint.x < aabb.maxPoint.x &&

               minPoint.y < aabb.maxPoint.y &&

               minPoint.z < aabb.maxPoint.z &&

               aabb.minPoint.x < maxPoint.x &&

               aabb.minPoint.y < maxPoint.y &&

               aabb.minPoint.z < maxPoint.z;
    }

    //错误
    void AABBTransformAsAABB(AABBThreeD aabb, Matrix4x4 m)
    {
        //Vector3 newCenter = m.MulPos(aabb.CenterPoint());
        // center = m.MultiplyPoint(aabb.CenterPoint());
        Vector3 newDir;
        //Vector3 h = aabb.HalfSize();
        //halfSize
        Vector3 h = halfSize;
        //The following is equal to taking the absolute value of the whole matrix m.
        newDir.x = MathFunc.ABSDOT3(m.GetRow(0), h);
        newDir.y = MathFunc.ABSDOT3(m.GetRow(1), h);
        newDir.z = MathFunc.ABSDOT3(m.GetRow(2), h);
        aabb.minPoint = center - newDir;
        aabb.maxPoint = center + newDir;
    }

    Vector3 CenterPoint()
    {
        return (minPoint + maxPoint) / 2.0f;
    }

    Vector3 Size()
    {
        return maxPoint - minPoint;
    }

    public Vector3 HalfSize()
    {
        return Size() / 2.0f;
    }

    private void OnDrawGizmos()
    {
        // Gizmos.DrawCube(center, halfSize*2);
    }



    public void SetFrom(OBBThreeD obb)
    {
        Vector3 halfSize = MathFunc.Abs(obb.axis[0] * obb.r[0]) + MathFunc.Abs(obb.axis[1] * obb.r[1]) + MathFunc.Abs(obb.axis[2] * obb.r[2]);
        SetFromCenterAndSize(obb.pos, 2.0f * halfSize);
    }

    public void SetFromCenterAndSize(Vector3 p_center, Vector3 p_size)
    {
        center = p_center;

        size = p_size;
        halfSize = p_size * 0.5f;
        minPoint = center - halfSize;
        maxPoint = center + halfSize;



    }

    public bool Contains(OBBThreeD obb)
    {

        return Contains(obb.MinimalEnclosingAABB());
    }

    public bool Contains(AABBThreeD aabb)
    {
        return this.Contains(aabb.minPoint) && this.Contains(aabb.maxPoint);
    }

    public bool Contains(Vector3 point)
    {
        return minPoint.x <= point.x && point.x <= maxPoint.x &&
               minPoint.y <= point.y && point.y <= maxPoint.y &&
               minPoint.z <= point.z && point.z <= maxPoint.z;
    }

    public bool Intersects(OBBThreeD obb)
    {
        return obb.Intersects(this);
    }


    public bool Intersects(Sphere sphere)
    {
        // Find the point on this AABB closest to the sphere center.
        //  Vector3 pt = clostPtToAABB(this, sphere.pos);
        Vector3 pt = ClosestPoint(sphere.pos);
        
        // If that point is inside sphere, the AABB and sphere intersect.
        //if (closestPointOnAABB)
        //    *closestPointOnAABB = pt;
        var s = MathFunc.DistanceSq(pt, sphere.pos);
        return s <= sphere.r * sphere.r;
    }

    //Vector3 clostPtToAABB(AABBThreeD a, Vector3 p)
    //{
    //    Vector3 q = new Vector3();
    //    if (p.x < a.minPoint.x) q.x = a.minPoint.x;
    //    else if (p.x > a.maxPoint.x) q.x = a.maxPoint.x;

    //    if (p.y < a.minPoint.y) q.y = a.minPoint.y;
    //    else if (p.y > a.maxPoint.y) q.y = a.maxPoint.y;

    //    if (p.z < a.minPoint.z) q.z = a.minPoint.z;
    //    else if (p.z > a.maxPoint.z) q.z = a.maxPoint.z;
    //    return q;
    //}


    Vector3 Min(Vector3 a, Vector3 ceil)
    {
        return new Vector3(Min(a.x, ceil.x), Min(a.y, ceil.y), Min(a.z, ceil.z));
    }

    float Min(float a, float b)
    {

        return a <= b ? a : b;

    }

    Vector3 Max(Vector3 a, Vector3 ceil)
    {
        return new Vector3(Max(a.x, ceil.x), Max(a.y, ceil.y), Max(a.z, ceil.z));
    }

    float Max(float a, float b)
    {

        return a >= b ? a : b;

    }

    Vector3 ClosestPoint(Vector3 targetPoint)
    {
        return Clamp(targetPoint, minPoint, maxPoint);
    }

    Vector3 Clamp(Vector3 targetPoint, Vector3 floor, Vector3 ceil)
    {
        var s = Min(targetPoint, ceil);

        s = Max(s, floor);
        return s;
    }


}
