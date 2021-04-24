using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AillieoUtils
{
    public class DNA
    {
        private NeuralNetwork network;

        private int[] layerSize;

        private static float GetRawValue()
        {
            return Random.Range(-10f, 10f);
        }

        public DNA(params int[] layerSize)
        {
            List<Matrix> weights = new List<Matrix>();
            for (int i = 0; i < layerSize.Length - 1; ++i)
            {
                Matrix matrix = new Matrix(layerSize[i], layerSize[i + 1]);
                for (int j = 0; j < matrix.row; ++ j)
                {
                    for (int k = 0; k < matrix.column; ++ k)
                    {
                        matrix[j, k] = GetRawValue();
                    }
                }
                weights.Add(matrix);
            }
            Init(weights);
        }

        public DNA(IEnumerable<Matrix> weights)
        {
            Init(weights);
        }
        
        private void Init(IEnumerable<Matrix> weights)
        {
            var ws = weights.ToArray();
            List<int> layers = new List<int>();
            layers.Add(ws[0].row);
            for (int i = 0; i < ws.Length; ++i)
            {
                layers.Add(ws[i].column);
            }
            this.layerSize = layers.ToArray();
            this.network = new NeuralNetwork(weights);
        }

        public static DNA Cross(DNA parent0, DNA parent1, float mutation)
        {
            DNA dna = new DNA(parent0.layerSize);
            for (int i = 0; i < dna.network.layers.Count; ++i)
            {
                Matrix matrix = dna.network.layers[i].weights;
                for (int j = 0; j < matrix.row; ++j)
                {
                    for (int k = 0; k < matrix.column; ++k)
                    {
                        matrix[j, k] = Random.value > 0.5 ? 
                            parent0.network.layers[i].weights[j,k] 
                            :
                            parent1.network.layers[i].weights[j, k];

                        if (Random.value < mutation)
                        {
                            matrix[j, k] = GetRawValue();
                        }
                    }
                }
            }

            return dna;
        }

        public Vector Process(Vector input)
        {
            Vector output = this.network.Forward(input);
            return output;
        }
        
        public NeuralNetwork GetNetwork()
        {
            return network;
        }

        public Vector Serialize()
        {
            return Vector.Join(network.layers.Select(l => {
                Vector v = new Vector(l.weights.column * l.weights.row + 2);
                Vector w = l.weights.Flat();
                v[0] = l.weights.row;
                v[1] = l.weights.column;
                for (int i = 2; i < v.size; ++ i)
                {
                    v[i] = w[i - 2];
                }
                return v;
            }));
        }

        public static DNA Deserialize(Vector vector)
        {
            int i = 0;
            List<Matrix> ms = new List<Matrix>();
            while (i < vector.size)
            {
                int row = (int)vector[i++];
                int col = (int)vector[i++];
                Matrix m = new Matrix(row, col);
                int count = row * col;
                m.Load(vector.Silce(i, i + count));
                
                ms.Add(m);
                i+= count;
            }
            return new DNA(ms);
        }

        public override string ToString()
        {
            return string.Join("/", this.network.layers.Select(l => l.weights));
        }
    }

}
