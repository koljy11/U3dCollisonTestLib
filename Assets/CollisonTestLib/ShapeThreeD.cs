using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeThreeD : MonoBehaviour {
    public System.Action<ShapeThreeD> collsionEnter;
    public static int Character = 1;
    public static int Build = 2;
    public static int Bullet = 3;

    public int mask = 2;

    //~0000000000000000000
    //11111111111111111111
    public int collsionMask=~0;//0取反则全部是打开
    protected virtual void Start()
    {
       
    }

    private void OnDestroy()
    {
       
    }








}
