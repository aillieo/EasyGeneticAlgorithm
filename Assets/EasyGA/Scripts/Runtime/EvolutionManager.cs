using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AillieoUtils
{
    public class EvolutionManager : MonoBehaviour
    {
        public Agent agentTemplate;
        public Vector2Int amount = new Vector2Int(5,5);
        public Vector2 distance = new Vector2(5,5);
        public float elimination = 0.25f;

        [NonSerialized]
        public List<Agent> agents = new List<Agent>();
        public float mutation = 0.01f;

        public int generation { get; private set; } = 0;
        private float timer = 0;


        public GenerationAsset baseGeneration;
        
        private void Awake()
        {
            for (int i = 0; i < amount.x; ++ i)
            {
                for (int j = 0; j < amount.y; ++j)
                {
                    Agent newAgent = GameObject.Instantiate(agentTemplate, this.transform);
                    agents.Add(newAgent);
                    newAgent.transform.localPosition = new Vector3(i * distance.x, 0, j * distance.y);
                }
            }


            List<DNA> pre = new List<DNA>();
            if (baseGeneration != null)
            {
                this.generation = baseGeneration.generation;
                foreach (var da in baseGeneration.dnaAssets)
                {
                    DNA d = da.Load();
                    pre.Add(d);
                }
            }

            int i2 = 0;
            foreach (var a in agents)
            {
                a.Init(pre.Count > 0 ? pre[i2++ % pre.Count] : null);
                a.Restart();
            }
        }

        private void FixedUpdate()
        {
            float dt = Time.fixedDeltaTime;

            //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            //sw.Start();

            foreach (var a in agents)
            {
                if(a.running)
                {
                    a.OnUpdate(dt);
                }
            }

            //sw.Stop();
            //Debug.LogError("sw = " + sw.ElapsedMilliseconds);

            timer -= dt;

            int runningCount = agents.Count(a => a.running);

            if (runningCount == 0)
            {
                Regenerate();
                return;
            }

            if (timer < 0 && runningCount < agents.Count * (1 - elimination))
            {
                Regenerate();
                return;
            }
        }

        private void Regenerate()
        {
            float best = agents.Max(a => a.score); 
            
            Debug.Log($"generation={generation} avg={agents.Average(a => a.score)} best={best}");

            timer = GetTimeLimit(best);

            if (generation == 0)
            {
                foreach (var a in agents)
                {
                    a.Init(null);
                }
            }
            else
            {
                var list = ReCollect();
                foreach (var a in agents.ToArray())
                {
                    var ws = list.RandomTake(2).ToList();
                    DNA p0 = ws[0];
                    DNA p1 = ws[1];

                    //Debug.LogError($"选中的是 {p0.testValue}xx{p1.testValue}");

                    a.dna = DNA.Cross(p0, p1, mutation);
                }
            }

            foreach (var a in agents)
            {
                a.Restart();
            }
            generation++;
        }

        public float GetTimeLimit(float bestScore)
        {
            return 10f + 1.5f * bestScore;
        }
        
        private WeightedSet<DNA> ReCollect()
        {
            Agent[] sorted = agents.OrderByDescending(a => a.score).ToArray();
            int sourceCount = Mathf.Max((int)(sorted.Length * elimination), 2);
            Agent[] result = new Agent[sourceCount];
            Array.Copy(sorted, result, result.Length);


            WeightedSet<DNA> list = new WeightedSet<DNA>();
            foreach (var a in result)
            {
                list.Add(a.dna, a.GetFitness());
            }

            return list;
        }
    }
}
