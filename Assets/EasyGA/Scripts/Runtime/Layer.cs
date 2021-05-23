using System;
using System.Collections.Generic;

namespace AillieoUtils
{
    public class Layer
    {
        public readonly int inputSize;
        public readonly int outputSize;
        public Matrix weights { get; private set; }
        public Vector bias;

        public Layer(int inputSize, int outputSize)
        {
            this.inputSize = inputSize;
            this.outputSize = outputSize;

            weights = new Matrix(inputSize, outputSize);

            for (int i = 0; i < inputSize; i++)
            {
                for (int j = 0; j < outputSize; j++)
                {
                    weights[i, j] = 0;
                }
            }
        }
        
        public Layer(Matrix matrix)
        {
            this.inputSize = matrix.row;
            this.outputSize = matrix.column;
            this.weights = matrix;
        }

        private Vector input = null;
        private Vector output = null;
        public Vector Forward(Vector input)
        {
            this.input = input;
            if (output == null)
            {
                output = new Vector(weights.column);
            }

            Matrix.MultiplyNoAllocUnsafe(input, weights, output);

            for (int i = 0; i < output.size; i++)
            {
                output[i] = FunctionUtils.TanH((float)output[i]);
            }

            return output;
        }

        public Vector Backward()
        {
            throw new NotImplementedException();
        }
    }
}
