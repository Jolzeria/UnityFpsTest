using System;
using Manager;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DamageText : MonoBehaviour
{
    private DamageTextManager.DamageTextData m_damageTextData;
    private Text m_text;
    private RectTransform m_rect;
    private RectTransform m_canvasRect;
    private float m_lifeTime;
    private float m_alpha;
    private Vector3 m_offset;

    private int minFontSize = 40;
    private int maxFontSize = 80;

    private void Awake()
    {
        m_text = transform.GetComponent<Text>();
        m_rect = transform.GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        m_lifeTime = 2f;
        m_alpha = 1f;
        m_offset = Vector3.zero;
    }

    private void OnDisable()
    {
        
    }

    void Update()
    {
        if (m_lifeTime > 0)
        {
            if (m_canvasRect == null)
                return;

            m_offset = new Vector3(0, m_offset.y + 1 * Time.deltaTime, 0);
            m_alpha -= 0.5f * Time.deltaTime;

            var canvasRectTransform = m_canvasRect;
            var showPosition = m_damageTextData.position + m_offset;
            var screenPosition = Camera.main.WorldToScreenPoint(showPosition);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRectTransform,
                screenPosition,
                null,
                out Vector2 uiPosition);

            m_rect.anchoredPosition = uiPosition;

            var characterTrans = CharacterManager.Instance.GetTransform();
            var distanceVector = characterTrans.position - transform.position;
            var sqrDistance = distanceVector.sqrMagnitude;
            m_text.fontSize = (int) Mathf.Lerp(minFontSize, maxFontSize, Mathf.InverseLerp(0f, 1300000f, sqrDistance));

            // 控制渐隐
            var currentColor = m_text.color;
            currentColor.a = m_alpha;
            m_text.color = currentColor;
        }

        m_lifeTime -= Time.deltaTime;
        if (m_lifeTime <= 0)
        {
            DamageTextPool.Instance.Release(gameObject);
            DamageTextManager.Instance.RemoveLivedText(m_damageTextData);
        }
    }

    public void SetData(DamageTextManager.DamageTextData data, Transform trans)
    {
        m_damageTextData = data;
        m_canvasRect = trans.GetComponent<RectTransform>();
        m_text.text = data.damage.ToString();
    }
}