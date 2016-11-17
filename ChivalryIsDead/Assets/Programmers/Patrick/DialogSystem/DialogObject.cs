using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DialogObject : MonoBehaviour {

    public GameObject UI;
    GameMenu gameMenu;

    public DialogInfo[] dialog;

    GameObject[] container;

    public GameObject playerText;
    public GameObject playerBubble;

    public GameObject swordText;
    public GameObject swordBubble;

    public GameObject princessText;
    public GameObject princessBubble;

    public GameObject kingText;
    public GameObject kingBubble;

    GameObject[] enemyText;
    GameObject[] enemyBubble;

    public GameObject peasantABubble;
    public GameObject peasantAText;

    public GameObject peasantBBubble;
    public GameObject peasantBText;

    GameObject targetBubble;
    int indexCount;

    float test;
    bool isSkipped;
    float SpeakingTime;

    // Use this for initialization
    void Start () {
        indexCount = 0;
        isSkipped = false;
        gameMenu = UI.GetComponent<GameMenu>();
        //peasantA = GameObject.FindGameObjectWithTag("PeasantA");
        //peasantB = GameObject.FindGameObjectWithTag("PeasantB");

        //if (GameObject.FindGameObjectWithTag("Player") != null)
        //{
        //    if (GameObject.FindGameObjectWithTag("PlayerBubble"))
        //    {
        //        playerText = GameObject.FindGameObjectWithTag("PlayerText");
        //        playerBubble = GameObject.FindGameObjectWithTag("PlayerBubble");
        //        playerBubble.SetActive(false);
        //    }
        //}

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


   

    public IEnumerator DialogSystem(int v)
    {
        DialogInfo d = dialog[v];

        d.Dialog = d.Name.Length;

        for (int i = 0; i < d.Dialog; i++)
        {
            if (d.Name[i] == "Player")
            {
                indexCount = i;
                playerBubble.SetActive(true);
                playerText.GetComponent<Text>().text = d.Text[i];
                //yield return new WaitForSeconds(d.Wait[i]);
                SpeakingTime = d.Wait[i];

                while (!isSkipped && SpeakingTime > 0)
                {
                    SpeakingTime -= Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }


                isSkipped = false;
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


            if (d.Name[i] == "Enemy" && enemyBubble != null && enemyText != null)
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
                indexCount = i;
                swordBubble.SetActive(true);
                swordText.GetComponent<Text>().text = d.Text[i];
                //yield return new WaitForSeconds(d.Wait[i]);
                SpeakingTime = d.Wait[i];

                while (!isSkipped && SpeakingTime > 0)
                {
                    SpeakingTime -= Time.deltaTime;
                    yield return new WaitForEndOfFrame();

                }
                isSkipped = false;
                //test = d.Wait[i];
                //yield return StartCoroutine("SkipTest");
                swordBubble.SetActive(false);
            }

            if (d.Name[i] == "Princess")
            {
                indexCount = i;
                princessBubble.SetActive(true);
                princessText.GetComponent<Text>().text = d.Text[i];
                ////yield return new WaitForSeconds(d.Wait[i]);
                SpeakingTime = d.Wait[i];

                while (!isSkipped && SpeakingTime > 0)
                {
                    SpeakingTime -= Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }
                    

                isSkipped = false;
                //test = d.Wait[i];
                //yield return StartCoroutine("SkipTest");
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
        swordText.GetComponent<Text>().text = "";
        princessText.GetComponent<Text>().text = "";
        kingText.GetComponent<Text>().text = "";

        gameMenu.skipAllBtn.SetActive(false);
        gameMenu.skipBtn.SetActive(false);
        gameMenu.sword.GetComponent<Animator>().SetTrigger("Outro");
        gameMenu.princess.GetComponent<Animator>().SetTrigger("Outro");
        StartCoroutine(Hide());
    }

    IEnumerator Hide()
    {
        // Remember :)
        yield return new WaitForSeconds(2f);
        gameMenu.sword.SetActive(false);
        gameMenu.princess.SetActive(false);
        gameMenu.skipBtn.SetActive(false);
        gameMenu.speaking = false;
    }

    IEnumerator SkipTest()
    {
        yield return new WaitForSeconds(1);
        isSkipped = true;
    }

    public void SkipDialog()
    {
        //StopCoroutine("SkipTest");
        //test = 0.5f;
        //StartCoroutine("SkipTest");

        swordBubble.SetActive(false);
        princessBubble.SetActive(false);
        playerBubble.SetActive(false);
        isSkipped = true;
        //StopAllCoroutines();
        //StartCoroutine(DialogSystem(0, indexCount+1));
        //DialogSystem(0).MoveNext();
    }

    public void StopDialog()
    {
        StopCoroutine("DialogSystem");
        gameMenu.sword.GetComponent<Animator>().SetTrigger("Outro");
        gameMenu.princess.GetComponent<Animator>().SetTrigger("Outro");
        StartCoroutine(Hide());

        
        playerBubble.SetActive(false);
        swordBubble.SetActive(false);
        princessBubble.SetActive(false);
        kingBubble.SetActive(false);
        peasantABubble.SetActive(false);
        peasantBBubble.SetActive(false);


        if (GameObject.FindGameObjectWithTag("EnemyBubble") && GameObject.FindGameObjectWithTag("Enemy") != null)
        {
            foreach (GameObject arrayEnemyBubble in enemyBubble)
            {
                arrayEnemyBubble.SetActive(false);
            }
        }
        
        
        

    }


}
