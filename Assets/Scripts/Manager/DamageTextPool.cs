using UnityEngine;
using System.Collections.Generic;

namespace Manager
{
    public class DamageTextPool : Singleton<DamageTextPool>
    {
        private Queue<GameObject> m_DamageTextPool;
        private Transform m_PoolTransform;

        public override void Init()
        {
            base.Init();

            m_DamageTextPool = new Queue<GameObject>();
        }

        public override void UnInit()
        {
            base.UnInit();

            m_DamageTextPool?.Clear();
            m_DamageTextPool = null;
        }

        public void SetParent(Transform parent)
        {
            m_PoolTransform = parent;

            for (int i = 0; i < 3; i++)
            {
                var damageText = CreateDamageText();
                if (damageText == null)
                {
                    continue;
                }

                Release(damageText);
            }
        }

        public GameObject Get()
        {
            if (m_DamageTextPool == null) return null;

            if (m_DamageTextPool.Count > 0)
            {
                var damageText = m_DamageTextPool.Dequeue();
                damageText.SetActive(true);
                return damageText;
            }

            return CreateDamageText();
        }

        public void Release(GameObject damageText)
        {
            if (damageText == null) return;
            if (m_DamageTextPool == null) return;

            damageText.SetActive(false);
            m_DamageTextPool.Enqueue(damageText);

            if (m_PoolTransform != null)
            {
                damageText.transform.SetParent(m_PoolTransform);
            }
        }

        private GameObject CreateDamageText()
        {
            var prefab = Resources.Load<GameObject>("DamageText");
            var obj = GameObject.Instantiate(prefab);
            return obj;
        }
    }
}