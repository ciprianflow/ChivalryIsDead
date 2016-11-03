using UnityEngine;
using System.Collections;
using System;

public interface EnemyActions
{
    void Taunted(Vector3 playerPosition);
    void Hit(int damage);

}


public class Enemy : MonoBehaviour, EnemyActions {

    public float speed = 25f;
    public int hp = 100;

    public void Taunted(Vector3 playerPosition)
    {
       
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, playerPosition, step);

    }

    public void Hit(int damage)
    {
        this.hp -= damage;
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


}
