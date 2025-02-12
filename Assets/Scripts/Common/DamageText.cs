using System;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DamageText : MonoBehaviour
{
    private DamageTextManager.DamageTextData m_damageTextData;
    private TMP_Text m_text;
    private RectTransform m_rect;
    private RectTransform m_canvasRect;
    private float m_lifeTime;
    private float m_alpha;
    private Vector3 m_offset;

    private int minFontSize = 30;
    private int maxFontSize = 70;

    private void Awake()
    {
        m_text = transform.GetComponent<TMP_Text>();
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

            // 动画-向上飘和透明度
            m_offset = new Vector3(0, m_offset.y + 1 * Time.deltaTime, 0);
            m_alpha -= 0.5f * Time.deltaTime;

            // 三维世界坐标转换为canvas屏幕坐标
            var canvasRectTransform = m_canvasRect;
            var showPosition = m_damageTextData.position + m_offset;
            var screenPosition = Camera.main.WorldToScreenPoint(showPosition);

            // 屏幕坐标(左上坐标系)转换为UI坐标(中间坐标系)
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRectTransform,
                screenPosition,
                null,
                out Vector2 uiPosition);

            m_rect.anchoredPosition = uiPosition;

            // 根据角色与伤害数字出现的位置计算显示文字大小
            var characterTrans = CharacterManager.Instance.GetTransform();
            var distanceVector = characterTrans.position - transform.position;
            var distance = distanceVector.magnitude;
            m_text.fontSize = (int) Mathf.Lerp(minFontSize, maxFontSize, Mathf.InverseLerp(0f, 20f, distance));

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

    public void SetData(DamageTextManager.DamageTextData data, Transform canvasTrans)
    {
        m_damageTextData = data;
        m_canvasRect = canvasTrans.GetComponent<RectTransform>();
        m_text.text = data.damage.ToString();
    }
}