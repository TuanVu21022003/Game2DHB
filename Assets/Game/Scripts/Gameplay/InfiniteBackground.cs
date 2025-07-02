using UnityEngine;

public class InfiniteBackground : MonoBehaviour
{
    public Transform[] backgrounds; // gán 2 nền trong inspector
    public float backgroundWidth;   // chiều rộng sprite nền

    void Update()
    {
        foreach (Transform bg in backgrounds)
        {
            // Nếu nền đã đi ra khỏi camera phía bên trái
            if (bg.position.x + backgroundWidth < Camera.main.transform.position.x)
            {
                // Di chuyển nó về phía phải sau nền còn lại
                float rightMostX = FindRightMostBackgroundX();
                bg.position = new Vector3(rightMostX + backgroundWidth, bg.position.y, bg.position.z);
            }
        }
    }

    float FindRightMostBackgroundX()
    {
        float maxX = float.MinValue;
        foreach (Transform bg in backgrounds)
        {
            if (bg.position.x > maxX)
                maxX = bg.position.x;
        }
        return maxX;
    }
}
