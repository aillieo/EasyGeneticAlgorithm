using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AillieoUtils;
using UnityEngine.Assertions;


public class VectorTest : MonoBehaviour
{
    void Start()
    {
        TestAdd();
        TestConstructWithArray();
        
        Debug.Log("VectorTest Pass");
    }

    public void TestAdd()
    {
        
    }

    public void TestConstructWithArray()
    {
        double[] src = new[] {0.1, 0.22, -9.8, 0, -88, 98989011.2};
        Vector v = new Vector(src);
        Assert.AreEqual(v.size, src.Length);
        for (int i = 0; i < src.Length; ++i)
        {
            Assert.AreEqual(v[i], src[i]);
        }
    }
}
