using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DialogObject : MonoBehaviour {

    public GameObject UI;

    public DialogInfo[] dialog;

    GameObject[] container;

    public GameObject playerText;
    public GameObject playerBubble;

    public GameObject swordText;
    public GameObject swordBubble;

    public GameObject princessText;
    public GameObject princessBubble;

    GameObject kingText;
    GameObject kingBubble;

    GameObject[] enemyText;
    GameObject[] enemyBubble;

    public GameObject peasantABubble;
    public GameObject peasantAText;

    public GameObject peasantBBubble;
    public GameObject peasantBText;

    // Use this for initialization
    void Start () {



        //peasantA = GameObject.FindGameObjectWithTag("PeasantA");
        //peasantB = GameObject.FindGameObjectWithTag("PeasantB");

        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            if (GameObject.FindGameObjectWithTag("PlayerBubble"))
            {
                playerText = GameObject.FindGameObjectWithTag("PlayerText");
                playerBubble = GameObject.FindGameObjectWithTag("PlayerBubble");
                playerBubble.SetActive(false);
            }
        }

        if (GameObject.FindGameObjectWithTag("Enemy") != null)
        {

            if (GameObject.FindGameObjectWithTag("EnemyBubble"))
            {
                enemyText = GameObject.FindGameObjectsWithTag("EnemyText");
                enemyBubble = GameObject.FindGameObjectsWithTag("EnemyBubble");

                foreach (GameObject arrayEnemyBubble in enemyBubble)
                {
                    arrayEnemyBubble.SetActive(false);
                }

            }
        }


        //if (GameObject.FindGameObjectWithTag("SwordBubble") != null)
        //{
        //    sword = GameObject.FindGameObjectWithTag("SwordText");
        //    swordBubble = GameObject.FindGameObjectWithTag("SwordBubble");
        //    swordBubble.SetActive(false);
        //    //UI.GetComponent<GameMenu>().sword.SetActive(false);
        //}
        
        //if (GameObject.FindGameObjectWithTag("PrincessBubble") != null)
        //{
        //    princess = GameObject.FindGameObjectWithTag("PrincessText");
        //    princessBubble = GameObject.FindGameObjectWithTag("PrincessBubble");
        //    princessBubble.SetActive(false);
        //    //UI.GetComponent<GameMenu>().princess.SetActive(false);
        //}

        //if (GameObject.FindGameObjectWithTag("KingBubble") != null)
        //{
        //    king = GameObject.FindGameObjectWithTag("KingText");
        //    kingBubble = GameObject.FindGameObjectWithTag("KingBubble");
        //    kingBubble.SetActive(false);
        //}


        //if (peasantA != null)
        //{
        //    if (GameObject.FindGameObjectWithTag("PeasantABubble"))
        //    {
        //        peasantAText = GameObject.FindGameObjectWithTag("PeasantAText");
        //        peasantABubble = GameObject.FindGameObjectWithTag("PeasantABubble");
        //        peasantABubble.SetActive(false);
        //    }
        //}
        //if (peasantB != null)
        //{
        //    if (GameObject.FindGameObjectWithTag("PeasantBBubble"))
        //    {
        //        peasantBText = GameObject.FindGameObjectWithTag("PeasantBText");
        //        peasantBBubble = GameObject.FindGameObjectWithTag("PeasantBBubble");
        //        peasantBBubble.SetActive(false);
        //    }
        //}



    }
	
	// Update is called once per frame
	void Update () {


        if (Input.GetKeyDown(KeyCode.Return))
        {
            StopDialog();
        }

    }




    IEnumerator DialogSystem(int v)
    {

        DialogInfo d = dialog[v];

        d.Dialog = d.Name.Length;

        for(int i = 0; i < d.Dialog; i++)
        {
           

            if (d.Name[i] == "Player")
            {
                playerBubble.SetActive(true);
                playerText.GetComponent<Text>().text = d.Text[i];
                yield return new WaitForSeconds(d.Wait[i]);
                playerBubble.SetActive(false);
            }


            if (GameObject.FindGameObjectWithTag("Enemy") != null)
            {

                if (GameObject.FindGameObjectWithTag("EnemyBubble"))
                {
                    enemyText = GameObject.FindGameObjectsWithTag("EnemyText");
                    enemyBubble = GameObject.FindGameObjectsWithTag("EnemyBubble");

                    foreach (GameObject arrayEnemyBubble in enemyBubble)
                    {
                        arrayEnemyBubble.SetActive(false);
                    }

                }
            }


            if (d.Name[i] == "Enemy")
            {
                for (int j = 0; j < enemyText.Length; ++j)
                {
                    enemyBubble[j].SetActive(true);
                    enemyText[j].GetComponent<Text>().text = d.Text[i];

                }

                yield return new WaitForSeconds(d.Wait[i]);


                for (int j = 0; j < enemyText.Length; ++j)
                {
                    enemyBubble[j].SetActive(false);

                }
            }

            if (d.Name[i] == "PeasantA")
            {
                peasantABubble.SetActive(true);
                peasantAText.GetComponent<Text>().text = d.Text[i];
                yield return new WaitForSeconds(d.Wait[i]);
                peasantABubble.SetActive(false);
            }
            if (d.Name[i] == "PeasantB")
            {
                peasantBBubble.SetActive(true);
                peasantBText.GetComponent<Text>().text = d.Text[i];
                yield return new WaitForSeconds(d.Wait[i]);
                peasantBBubble.SetActive(false);
            }


 
                
            if (d.Name[i] == "Sword")
            {
                swordBubble.SetActive(true);
                swordText.GetComponent<Text>().text = d.Text[i];
                yield return new WaitForSeconds(d.Wait[i]);
                swordBubble.SetActive(false);
            }

            if (d.Name[i] == "Princess")
            {
                princessBubble.SetActive(true);
                princessText.GetComponent<Text>().text = d.Text[i];
                yield return new WaitForSeconds(d.Wait[i]);
                princessBubble.SetActive(false);
            }

            if (d.Name[i] == "King")
            {
                kingBubble.SetActive(true);
                kingText.GetComponent<Text>().text = d.Text[i];
                yield return new WaitForSeconds(d.Wait[i]);
                kingBubble.SetActive(false);
            }
                

        }

        
        UI.GetComponent<GameMenu>().sword.GetComponent<Animator>().SetTrigger("Hide");
        UI.GetComponent<GameMenu>().princess.GetComponent<Animator>().SetTrigger("Hide");
        StartCoroutine(Hide());
    }

    IEnumerator Hide()
    {
        // Remememememember :)
        yield return new WaitForSeconds(4f);
        UI.GetComponent<GameMenu>().sword.SetActive(false);
        UI.GetComponent<GameMenu>().princess.SetActive(false);
    }

    public void StopDialog()
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
        if (GameObject.FindGameObjectWithTag("PeasantABubble"))
        {
            peasantABubble.SetActive(false);
        }
        if (GameObject.FindGameObjectWithTag("PeasantBBubble"))
        {
            peasantBBubble.SetActive(false);
        }

    }


}
