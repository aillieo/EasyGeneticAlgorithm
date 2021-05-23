using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AillieoUtils
{
    [CustomEditor(typeof(EvolutionManager))]
    public class EvolutionManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Save"))
            {
                string file = $"Assets/{System.DateTime.Now:yyyyMMdd_HH_mm_ss_ffff}.asset";
                GenerationAsset asset = DNAAsset.CreateInstance<GenerationAsset>();
                AssetDatabase.CreateAsset(asset, file);

                List<Agent> agents = (target as EvolutionManager).agents;

                asset.dnaAssets = new DNAAsset[agents.Count];
                asset.generation = (target as EvolutionManager).generation;

                for (int i = 0; i < agents.Count; ++ i)
                {
                    DNAAsset dasset = DNAAsset.CreateInstance<DNAAsset>();
                    asset.dnaAssets[i] = dasset;

                    dasset.Save(agents[i].dna);
                    AssetDatabase.AddObjectToAsset(dasset, asset);
                }

                AssetDatabase.ImportAsset(file);
                EditorUtility.SetDirty(asset);
                //AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                ProjectWindowUtil.ShowCreatedAsset(asset);
            }

            if (GUILayout.Button("++"))
            {
                UnityEngine.Time.timeScale *= 2f;
            }
            
            if (GUILayout.Button("--"))
            {
                UnityEngine.Time.timeScale /= 2f;
            }
            
            if (GUILayout.Button("<>"))
            {
                UnityEngine.Time.timeScale = 1f;
            }
        }
    }
}
