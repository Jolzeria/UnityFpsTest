using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    private DamageTextManager.DamageTextData m_damageTextData;
    private Text m_text;
    private RectTransform m_rect;
    private RectTransform m_canvasRect;
    private float lifeTime;

    private int minFontSize = 40;
    private int maxFontSize = 80;

    private void Awake()
    {
        m_text = transform.GetComponent<Text>();
        m_rect = transform.GetComponent<RectTransform>();

        lifeTime = 2f;
    }

    void Update()
    {
        if (lifeTime > 0)
        {
            if (m_canvasRect == null)
                return;

            var canvasRectTransform = m_canvasRect;
            var showPosition = m_damageTextData.position + new Vector3(0, 3, 0);
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
        }

        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
            gameObject.SetActive(false);
    }

    public void SetData(DamageTextManager.DamageTextData data, Transform trans)
    {
        m_damageTextData = data;
        m_canvasRect = trans.GetComponent<RectTransform>();
        m_text.text = data.damage.ToString();
    }
}