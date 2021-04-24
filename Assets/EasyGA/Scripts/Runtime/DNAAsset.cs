using System.Linq;
using UnityEngine;

namespace AillieoUtils
{
    //[CreateAssetMenu(fileName = "DNAAsset")]
    public class DNAAsset: ScriptableObject
    {
        public double[] values;

        public void Save(DNA dna)
        {
            values = dna.Serialize().ToArray();
        }

        public DNA Load()
        {
            return DNA.Deserialize(new Vector(values));
        }
    }
}
