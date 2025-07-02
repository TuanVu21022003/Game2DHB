using UnityEngine;

public static class HelperUtils
{
    public static Vector2 GetRandomPosition2D(Vector2 origin, float radius)
    {
        Vector2 randomOffset = Random.insideUnitCircle * radius;
        return origin + randomOffset;
    }
}
