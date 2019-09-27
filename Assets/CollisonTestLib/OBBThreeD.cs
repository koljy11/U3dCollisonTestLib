

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class OBBThreeD : ShapeThreeD
{
    [SerializeField]
    Vector3 maxPoint;
    [SerializeField]
    Vector3 minPoint;
    public Vector3 pos;
    [SerializeField]
    public Vector3[] axis = new Vector3[3];
    //HalfSize
    public Vector3 r;

    Quaternion rotation;

    Vector3 halfExtents = new Vector3();
    public void SetFrom(Vector3 p_pos, Vector3 p_minPoint, Vector3 p_maxPoint, Quaternion quaternion)
    {

        pos = p_pos;
        maxPoint = p_maxPoint;
        minPoint = p_minPoint;
        r = HalfSize();

        axis[0] = quaternion * new Vector3(1, 0, 0);
        axis[1] = quaternion * new Vector3(0, 1, 0);
        axis[2] = quaternion * new Vector3(0, 0, 1);





    }

    float LengthSq(Vector3 p)
    {
        return p.x * p.x + p.y * p.y + p.z * p.z;
    }


    //float Dot(Vector3 a, Vector3 b)
    //{
    //    return Vector3.Dot(a, b);
    //}
    public void SetFromWithAix(Vector3 p_pos, Vector3 p_minPoint, Vector3 p_maxPoint, Quaternion quaternion)
    {
        // pos =m.MultiplyPoint(p_pos);
        pos = (p_pos);
        minPoint = (p_minPoint);
        maxPoint = (p_maxPoint);

        Vector3 size = HalfSize();
        r.x = size.x;
        r.y = size.y;
        r.z = size.z;
        //// axis[0] = quaternion*m.GetColumn(0);
        // axis[1] = quaternion * m.GetColumn(1);
        // axis[2] = quaternion * m.GetColumn(2);
        axis[0] = quaternion * new Vector3(1, 0, 0);
        axis[1] = quaternion * new Vector3(0, 1, 0);
        axis[2] = quaternion * new Vector3(0, 0, 1);


        // If the matrix m contains scaling, propagate the scaling from the axis vectors to the half-length vectors,
        // since we want to keep the axis vectors always normalized in our representation.
        //float matrixScale = axis[0].LengthSq();
        float matrixScale = LengthSq(axis[0]);
        matrixScale = Mathf.Sqrt(matrixScale);
        r *= matrixScale;
        matrixScale = 1.0f / matrixScale;
        axis[0] *= matrixScale;
        axis[1] *= matrixScale;
        axis[2] *= matrixScale;

        Orthonormalize(ref axis[0], ref axis[1], ref axis[2]);
    }

    public void SetFrom(Vector3 p_pos, Quaternion quaternion, Vector3 p_extents)
    {
        // pos =m.MultiplyPoint(p_pos);
        pos = (p_pos);
        // minPoint = (p_minPoint);
        // maxPoint = (p_maxPoint);

        // Vector3 size = HalfSize();
        rotation = quaternion;
        halfExtents = p_extents;


    }

    void Orthonormalize(ref Vector3 a, ref Vector3 b, ref Vector3 c)
    {
        // assume(!a.IsZero());
        a.Normalize();
        // b -= b.ProjectToNorm(a);
        b -= ProjectToNorm(b, a);
        // assume(!b.IsZero());
        b.Normalize();
        c -= ProjectToNorm(c, a);
        c -= ProjectToNorm(c, b);
        //  assume(!c.IsZero());
        c.Normalize();
    }

    Vector3 ProjectToNorm(Vector3 pthis, Vector3 direction)
    {

        // assume(direction.IsNormalized());
        return direction * Vector3.Dot(direction, pthis);
    }


    Vector3 Size()
    {
        return maxPoint - minPoint;
    }

    Vector3 HalfSize()
    {
        return Size() * 0.5f;
    }

    float Dot(Vector3 a, Vector3 b)
    {
        return Vector3.Dot(a, b);
    }

    float DOT3(Vector3 a, float[] b)
    {
        return Vector3.Dot(a, new Vector3(b[0], b[1], b[2]));
    }
    public bool Intersects(OBBThreeD b, float epsilon = 0)
    {

        //assume(pos.IsFinite());
        // assume(b.pos.IsFinite());
        // assume(float3::AreOrthonormal(axis[0], axis[1], axis[2]));
        // assume(float3::AreOrthonormal(b.axis[0], b.axis[1], b.axis[2]));
        float ra;
        float rb;
        // Generate a rotation matrix that transforms from world space to this OBB's coordinate space.
        var R = Float3X3.Create();
        for (int i = 0; i < 3; ++i)
            for (int j = 0; j < 3; ++j)
                R[i][j] = Dot(axis[i], b.axis[j]);

        Vector3 t = b.pos - pos;
        // Express the translation vector in a's coordinate frame.
        t = new Vector3(Dot(t, axis[0]), Dot(t, axis[1]), Dot(t, axis[2]));

        var AbsR = Float3X3.Create();
        for (int i = 0; i < 3; ++i)
            for (int j = 0; j < 3; ++j)
                AbsR[i][j] = Mathf.Abs(R[i][j]) + epsilon;

        // Test the three major axes of this OBB.
        for (int i = 0; i < 3; ++i)
        {
            ra = r[i];
            rb = DOT3(b.r, AbsR[i]);
            if (Mathf.Abs(t[i]) > ra + rb)
                return false;
        }

        // Test the three major axes of the OBB b.
        for (int i = 0; i < 3; ++i)
        {
            ra = r[0] * AbsR[0][i] + r[1] * AbsR[1][i] + r[2] * AbsR[2][i];
            rb = b.r[i];
            if (Mathf.Abs(t.x * R[0][i] + t.y * R[1][i] + t.z * R[2][i]) > ra + rb)
                return false;
        }

        // Test the 9 different cross-axes.

        // A.x <cross> B.x
        ra = r.y * AbsR[2][0] + r.z * AbsR[1][0];
        rb = b.r.y * AbsR[0][2] + b.r.z * AbsR[0][1];
        if (Mathf.Abs(t.z * R[1][0] - t.y * R[2][0]) > ra + rb)
            return false;

        // A.x < cross> B.y
        ra = r.y * AbsR[2][1] + r.z * AbsR[1][1];
        rb = b.r.x * AbsR[0][2] + b.r.z * AbsR[0][0];
        if (Mathf.Abs(t.z * R[1][1] - t.y * R[2][1]) > ra + rb)
            return false;

        // A.x <cross> B.z
        ra = r.y * AbsR[2][2] + r.z * AbsR[1][2];
        rb = b.r.x * AbsR[0][1] + b.r.y * AbsR[0][0];
        if (Mathf.Abs(t.z * R[1][2] - t.y * R[2][2]) > ra + rb)
            return false;

        // A.y <cross> B.x
        ra = r.x * AbsR[2][0] + r.z * AbsR[0][0];
        rb = b.r.y * AbsR[1][2] + b.r.z * AbsR[1][1];
        if (Mathf.Abs(t.x * R[2][0] - t.z * R[0][0]) > ra + rb)
            return false;

        // A.y <cross> B.y
        ra = r.x * AbsR[2][1] + r.z * AbsR[0][1];
        rb = b.r.x * AbsR[1][2] + b.r.z * AbsR[1][0];
        if (Mathf.Abs(t.x * R[2][1] - t.z * R[0][1]) > ra + rb)
            return false;

        // A.y <cross> B.z
        ra = r.x * AbsR[2][2] + r.z * AbsR[0][2];
        rb = b.r.x * AbsR[1][1] + b.r.y * AbsR[1][0];
        if (Mathf.Abs(t.x * R[2][2] - t.z * R[0][2]) > ra + rb)
            return false;

        // A.z <cross> B.x
        ra = r.x * AbsR[1][0] + r.y * AbsR[0][0];
        rb = b.r.y * AbsR[2][2] + b.r.z * AbsR[2][1];
        if (Mathf.Abs(t.y * R[0][0] - t.x * R[1][0]) > ra + rb)
            return false;

        // A.z <cross> B.y
        ra = r.x * AbsR[1][1] + r.y * AbsR[0][1];
        rb = b.r.x * AbsR[2][2] + b.r.z * AbsR[2][0];
        if (Mathf.Abs(t.y * R[0][1] - t.x * R[1][1]) > ra + rb)
            return false;

        // A.z <cross> B.z
        ra = r.x * AbsR[1][2] + r.y * AbsR[0][2];
        rb = b.r.x * AbsR[2][1] + b.r.y * AbsR[2][0];
        if (Mathf.Abs(t.y * R[0][2] - t.x * R[1][2]) > ra + rb)
            return false;

        // No separating axis exists, so the two OBB don't intersect.
        return true;
    }
    //负数的缩放会存在问题
    public bool Intersects(Sphere sphere)
    {
        // Find the point on this AABB closest to the sphere center.
        Vector3 closepoint = ClosestPointTo(sphere.pos);

        // If that point is inside sphere, the AABB and sphere intersect.
        //  if (closestPointOnOBB)
        //     * closestPointOnOBB = pt;
        // var d=Vector3.Distance(pt,sphere.pos);
        // var myR=
        var s1 = DistanceSq(closepoint, sphere.pos) <= sphere.r * sphere.r;
        //if (s1) return true;
        return s1;
        var my_r = Vector3.Distance(closepoint, pos);
        s1 = my_r * my_r <= sphere.r * sphere.r;
        // var myR=
        return s1;
    }

    /// The implementation of this function is from Christer Ericson's Real-Time Collision Detection, p.133.
    public Vector3 ClosestPoint(Vector3 targetPoint)
    {
        axis[0] = rotation * new Vector3(1, 0, 0);

        axis[1] = rotation * new Vector3(0, 1, 0);

        axis[2] = rotation * new Vector3(0, 0, 1);

        Vector3 d = targetPoint - pos;
        Vector3 closestPoint = pos; // Start at the center point of the OBB.
        for (int i = 0; i < 3; ++i)
        {// Project the target onto the OBB axes and walk towards that point.
         // closestPoint += Mathf.Clamp(Dot(d, axis[i]), -r[i], r[i]) * axis[i];
            closestPoint += Mathf.Clamp(Dot(d, axis[i]), -r[i], r[i]) * axis[i];
        }

        return closestPoint;
    }

    // //// axis[0] = quaternion*m.GetColumn(0);
    // axis[1] = quaternion * m.GetColumn(1);
    // axis[2] = quaternion * m.GetColumn(2);

    Vector3 CenterPoint()
    {
        return (minPoint + maxPoint) / 2.0f;
    }

    float Length(Vector3 v3)
    {
        return Mathf.Sqrt(LengthSq(v3));
    }
    float Normalize(ref Vector3 v3)
    {
        // assume(IsFinite());
        float length = Length(v3);
        if (length > 1e-6f)
        {
            v3 *= 1.0f / length;
            return length;
        }
        else
        {
            v3 = new Vector3(1.0f, 0, 0); // We will always produce a normalized vector.
            return 0; // But signal failure, so user knows we have generated an arbitrary normalization.
        }
    }
    public Vector3 ClosestPoint(Vector3 targetPoint, Matrix4x4 m, Vector3 p_pos, Vector3 p_minPoint, Vector3 p_maxPoint)
    {


        var o = this;
        //  pos = (p_pos);
        minPoint = (p_minPoint);
        maxPoint = (p_maxPoint);
        o.pos = m.MultiplyPoint3x4(p_pos);
        r = HalfSize();

        o.axis[0] = m.MultiplyVector(o.r.x * o.axis[0]);
        o.axis[1] = m.MultiplyVector(o.r.y * o.axis[1]);
        o.axis[2] = m.MultiplyVector(o.r.z * o.axis[2]);
        o.r.x = Normalize(ref o.axis[0]);
        o.r.y = Normalize(ref o.axis[1]);
        o.r.z = Normalize(ref o.axis[2]);
        //pos = m.MultiplyPoint3x4(p_pos);
        //r = HalfSize();


        //axis[0] = m.GetColumn(0);
        //axis[1] = m.GetColumn(1);
        //axis[2] = m.GetColumn(2);


        //float matrixScale = LengthSq(axis[0]);
        //matrixScale = Mathf.Sqrt(matrixScale);
        //r *= matrixScale;
        //matrixScale = 1.0f / matrixScale;
        //axis[0] *= matrixScale;
        //axis[1] *= matrixScale;
        //axis[2] *= matrixScale;

        //Orthonormalize(ref axis[0], ref axis[1], ref axis[2]);

        Vector3 d = targetPoint - pos;
        Vector3 closestPoint = pos; // Start at the center point of the OBB.
        for (int i = 0; i < 3; ++i)
        {// Project the target onto the OBB axes and walk towards that point.
         // closestPoint += Mathf.Clamp(Dot(d, axis[i]), -r[i], r[i]) * axis[i];
            closestPoint += Mathf.Clamp(Dot(d, axis[i]), -r[i], r[i]) * axis[i];
        }

        return closestPoint;
    }


    float DistanceSq(Vector3 a, Vector3 rhs)
    {
        float dx = a.x - rhs.x;
        float dy = a.y - rhs.y;
        float dz = a.z - rhs.z;
        return dx * dx + dy * dy + dz * dz;
    }

    public static bool IntersectBoxBox(OBBThreeD box0, OBBThreeD box1)
    {

        Vector3 v = box1.pos - box0.pos;



        //Compute A's basis

        Vector3 VAx = box0.rotation * new Vector3(1, 0, 0);

        Vector3 VAy = box0.rotation * new Vector3(0, 1, 0);

        Vector3 VAz = box0.rotation * new Vector3(0, 0, 1);



        Vector3[] VA = new Vector3[3];

        VA[0] = VAx;

        VA[1] = VAy;

        VA[2] = VAz;



        //Compute B's basis

        Vector3 VBx = box1.rotation * new Vector3(1, 0, 0);

        Vector3 VBy = box1.rotation * new Vector3(0, 1, 0);

        Vector3 VBz = box1.rotation * new Vector3(0, 0, 1);



        Vector3[] VB = new Vector3[3];

        VB[0] = VBx;

        VB[1] = VBy;

        VB[2] = VBz;



        Vector3 T = new Vector3(Vector3.Dot(v, VAx), Vector3.Dot(v, VAy), Vector3.Dot(v, VAz));



        float[,] R = new float[3, 3];

        float[,] FR = new float[3, 3];

        float ra, rb, t;



        for (int i = 0; i < 3; i++)

        {

            for (int k = 0; k < 3; k++)

            {

                R[i, k] = Vector3.Dot(VA[i], VB[k]);

                FR[i, k] = 1e-6f + Mathf.Abs(R[i, k]);

            }

        }



        // A's basis vectors

        for (int i = 0; i < 3; i++)

        {

            ra = box0.halfExtents[i];

            rb = box1.halfExtents[0] * FR[i, 0] + box1.halfExtents[1] * FR[i, 1] + box1.halfExtents[2] * FR[i, 2];

            t = Mathf.Abs(T[i]);

            if (t > ra + rb) return false;

        }



        // B's basis vectors

        for (int k = 0; k < 3; k++)

        {

            ra = box0.halfExtents[0] * FR[0, k] + box0.halfExtents[1] * FR[1, k] + box0.halfExtents[2] * FR[2, k];

            rb = box1.halfExtents[k];

            t = Mathf.Abs(T[0] * R[0, k] + T[1] * R[1, k] + T[2] * R[2, k]);

            if (t > ra + rb) return false;

        }



        //9 cross products



        //L = A0 x B0

        ra = box0.halfExtents[1] * FR[2, 0] + box0.halfExtents[2] * FR[1, 0];

        rb = box1.halfExtents[1] * FR[0, 2] + box1.halfExtents[2] * FR[0, 1];

        t = Mathf.Abs(T[2] * R[1, 0] - T[1] * R[2, 0]);

        if (t > ra + rb) return false;



        //L = A0 x B1

        ra = box0.halfExtents[1] * FR[2, 1] + box0.halfExtents[2] * FR[1, 1];

        rb = box1.halfExtents[0] * FR[0, 2] + box1.halfExtents[2] * FR[0, 0];

        t = Mathf.Abs(T[2] * R[1, 1] - T[1] * R[2, 1]);

        if (t > ra + rb) return false;



        //L = A0 x B2

        ra = box0.halfExtents[1] * FR[2, 2] + box0.halfExtents[2] * FR[1, 2];

        rb = box1.halfExtents[0] * FR[0, 1] + box1.halfExtents[1] * FR[0, 0];

        t = Mathf.Abs(T[2] * R[1, 2] - T[1] * R[2, 2]);

        if (t > ra + rb) return false;



        //L = A1 x B0

        ra = box0.halfExtents[0] * FR[2, 0] + box0.halfExtents[2] * FR[0, 0];

        rb = box1.halfExtents[1] * FR[1, 2] + box1.halfExtents[2] * FR[1, 1];

        t = Mathf.Abs(T[0] * R[2, 0] - T[2] * R[0, 0]);

        if (t > ra + rb) return false;



        //L = A1 x B1

        ra = box0.halfExtents[0] * FR[2, 1] + box0.halfExtents[2] * FR[0, 1];

        rb = box1.halfExtents[0] * FR[1, 2] + box1.halfExtents[2] * FR[1, 0];

        t = Mathf.Abs(T[0] * R[2, 1] - T[2] * R[0, 1]);

        if (t > ra + rb) return false;



        //L = A1 x B2

        ra = box0.halfExtents[0] * FR[2, 2] + box0.halfExtents[2] * FR[0, 2];

        rb = box1.halfExtents[0] * FR[1, 1] + box1.halfExtents[1] * FR[1, 0];

        t = Mathf.Abs(T[0] * R[2, 2] - T[2] * R[0, 2]);

        if (t > ra + rb) return false;



        //L = A2 x B0

        ra = box0.halfExtents[0] * FR[1, 0] + box0.halfExtents[1] * FR[0, 0];

        rb = box1.halfExtents[1] * FR[2, 2] + box1.halfExtents[2] * FR[2, 1];

        t = Mathf.Abs(T[1] * R[0, 0] - T[0] * R[1, 0]);

        if (t > ra + rb) return false;



        //L = A2 x B1

        ra = box0.halfExtents[0] * FR[1, 1] + box0.halfExtents[1] * FR[0, 1];

        rb = box1.halfExtents[0] * FR[2, 2] + box1.halfExtents[2] * FR[2, 0];

        t = Mathf.Abs(T[1] * R[0, 1] - T[0] * R[1, 1]);

        if (t > ra + rb) return false;



        //L = A2 x B2

        ra = box0.halfExtents[0] * FR[1, 2] + box0.halfExtents[1] * FR[0, 2];

        rb = box1.halfExtents[0] * FR[2, 1] + box1.halfExtents[1] * FR[2, 0];

        t = Mathf.Abs(T[1] * R[0, 2] - T[0] * R[1, 2]);

        if (t > ra + rb) return false;



        return true;

    }


    public Vector3 Closest(Vector3 p)
    {
        Vector3 d = p - this.pos;
        // Start result at center of box; make steps from there
        Vector3 q = this.pos;

        var u = new Vector3[]
        {
            Forward(this.rotation), Up(this.rotation), Right(this.rotation)
        };

        var e = new float[]
        {
            this.halfExtents.x, this.halfExtents.y, this.halfExtents.z
        };

        // For each OBB axis...
        for (int i = 0; i < 3; i++)
        {
            // ...project d onto that axis to get the distance
            // along the axis of d from the box center
            float dist = Vector3.Dot(d, u[i]);
            // If distance farther than the box extents, clamp to the box if (dist > b.e[i]) dist = b.e[i];
            //if (dist > e[i]) dist = e[i];
            if (dist < -e[i]) dist = -e[i];
            // Step that distance along the axis to get world coordinate
            q += dist * u[i];
        }

        return q;
    }

    //from http://blog.diabolicalgame.co.uk/2013_06_01_archive.html
    public static Vector3 Forward(Quaternion q)
    {
        return new Vector3(
          -2 * (q.x * q.z + q.w * q.y),
          -2 * (q.y * q.z - q.w * q.x),
          -1 + 2 * (q.x * q.x + q.y * q.y));
    }
    public static Vector3 Up(Quaternion q)
    {
        return new Vector3(
          2 * (q.x * q.y - q.w * q
               .z),
          1 - 2 * (q.x * q.x + q.z * q.z),
          2 * (q.y * q.z + q.w * q.x));
    }
    public static Vector3 Right(Quaternion q)
    {
        return new Vector3(
          1 - 2 * (q.y * q.y + q.z * q.z),
          2 * (q.x * q.y + q.w * q.z),
          2 * (q.x * q.z - q.w * q.y));
    }

    public Vector3 ClosestPointTo(Vector3 point)
    {
        var Center = pos;
        var HalfExtents = halfExtents;
        // vector from box centre to point
        var directionVector = point - Center;

        // for each OBB axis...
        // ...project d onto that axis to get the distance 
        // along the axis of d from the box center
        // then if distance farther than the box extents, clamp to the box 
        // then step that distance along the axis to get world coordinate
        var XAxis = rotation * new Vector3(1, 0, 0);
        var YAxis = rotation * new Vector3(0, 1, 0);
        var ZAxis = rotation * new Vector3(0, 0, 1);
        var distanceX = Vector3.Dot(directionVector, XAxis);
        if (distanceX > HalfExtents.x) distanceX = HalfExtents.x;
        else if (distanceX < -HalfExtents.x) distanceX = -HalfExtents.x;

        var distanceY = Vector3.Dot(directionVector, YAxis);
        if (distanceY > HalfExtents.y) distanceY = HalfExtents.y;
        else if (distanceY < -HalfExtents.y) distanceY = -HalfExtents.y;

        var distanceZ = Vector3.Dot(directionVector, ZAxis);
        if (distanceZ > HalfExtents.z) distanceZ = HalfExtents.z;
        else if (distanceZ < -HalfExtents.z) distanceZ = -HalfExtents.z;

        return Center + distanceX * XAxis + distanceY * YAxis + distanceZ * ZAxis;
    }


    public static Vector3 ClosestPointTo(Vector3 point,Vector3 obbCenter,Vector3 obbHalfExtents,Quaternion obbrotation )
    {
        var Center = obbCenter;
        var HalfExtents = obbHalfExtents;
        // vector from box centre to point
        var directionVector = point - Center;

        // for each OBB axis...
        // ...project d onto that axis to get the distance 
        // along the axis of d from the box center
        // then if distance farther than the box extents, clamp to the box 
        // then step that distance along the axis to get world coordinate
        var XAxis = obbrotation * new Vector3(1, 0, 0);
        var YAxis = obbrotation * new Vector3(0, 1, 0);
        var ZAxis = obbrotation * new Vector3(0, 0, 1);
        var distanceX = Vector3.Dot(directionVector, XAxis);
        if (distanceX > HalfExtents.x) distanceX = HalfExtents.x;
        else if (distanceX < -HalfExtents.x) distanceX = -HalfExtents.x;
        Debug.Log("distanceX=" + distanceX);
        Debug.Log(XAxis);
        var distanceY = Vector3.Dot(directionVector, YAxis);
        if (distanceY > HalfExtents.y) distanceY = HalfExtents.y;
        else if (distanceY < -HalfExtents.y) distanceY = -HalfExtents.y;

        var distanceZ = Vector3.Dot(directionVector, ZAxis);
        Debug.Log("distanceZ="+ distanceZ);
        if (distanceZ > HalfExtents.z) distanceZ = HalfExtents.z;
        else if (distanceZ < -HalfExtents.z) distanceZ = -HalfExtents.z;
        Debug.Log("distanceZ2=" + distanceZ);
        Debug.Log("ZAxis=" + ZAxis);

        var mulX = distanceX * XAxis;
        var mulY = distanceY * YAxis;
        var mulZ = distanceZ * ZAxis;
        Debug.Log("startAdd");
        Debug.Log(Center);
        Debug.Log(mulX);
        Debug.Log(mulY);
        Debug.Log(mulZ);

        return Center + mulX + mulY + mulZ;
    }

    private void Awake()
    {
        if (useThisTransform)
            obbView = transform;
        else
            obbView = transform.Find("CharacterOBB");


    }


    public bool useThisTransform;

    [SerializeField]
    public Transform obbView;
    private void Update()
    {

        SetFrom(obbView.transform.position, obbView.rotation, 0.5f * obbView.localScale);
        var center = transform.position;
        var size = transform.localScale;

        var halfSize = 0.5f * size;
        minPoint = center - halfSize;
        maxPoint = center + halfSize;
        SetFromWithAix(obbView.transform.position, minPoint, maxPoint, transform.rotation);
    }


    public AABBThreeD MinimalEnclosingAABB()
    {

        AABBThreeD aabb = new AABBThreeD(); ;
        aabb.SetFrom(this);
        return aabb;
    }

    public bool Intersects(AABBThreeD aabb)
    {
        var obb = new OBBThreeD();
        obb.SetFrom(aabb);
        return Intersects(obb, 0);
    }


    void SetFrom(AABBThreeD aabb)
    {
        pos = aabb.center;
        r = aabb.halfSize;
        axis[0] = new Vector3(1, 0, 0);
        axis[1] = new Vector3(0, 1, 0);
        axis[2] = new Vector3(0, 0, 1);
    }



}



