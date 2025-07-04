using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private float distanceDown = 3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OnInit();
    }

    public void OnInit()
    {

    }

    public void OnDown()
    {
        StartCoroutine(MoveCoroutine(distanceDown));
    }

    public void OnUp()
    {
        StartCoroutine(MoveCoroutine(-distanceDown));
    }

    private IEnumerator MoveCoroutine(float distance, float duration = 1)
    {
        Vector2 startPos = transform.position;
        Vector2 endPos = startPos + Vector2.down * distance;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            transform.position = Vector2.Lerp(startPos, endPos, t);
            yield return null;
        }

        transform.position = endPos; // Đảm bảo đến đúng vị trí cuối cùng
    }
}
