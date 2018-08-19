using UnityEngine;
using Artificialintelligence.Neural;
using Artificialintelligence.Genetic;
using Extensions;
using System.Collections;
using System.Collections.Generic;

namespace Artificialintelligence
{
    public abstract class IntelligentEntity : MonoBehaviour
    {
        private readonly int raycast_layerMask = ~((1 << Layers.Player) | (1 << Layers.ScoreWall));
        private float raycast_distance = 75;
        // Changing updateRate may disrupt AI as thrust also changes.
        private static readonly float updateRate = 0.20f;
        private float updateLast = 0;

        [SerializeField]
        protected float size = 1;
        private float time_perScore = 5;
        private float time_lastScore = 0;
        protected bool init = false;
        protected Color color = Color.green;

        protected new Rigidbody2D rigidbody;
        protected Vector2 startPosition;
        protected Quaternion startOrientation;

        public Gene<NeuronLayer> Gene { get; private set; }
        protected NeuralNetwork net;

        public float Fitness
        {
            get { return Gene.Fitness; }
            set
            {
                if (value > Gene.Fitness)
                {
                    time_lastScore = Time.time;
                    // Blink(1);
                }
                Gene.Fitness = value;
            }
        }

        public bool IsInactive
        {
            get { return rigidbody.IsSleeping(); }
        }

        public bool IsVictorious { get; set; }

        public void Init(Gene<NeuronLayer> gene, NeuralNetwork net)
        {
            this.Gene = gene;
            this.net = net;
            this.startPosition = transform.position;
            this.startOrientation = transform.rotation;
            this.Fitness = 0;
            this.init = true;
        }

        private void Awake()
        {
            IsVictorious = false;
            raycast_distance *= size;
            rigidbody = GetComponent<Rigidbody2D>();
            CreateShape();
            time_lastScore = Time.time;
            updateLast = Time.time;
        }

        private void FixedUpdate()
        {
            if (Time.time - updateLast > updateRate)
            {
                updateLast = Time.time;
                SelfUpdate();
            }
        }

        private void SelfUpdate()
        {
            if (init == true && IsInactive == false)
            {
                if (Time.time - time_lastScore < time_perScore)
                {
                    ProcessOutput(GenerateOutput());
                }
                else
                {
                    Defeat();
                }
            }
        }

        protected abstract void ProcessOutput(float[] outputs);

        protected float[] GenerateOutput(float[] inputs = null)
        {
            if (inputs == null)
                inputs = GenerateInput();
            
            float[] outputs = net.FeedForward(inputs);
            // Debug.Log(inputs[0] + ", " + inputs[1] + " = " + outputs[0]);
            float total = 0;
            foreach (var n in net.hiddenLayers[0].neurons)
            {
                total += n.input;
            }
            Debug.Log(total);

            return outputs;
        }

        protected abstract float[] GenerateInput();

        protected abstract void CreateShape();

        protected abstract void ChangeColor(Color color);

        public void Defeat()
        {
            Freeze();
        }

        public void Blink(int count = 1)
        {
            StartCoroutine(BlinkCoroutine(count));
        }

        private IEnumerator BlinkCoroutine(int count = 1)
        {
            for (int i = 0; i < count; i++)
            {
                ChangeColor(Color.white);
                yield return new WaitForSeconds(0.1f);
                ChangeColor(color);
                yield return new WaitForSeconds(0.1f);
            }
        }

        public void Victory()
        {
            IsVictorious = true;
            Freeze();
            ChangeColor(Color.cyan);
        }

        public void Freeze()
        {
            rigidbody.isKinematic = true;
            rigidbody.Sleep();
            rigidbody.velocity = Vector3.zero;
            ChangeColor(Color.gray);
            
        }

        public void Unfreeze()
        {
            IsVictorious = false;
            rigidbody.isKinematic = false;
            rigidbody.WakeUp();
            ChangeColor(color);
        }

        public void Restart()
        {
            transform.position = startPosition;
            transform.rotation = startOrientation;
            time_lastScore = Time.time;
            Fitness = 0;
            Unfreeze();
        }

        protected float[] Raycast(params float[] angles)
        {

            return Physics2DExt.Raycast(transform, raycast_distance, raycast_layerMask, angles);
        }

        protected float[] RaycastReflect(params float[] angles)
        {
            return Physics2DExt.RaycastReflect(transform, raycast_distance, raycast_layerMask, angles);
        }
    }
}