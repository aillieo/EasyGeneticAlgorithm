using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AillieoUtils
{
    public static class FunctionUtils
    {
        public static float Sigmoid(float input)
        {
            return 1 / (float)(1 + Mathf.Exp(-input));
        }

        public static void Sigmoid(Vector input, Vector output)
        {
            for (int i = 0; i < input.size; ++i)
            {
                output[i] = Sigmoid((float)input[i]);
            }
        }

        public static Vector Sigmoid(Vector input)
        {
            Vector output = new Vector(input.size);
            Sigmoid(input, output);
            return output;
        }

        public static float TanH(float input)
        {
            return (float)Math.Tanh(input);
        }

        public static void TanH(Vector input, Vector output)
        {
            for (int i = 0; i < input.size; ++i)
            {
                output[i] = TanH((float)input[i]);
            }
        }

        public static Vector TanH(Vector input)
        {
            Vector output = new Vector(input.size);
            TanH(input, output);
            return output;
        }

        public static float ReLU(float input)
        {
            return Mathf.Max(0, input);
        }

        public static void ReLU(Vector input, Vector output)
        {
            for (int i = 0; i < input.size; ++i)
            {
                output[i] = ReLU((float)input[i]);
            }
        }

        public static Vector ReLU(Vector input)
        {
            Vector output = new Vector(input.size);
            ReLU(input, output);
            return output;
        }
    }
}
