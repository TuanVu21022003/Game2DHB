using UnityEngine;

public class EffectBase : MonoBehaviour
{
    private void Start()
    {
        OnInit();
    }

    public void OnInit()
    {
        StartCoroutine(HelperUtils.DelayCoroutine(1f, OnDespawn));
    }

    private void OnDespawn()
    {
        Destroy(gameObject);
    }
}
