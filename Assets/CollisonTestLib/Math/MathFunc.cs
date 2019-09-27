using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathFunc
{

    public static float ABSDOT3(Vector3 v1, Vector3 v2)
    {
        return (Mathf.Abs((v1)[0] * (v2)[0]) + Mathf.Abs((v1)[1] * (v2)[1]) + Mathf.Abs((v1)[2] * (v2)[2]));
    }

    public static Vector3 Abs(Vector3 v3)
    {
        return new Vector3(Mathf.Abs(v3.x), Mathf.Abs(v3.y), Mathf.Abs(v3.z));
    }

    public static float DistanceSq(Vector3 a, Vector3 rhs)
    {
        float dx = a.x - rhs.x;
        float dy = a.y - rhs.y;
        float dz = a.z - rhs.z;
        return dx * dx + dy * dy + dz * dz;
    }
}
