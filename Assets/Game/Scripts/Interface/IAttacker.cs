using UnityEngine;

public interface IAttacker
{
    public Character Target { get; set; }
    public void Attack();
}
