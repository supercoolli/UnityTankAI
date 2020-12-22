using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AITank
{
    public class ShootObject : MonoBehaviour
    {
        public LayerMask hitLayerMask;
        public float hit, hitRange, launchSpeed;
        public GameObject expEffect;

        private Action<float> _scoreCallback;

        public void Setup(Action<float> scoreCallback)
        {
            _scoreCallback = scoreCallback;
        }

        void Start()
        {
            GetComponent<Rigidbody>().velocity = transform.forward * launchSpeed;
        }

        void Update()
        {
            var cols = Physics.OverlapSphere(transform.position, hitRange, hitLayerMask);
            foreach (var col in cols)
            {
                var unit = col.GetComponentInParent<Unit>();
                if (!unit) continue;
                var killed = unit.ApplyDamage(hit);
                _scoreCallback(killed ? 5 : 1);
            }

            if (cols.Length <= 0) return;
            Destroy(Instantiate(expEffect, transform.position, Quaternion.identity), 2f);
            Destroy(gameObject);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, hitRange);
        }
    }
}