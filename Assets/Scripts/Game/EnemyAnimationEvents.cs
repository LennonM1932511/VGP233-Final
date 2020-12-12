using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{
    [SerializeField]
    float damage = 10.0f;
    Player player;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    public void DealDamageToPlayer()
    {
        player.TakeDamage(damage);
    }

}
