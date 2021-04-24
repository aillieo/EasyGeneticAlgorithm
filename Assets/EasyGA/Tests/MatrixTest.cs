using System;
using System.Collections;
using System.Collections.Generic;
using AillieoUtils;
using UnityEngine;
using UnityEngine.Assertions;

public class MatrixTest : MonoBehaviour
{
    void Start()
    {
        TestFlat();
        TestLoad();
        
        Debug.Log("MatrixTest Pass");
    }

    public void TestFlat()
    {
        Matrix m = new Matrix(2, 3);
        for (int i = 0; i < m.column; ++i)
        {
            for (int j = 0; j < m.row; ++j)
            {
                m[j, i] = i * 10 + j;
            }
        }
        
        Vector v = m.Flat();
        Assert.AreEqual(v[0], m[0,0]);
        Assert.AreEqual(v[1], m[0,1]);
        Assert.AreEqual(v[2], m[0,2]);
        Assert.AreEqual(v[3], m[1,0]);
        Assert.AreEqual(v[4], m[1,1]);
        Assert.AreEqual(v[5], m[1,2]);
    }

    public void TestLoad()
    {
        Vector v = new Vector(new double[]{1,2,3,4,5,6,});
        Matrix m = new Matrix(2, 3);
        m.Load(v);
        
        Assert.AreEqual(v[0], m[0,0]);
        Assert.AreEqual(v[1], m[0,1]);
        Assert.AreEqual(v[2], m[0,2]);
        Assert.AreEqual(v[3], m[1,0]);
        Assert.AreEqual(v[4], m[1,1]);
        Assert.AreEqual(v[5], m[1,2]);
    }
}
