using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AITank
{
    public class Unit : MonoBehaviour
    {
        public float fullHealth = 100;
        private float _curHealth;

        public float health
        {
            get { return _curHealth / fullHealth; }
            set { _curHealth = fullHealth * value; }
        }

        public virtual void Setup()
        {
            health = 1;
            gameObject.SetActive(true);
        }

        public bool ApplyDamage(float point)
        {
            _curHealth -= point;
            if (!(_curHealth <= 0)) return false;
            Dead();
            return true;
        }

        public virtual void Dead()
        {
            gameObject.SetActive(false);
        }
    }
}