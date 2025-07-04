using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public static class HelperUtils
{
    public static Vector2 GetRandomPosition2D(Vector2 origin, float radius)
    {
        Vector2 randomOffset = Random.insideUnitCircle * radius;
        return origin + randomOffset;
    }

    public static IEnumerator DelayCoroutine(float delay, Action callback)
    {
        yield return new WaitForSeconds(delay);
        callback?.Invoke();
    }

    public static void DrawCircleLine(Vector3 center, float radius, int segments, Color color, float duration)
    {
        float angleStep = 360f / segments;
        Vector3 prevPoint = center + new Vector3(radius, 0, 0); // Bắt đầu từ trục X dương

        for (int i = 1; i <= segments; i++)
        {
            float angle = angleStep * i;
            float rad = angle * Mathf.Deg2Rad;

            Vector3 nextPoint = center + new Vector3(Mathf.Cos(rad) * radius, Mathf.Sin(rad) * radius, 0);
            Debug.DrawLine(prevPoint, nextPoint, color, duration);
            prevPoint = nextPoint;
        }
    }
}
