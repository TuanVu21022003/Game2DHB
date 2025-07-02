using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HeartItemUI : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 1f;

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void EffectDestroy()
    {
        if (image != null)
        {
            StartCoroutine(FadeOutAndDestroy());
        }
        else
        {
            OnDespawn();
        }
    }

    public bool IsActive()
    {
        return gameObject.activeSelf;
    }

    private void OnDespawn()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator FadeOutAndDestroy()
    {
        Color startColor = image.color;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startColor.a, 0f, elapsed / fadeDuration);

            image.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

            yield return null;
        }

        OnDespawn();
    }
}
