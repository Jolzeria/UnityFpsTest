using System;
using UnityEngine;

namespace Common
{
    public class BulletMarks : MonoBehaviour
    {
        private float lifetime;

        private void Start()
        {
            lifetime = 5;
        }

        private void Update()
        {
            if (lifetime <= 0)
            {
                lifetime = 5;
                BulletMarksPool.Instance.Release(gameObject);
            }

            lifetime -= Time.deltaTime;
        }
    }
}