using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image imageFill;
    [SerializeField] private Vector3 offset;

    float hp;
    float maxhp;
    Transform target;
    // Update is called once per frame
    void Update()
    {
        imageFill.fillAmount = Mathf.Lerp(imageFill.fillAmount, hp / maxhp, Time.deltaTime * 4f);
        transform.position = target.position + offset;
    }

    public void OnInit(float maxhp, Transform target, CharacterType characterType)
    {
        this.target = target;
        this.maxhp = maxhp;
        this.hp = maxhp;
        imageFill.fillAmount = 1;
        if(characterType == CharacterType.Enemy)
        {
            imageFill.color = Color.red;
        }
        else if (characterType == CharacterType.Player)
        {
            imageFill.color = Color.green;
        }
        else
        {
            imageFill.color = Color.white;
        }
    }

    public void SetNewHP(float hp)
    {
        this.hp = hp;
    }

}
