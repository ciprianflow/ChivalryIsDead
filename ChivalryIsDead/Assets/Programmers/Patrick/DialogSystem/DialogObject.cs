using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DialogObject : MonoBehaviour {

    public DialogInfo[] dialog;

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


        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            if (GameObject.FindGameObjectWithTag("PlayerBubble"))
            {
                player = GameObject.FindGameObjectWithTag("PlayerText");
                playerBubble = GameObject.FindGameObjectWithTag("PlayerBubble");
                playerBubble.SetActive(false);
            }
        }

        if (GameObject.FindGameObjectWithTag("Enemy") != null)
        {

            if (GameObject.FindGameObjectWithTag("EnemyBubble"))
            {
                enemy = GameObject.FindGameObjectsWithTag("EnemyText");
                enemyBubble = GameObject.FindGameObjectsWithTag("EnemyBubble");

                foreach (GameObject arrayEnemyBubble in enemyBubble)
                {
                    arrayEnemyBubble.SetActive(false);
                }

            }
        }

      



    }
	
	// Update is called once per frame
	void Update () {



        if (Input.GetKeyDown(KeyCode.T))
        {
            //StartCoroutine("DialogSystem", 0);
            //DialogUpdate();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            StopCoroutine("DialogSystem");

            if (GameObject.FindGameObjectWithTag("PlayerBubble"))
            {
                playerBubble.SetActive(false);
            }

            if (GameObject.FindGameObjectWithTag("SwordBubble"))
            {
                swordBubble.SetActive(false);
            }

            if (GameObject.FindGameObjectWithTag("PrincessBubble"))
            {
                princessBubble.SetActive(false);
            }

            if (GameObject.FindGameObjectWithTag("KingBubble"))
            {
                kingBubble.SetActive(false);
            }


            if (GameObject.FindGameObjectWithTag("EnemyBubble"))
            {
                foreach (GameObject arrayEnemyBubble in enemyBubble)
                {
                    arrayEnemyBubble.SetActive(false);
                }
            }
            


        }

    }




    IEnumerator DialogSystem(int v)
    {

        DialogInfo d = dialog[v];

        d.Dialog = d.Name.Length;

        for(int i = 0; i < d.Dialog; i++)
        {
            if (GameObject.FindGameObjectWithTag("Player") != null)
            {
                if (GameObject.FindGameObjectWithTag("PlayerBubble"))
                {
                    player = GameObject.FindGameObjectWithTag("PlayerText");
                    playerBubble = GameObject.FindGameObjectWithTag("PlayerBubble");
                    playerBubble.SetActive(false);
                }
            }
            
            if(GameObject.FindGameObjectWithTag("Enemy") != null) { 

                if (GameObject.FindGameObjectWithTag("EnemyBubble"))
                {
                    enemy = GameObject.FindGameObjectsWithTag("EnemyText");
                    enemyBubble = GameObject.FindGameObjectsWithTag("EnemyBubble");

                    foreach (GameObject arrayEnemyBubble in enemyBubble)
                    {
                        arrayEnemyBubble.SetActive(false);
                    }

                }

            }
           


            if (d.Name[i] == "Player")
            {
                playerBubble.SetActive(true);
                player.GetComponent<Text>().text = d.Text[i];
                yield return new WaitForSeconds(d.Wait[i]);
                playerBubble.SetActive(false);
            }

            if (d.Name[i] == "Enemy")
            {
                for (int j = 0; j < enemy.Length; ++j)
                {
                    enemyBubble[j].SetActive(true);
                    enemy[j].GetComponent<Text>().text = d.Text[i];

                }

                yield return new WaitForSeconds(d.Wait[i]);


                for (int j = 0; j < enemy.Length; ++j)
                {
                    enemyBubble[j].SetActive(false);

                }
            }

           


            if (GameObject.FindGameObjectWithTag("GameUI") != null)
            {
                if (GameObject.FindGameObjectWithTag("SwordBubble"))
                {
                    sword = GameObject.FindGameObjectWithTag("SwordText");
                    swordBubble = GameObject.FindGameObjectWithTag("SwordBubble");
                    swordBubble.SetActive(false);
                }

                if (GameObject.FindGameObjectWithTag("PrincessBubble"))
                {
                    princess = GameObject.FindGameObjectWithTag("PrincessText");
                    princessBubble = GameObject.FindGameObjectWithTag("PrincessBubble");
                    princessBubble.SetActive(false);
                }

                if (GameObject.FindGameObjectWithTag("KingBubble"))
                {
                    king = GameObject.FindGameObjectWithTag("KingText");
                    kingBubble = GameObject.FindGameObjectWithTag("KingBubble");
                    kingBubble.SetActive(false);
                }
            } 
                
            if (d.Name[i] == "Sword")
            {
                swordBubble.SetActive(true);
                sword.GetComponent<Text>().text = d.Text[i];
                yield return new WaitForSeconds(d.Wait[i]);
                swordBubble.SetActive(false);
            }

            if (d.Name[i] == "Princess")
            {
                princessBubble.SetActive(true);
                princess.GetComponent<Text>().text = d.Text[i];
                yield return new WaitForSeconds(d.Wait[i]);
                princessBubble.SetActive(false);
            }

            if (d.Name[i] == "King")
            {
                kingBubble.SetActive(true);
                king.GetComponent<Text>().text = d.Text[i];
                yield return new WaitForSeconds(d.Wait[i]);
                kingBubble.SetActive(false);
            }
                



            

 

            
            
            

        }

        

    }
}
