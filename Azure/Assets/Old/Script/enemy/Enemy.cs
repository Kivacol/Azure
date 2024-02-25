using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int MaxLife = 3;
    private int currectLife;

    public EnemyPatrol enemyPatrol;

    void Start()
    {
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
        currectLife = MaxLife;

    }

    void Update()
    {
        
    }

    void Damage()
    {
        currectLife--;
        if(currectLife <= 0)
        {
            enemyPatrol.SendMessage("IsDie", 1);
            die();
        }
    }

    void die()
    {
        if(currectLife == 0)
        {
            GetComponent<Collider>().enabled = false;
            this.enabled = false;
        }
    }
}