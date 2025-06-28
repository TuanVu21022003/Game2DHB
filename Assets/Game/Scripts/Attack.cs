using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Character character;
    private void Start()
    {
        if (character == null)
        {
            character = transform.parent.GetComponent<Character>();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Enemy")
        {
            collision.GetComponent<Character>().OnHit(character.Damage);
            
        }
    }
    
}
