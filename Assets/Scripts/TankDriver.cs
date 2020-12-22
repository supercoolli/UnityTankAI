using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SPINACH.AI;
using System;

namespace AITank
{
    public class TankDriver : MonoBehaviour
    {
        public Tank target;
        public float viewRange;


        public GeneticOptimizeableNerualNetwork network;

        public double Fitness { get; set; }

        private void Awake()
        {
            target = GetComponent<Tank>();

            network = new GeneticOptimizeableNerualNetwork(5, 3);
            var actvationFunction = new TanhFunction();
            for (int i = 0; i < network.activateFunctions.Length; i++)
            {
                network.SetActivationFunctionForLayer(i, actvationFunction);
            }
        }

        public double CalculateFintness()
        {
            network.fitness = target.score;
            return network.fitness;
        }

        public void DoSomethingUseful()
        {
            var inputs = new double[5];
            var closestEnemy = target.ClosestEnemy(viewRange);

            //distance between enemy.
            inputs[0] = closestEnemy != null ? Vector3.Distance(transform.position, closestEnemy.position) / viewRange : 1f;
            //cos form enemy.
            inputs[1] = closestEnemy != null ? Vector3.Dot(transform.right, (closestEnemy.position - transform.position).normalized) : 1f;
            //is weapon ready
            inputs[2] = target.weaponReady ? 1d : 0d;
            //current speed.
            inputs[3] = target._rigidbody.velocity.magnitude / target.maxSpeed;
            //current torque.
            inputs[4] = target._rigidbody.angularVelocity.magnitude / target.maxTorque;

            var output = network.Compute(inputs);

            target.SetMove((float)output[0]);
            target.SetRotate((float)output[1]);
            if (output[2] > 0) target.Shoot();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, viewRange);
        }
    }
}