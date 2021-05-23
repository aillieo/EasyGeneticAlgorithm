using AillieoUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sample
{
    public class BallAgent : Agent
    {
        public GameObject ball;
        public GameObject plat;
        public Material black;
        public Material white;
        private MeshRenderer[] renderers;

        private static Vector3 G = new Vector3(0, -9.8f, 0);

        private Vector3 ballVelocity;
        private Vector2 platVelocity;
        
        public override void Restart()
        {
            score = 0;
            running = true;
            plat.transform.localPosition = Vector3.zero;
            Vector3 initPos = new Vector3(
                Random.Range(-0.1f, 0.1f),
                3,
                Random.Range(-0.1f, 0.1f));

            ball.transform.localPosition = initPos;
            ballVelocity = Vector3.zero;
            platVelocity = Vector2.zero;

            if (renderers == null)
            {
                renderers = GetComponentsInChildren<MeshRenderer>();
            }
            foreach (var r in renderers)
            {
                r.material = white;
            }
        }

        private void OnFail()
        {
            this.running = false;

            if (renderers == null)
            {
                renderers = GetComponentsInChildren<MeshRenderer>();
            }
            foreach (var r in renderers)
            {
                r.material = black;
            }
        }

        public override void OnUpdate(float deltaTime)
        {
            if (!running)
            {
                return;
            }
            UpdatePlat(deltaTime);
            UpdateBall(deltaTime);
        }

        private void UpdateBall(float deltaTime)
        {
            ball.transform.localPosition += ballVelocity * deltaTime;

            if (Mathf.Abs(ball.transform.localPosition.x) > 4 || Mathf.Abs(ball.transform.localPosition.z) > 4)
            {
                // 失败了
                OnFail();
                return;
            }

            if (ball.transform.localPosition.y < 0)
            {
                Vector3 delta = ball.transform.localPosition - plat.transform.localPosition;

                if (Mathf.Abs(delta.x) > 0.5 || Mathf.Abs(delta.z) > 0.5)
                {
                    // 失败了
                    OnFail();
                    return;
                }
                else
                {
                    ballVelocity.y = Mathf.Abs(ballVelocity.y);
                    delta.y = 0;
                    ballVelocity += delta * 0.9f;

                    // 击中了
                    score++;
                }
            }
            else
            {
                ballVelocity += G * deltaTime;
            }
        }


        private Vector cachedInput;
        private void UpdatePlat(float deltaTime)
        {
            if(cachedInput == null)
            {
                cachedInput = new Vector(8);
            }

            Vector input = cachedInput;
            input[0] = ball.transform.localPosition.x;
            input[1] = ball.transform.localPosition.y;
            input[2] = ball.transform.localPosition.z;
            input[3] = ballVelocity.x;
            input[4] = ballVelocity.y;
            input[5] = ballVelocity.z;
            input[6] = plat.transform.localPosition.x;
            input[7] = plat.transform.localPosition.z;

            Vector output = this.dna.Process(input);
            platVelocity.x = -1 + 2 * (float)output[0];
            platVelocity.y = -1 + 2 * (float)output[1];

            //Debug.LogError("input = " + input);
            //Debug.LogError("out = " + output);

            plat.transform.localPosition += new Vector3(platVelocity.x, 0, platVelocity.y) * deltaTime;
        }

        public override DNA CreateRawDNA()
        {
            return new DNA(8, 8, 2);
        }

        public override float GetFitness()
        {
            return score * score + 0.01f;
        }
    }
}
