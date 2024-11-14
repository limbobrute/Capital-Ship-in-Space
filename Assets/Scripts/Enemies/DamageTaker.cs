using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTaker : MonoBehaviour
{
    public EnemyAI enemyAI;

    public void Damage(int damage)
    {
        enemyAI.TakeDamage(damage);
    }
}
