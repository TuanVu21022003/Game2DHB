using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject hitVFX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" || collision.tag == "Enemy")
        {
            collision.GetComponent<Character>().OnHit(28);
            
        }
    }
    
}
