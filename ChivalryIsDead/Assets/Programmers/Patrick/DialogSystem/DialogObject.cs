using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DialogObject : MonoBehaviour {

    public DialogInfo dialog;

    GameObject[] container;

    GameObject player;
    GameObject playerBubble;

    GameObject sword;
    GameObject swordBubble;

    GameObject princess;
    GameObject princessBubble;

    GameObject king;
    GameObject kingBubble;

    GameObject[] enemy;
    GameObject[] enemyBubble;

    // Use this for initialization
    void Start () {

        if(GameObject.FindGameObjectWithTag("Player"))
        {
            player = GameObject.FindGameObjectWithTag("PlayerText");
            playerBubble = GameObject.FindGameObjectWithTag("PlayerBubble");
            playerBubble.SetActive(false);
        }
        
        if(GameObject.FindGameObjectWithTag("Sword"))
        {
            sword = GameObject.FindGameObjectWithTag("SwordText");
            swordBubble = GameObject.FindGameObjectWithTag("SwordBubble");
            swordBubble.SetActive(false);
        }
        
        if(GameObject.FindGameObjectWithTag("Princess"))
        {
            princess = GameObject.FindGameObjectWithTag("PrincessText");
            princessBubble = GameObject.FindGameObjectWithTag("PrincessBubble");
            princessBubble.SetActive(false);
        }

        if (GameObject.FindGameObjectWithTag("King"))
        {
            king = GameObject.FindGameObjectWithTag("KingText");
            kingBubble = GameObject.FindGameObjectWithTag("KingBubble");
            kingBubble.SetActive(false);
        }

        if(GameObject.FindGameObjectWithTag("Enemy"))
        {
            enemy = GameObject.FindGameObjectsWithTag("EnemyText");
            enemyBubble = GameObject.FindGameObjectsWithTag("EnemyBubble");

            foreach (GameObject arrayEnemyBubble in enemyBubble)
            {
                arrayEnemyBubble.SetActive(false);
            }

        }
       


    }
	
	// Update is called once per frame
	void Update () {
	
        if(Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine("DialogSystem", dialog);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            StopCoroutine("DialogSystem");
            playerBubble.SetActive(false);
            swordBubble.SetActive(false);
            princessBubble.SetActive(false);
            kingBubble.SetActive(false);

            foreach(GameObject arrayEnemyBubble in enemyBubble)
            {
                arrayEnemyBubble.SetActive(false);
            }
            
        }

    }

    IEnumerator DialogSystem(DialogInfo v)
    {
        v.Dialog = v.Name.Length;

        for(int i = 0; i < v.Dialog; i++)
        {
            if(v.Name[i] == "Player")
            {
                Debug.Log(v.Wait[i]);
                playerBubble.SetActive(true);
                player.GetComponent<Text>().text = "Player: " + v.Text[i];
                yield return new WaitForSeconds(v.Wait[i]);
                playerBubble.SetActive(false);
            }

            if (v.Name[i] == "Sword")
            {
                swordBubble.SetActive(true);
                sword.GetComponent<Text>().text = "Sword: " + v.Text[i];
                yield return new WaitForSeconds(v.Wait[i]);
                swordBubble.SetActive(false);
            }

            if (v.Name[i] == "Princess")
            {
                princessBubble.SetActive(true);
                princess.GetComponent<Text>().text = "Princess: " + v.Text[i];
                yield return new WaitForSeconds(v.Wait[i]);
                princessBubble.SetActive(false);
            }

            if (v.Name[i] == "King")
            {
                kingBubble.SetActive(true);
                king.GetComponent<Text>().text = "King: " + v.Text[i];
                yield return new WaitForSeconds(v.Wait[i]);
                kingBubble.SetActive(false);
            }

            if (v.Name[i] == "Enemy")
            {
                for(int j = 0; j < enemy.Length; ++j)
                {
                    enemyBubble[j].SetActive(true);
                    enemy[j].GetComponent<Text>().text = "Enemy: " + v.Text[i];
                    
                }

                yield return new WaitForSeconds(v.Wait[i]);
                

                for (int j = 0; j < enemy.Length; ++j)
                {
                    enemyBubble[j].SetActive(false);

                }

            }

 

            
            
            

        }

        

    }
}
