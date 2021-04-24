using System;
using System.Collections.Generic;
using System.Linq;

namespace AillieoUtils
{
    public class NeuralNetwork
    {
        public List<Layer> layers { get; private set; }

        private int[] layerSize;

        public NeuralNetwork(params int[] layerSize)
        {
            this.layerSize = layerSize;
            this.layers = new List<Layer>();

            for (int i = 0; i < layerSize.Length - 1; i++)
            {
                Layer l = new Layer(layerSize[i], layerSize[i + 1]);
                layers.Add(l);
            }
        }

        public NeuralNetwork(IEnumerable<Matrix> weights)
        {
            var ws = weights.ToArray();
            List<int> ls = new List<int>();
            ls.Add(ws[0].row);
            for (int i = 0; i < ws.Length; ++i)
            {
                ls.Add(ws[i].column);
            }
            this.layerSize = ls.ToArray();

            layers = new List<Layer>();
            for (int i = 0; i < ws.Length; ++i)
            {
                Layer l = new Layer(ws[i]);
                this.layers.Add(l);
            }
        }

        public int GetLayerCount()
        {
            return layers.Count;
        }

        public Layer GetLayer(int index)
        {
            return layers[index];
        }

        public Vector Forward(Vector input)
        {
            Vector output = input;

            for (int i = 0; i < layerSize.Length - 1; i++)
            {
                output = layers[i].Forward(output);
            }

            return output;
        }

        public Vector Backward()
        {
            throw new NotImplementedException();
        }
    }
}
