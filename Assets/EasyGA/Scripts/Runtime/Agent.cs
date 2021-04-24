using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AillieoUtils
{

    public abstract class Agent : MonoBehaviour
    {
        public int score { get; protected set; } = 0;
        public bool running { get; protected set; } = false;

        public DNA dna { get; set; }

        public void Init(DNA dna)
        {
            if (dna == null)
            {
                dna = CreateRawDNA();
            }

            this.dna = dna;
        }

        public abstract float GetFitness();

        public abstract DNA CreateRawDNA();

        public abstract void Restart();

        public abstract void OnUpdate(float deltaTime);

    }
}
