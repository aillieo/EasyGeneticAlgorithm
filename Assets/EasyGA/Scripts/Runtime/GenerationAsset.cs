using UnityEngine;

namespace AillieoUtils
{
    //[CreateAssetMenu(fileName = "GenerationAsset")]
    public class GenerationAsset: ScriptableObject
    {
        public int generation;
        public DNAAsset[] dnaAssets;
    }
}
