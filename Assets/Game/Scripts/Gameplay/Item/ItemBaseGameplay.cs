using System.Collections;
using UnityEngine;

public class ItemBaseGameplay : MonoBehaviour
{
    [SerializeField] public ItemType type;
    [SerializeField] public int amount;
    [SerializeField] public float duration;

    private Transform target;
    private Coroutine effectCoroutine;

    public virtual void OnInit(Transform target, int amount)
    {
        this.target = target;
        this.amount = amount;
        float timeDelay = 1.5f;
        BounceItem(timeDelay); // Bắt đầu với hiệu ứng nảy
        Invoke(nameof(StartMove), timeDelay); // Sau khi nảy xong, bắt đầu di chuyển
    }

    public void OnDespawn()
    {
        if (effectCoroutine != null)
        {
            StopCoroutine(effectCoroutine);
        }
        Destroy(gameObject);
    }

    private void StartMove()
    {
        if (target == null)
        {
            Debug.LogWarning("Target is not set for the item to move to.");
            return;
        }
        if (effectCoroutine != null)
        {
            StopCoroutine(effectCoroutine);
        }
        effectCoroutine = StartCoroutine(MoveItem(target, duration));
    }

    private void BounceItem(float time)
    {
        if (effectCoroutine != null)
        {
            StopCoroutine(effectCoroutine);
        }
        effectCoroutine = StartCoroutine(DampedVerticalOscillation(1f, 2f, 1f, time));
    }

    private IEnumerator MoveItem(Transform target, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            // Tăng tốc dần (ease-in)
            float easedT = Mathf.Pow(t, 2f);

            // Lấy lại vị trí đích mỗi frame
            Vector3 currentTargetPos = target.position;

            // Di chuyển theo tỉ lệ easedT giữa vị trí hiện tại và vị trí đích hiện tại
            transform.position = Vector3.Lerp(transform.position, currentTargetPos, easedT);

            yield return null;
        }

        // Đảm bảo cập nhật cuối cùng
        transform.position = target.position;
    }

    private IEnumerator DampedVerticalOscillation(float amplitude, float frequency, float damping, float duration)
    {
        Vector3 startPos = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            // Tính biên độ giảm dần theo thời gian: A * e^(-damping * t)
            float dampedAmplitude = amplitude * Mathf.Exp(-damping * elapsed);

            // Dao động sin: sin(2πft)
            float offsetY = dampedAmplitude * Mathf.Sin(2 * Mathf.PI * frequency * elapsed);

            // Cập nhật vị trí
            transform.position = new Vector3(startPos.x, startPos.y + offsetY, startPos.z);

            yield return null;
        }

        // Kết thúc: đặt lại vị trí chính xác
        transform.position = startPos;
    }
}
