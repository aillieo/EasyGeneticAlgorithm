using System.Collections;
using System.Collections.Generic;
using System.Net;
using AillieoUtils;
using UnityEngine;
using UnityEngine.Assertions;

public class DNATest : MonoBehaviour
{
    void Start()
    {
        TestSerialize();
        TestDeserialize();

        TestSerAndDeser();
        
        Debug.Log("DNATest Pass");

        
    }

    public void TestSerialize()
    {
        Matrix m = new Matrix(new double[,]
        {
            {0,1,2},
            {10,11,12}
        });
        
        DNA dna = new DNA(new Matrix[]{m});
        Vector v = dna.Serialize();
        double[] ds = v.ToArray();
        
        Assert.AreEqual(ds.Length, 8);
        Assert.AreEqual(ds[0], 2);
        Assert.AreEqual(ds[1], 3);
        Assert.AreEqual(ds[2], 0);
        Assert.AreEqual(ds[3], 1);
        Assert.AreEqual(ds[4], 2);
        Assert.AreEqual(ds[5], 10);
        Assert.AreEqual(ds[6], 11);
        Assert.AreEqual(ds[7], 12);
    }
    
    
    public void TestDeserialize()
    {
        double[] ds = new double[] { 2,3,0,1,2,10,11,12 };
        Vector v = new Vector(ds);
        DNA dna = DNA.Deserialize(v);

        Vector input = new Vector(new double[]{3, 2});
        Vector output = input * dna.GetNetwork().GetLayer(0).weights;
        
        Assert.AreEqual(output.size, 3);
        Assert.AreEqual(output[0], 20);
        Assert.AreEqual(output[1], 25);
        Assert.AreEqual(output[2], 30);
    }


    public void TestSerAndDeser()
    {
        double[] ds = new double[]
        {
            2,3,
            0.1,0.2,0.3,
            0.2,0.3,0.4,
            
            3,3,
            0.1,0.2,0.7,
            0.9,0.9,1.2,
            0.9,2.1,2.2,
            
            3,2,
            0.2,0.9,
            2.2,2.1,
            0.9,0.9
        };

        DNA dna = DNA.Deserialize(new Vector(ds));
        Vector v = dna.Serialize();
        double[] d2 = v.ToArray();

        Assert.AreEqual(ds.Length, ds.Length);

        for (int i = 0 ; i < ds.Length; ++ i)
        {
            Assert.AreEqual(ds[i], d2[i]);
        }
    }
}
