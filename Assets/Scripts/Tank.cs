using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AITank
{
    public class Tank : Unit
    {
        public LayerMask enemyMask;
        public float maxSpeed;
        public float maxTorque;

        public float shootCooldown;
        public Transform shootPoint;
        public GameObject bullet;

        public float score = 0;

        public Rigidbody _rigidbody { get; private set; }
        private Collider _collider;

        public bool weaponReady { get; private set; } = true;

        public override void Setup()
        {
            weaponReady = true;
            score = 0;
            base.Setup();
        }

        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
        }

        public void SetMove(float dir)
        {
            _rigidbody.velocity = transform.forward * maxSpeed * dir;
        }

        public void SetRotate(float dir)
        {
            _rigidbody.angularVelocity = transform.up * maxTorque * dir;
        }

        public void Score(float v)
        {
            score += v;
        }

        public void Shoot()
        {
            if (!weaponReady || !gameObject.activeSelf) return;
            weaponReady = false;
            Instantiate(bullet, shootPoint.position, shootPoint.rotation).GetComponent<ShootObject>().Setup(Score);

            StartCoroutine(CooldownWeapon());
        }

        private IEnumerator CooldownWeapon()
        {
            yield return new WaitForSeconds(shootCooldown);
            weaponReady = true;
        }

        public Transform ClosestEnemy(float viewRange)
        {
            var cols = new List<Collider>(Physics.OverlapSphere(transform.position, viewRange, enemyMask));
            cols.Remove(_collider);
            var firstOrDefault = cols.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).FirstOrDefault();
            if (firstOrDefault != null)
                return firstOrDefault.transform;
            return null;
        }

        public override void Dead()
        {
            base.Dead();
            score = 0;
        }
    }
}