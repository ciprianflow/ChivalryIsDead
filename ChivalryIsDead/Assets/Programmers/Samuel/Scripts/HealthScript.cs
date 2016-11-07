using UnityEngine;
using System.Collections;

public class HealthScript {

    int health;
    int maxHealth;

    public HealthScript(int maxHealth)
    {
        this.maxHealth = maxHealth;
        health = maxHealth;
    }

    public int getHealth()
    {
        return health;
    }

    public int getMaxhealth()
    {
        return maxHealth;
    }

    public bool takeDamage(int dmg)
    {
        health -= dmg;
        if(health <= 0)
        {
            return true;
        }
        return false;
    }
}
