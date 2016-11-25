using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DialogObject : MonoBehaviour {

    public GameObject UI;
    GameMenu gameMenu;

    public DialogInfo[] dialogEnglish;
    public DialogInfo[] dialogDansk;

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

    public GameObject SwordParticle;
    public GameObject skipBtn;

    float test;
    float duration;
    bool isSkipped;
    float SpeakingTime;
    bool isSpeaking;
    bool callBlink;

    // Use this for initialization
    void Start () {
        indexCount = 0;
        isSkipped = false;
        isSpeaking = false;
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
        
        DialogInfo d_English = dialogEnglish[v];
        DialogInfo d_Dansk = dialogDansk[v];

        DialogInfo d = d_English;

        if (PlayerPrefs.GetString("Language") == "English")
        {
            d = d_English;
        }
        else if (PlayerPrefs.GetString("Language") == "Dansk")
        {
            d = d_Dansk;
        }

        d.Dialog = d.Name.Length;

        isSpeaking = true;
        callBlink = true;
        skipBtn.SetActive(true);

        for (int i = 0; i < d.Dialog; i++)
        {
            if (d.Name[i] == "Player")
            {
                indexCount = i;
                playerBubble.SetActive(true);
                //skipBtn.SetActive(true);
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
                skipBtn.SetActive(false);
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
                    //skipBtn.SetActive(true);
                    enemyText[j].GetComponent<Text>().text = d.Text[i];
                    WwiseInterface.Instance.PlayGeneralMonsterSound(MonsterHandle.Ranged, MonsterAudioHandle.Aggro, this.gameObject);
                }

                //yield return new WaitForSeconds(d.Wait[i]);
                SpeakingTime = d.Wait[i];

                while (!isSkipped && SpeakingTime > 0)
                {
                    SpeakingTime -= Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }
                isSkipped = false;


                for (int j = 0; j < enemyText.Length; ++j)
                {
                    enemyBubble[j].SetActive(false);
                    skipBtn.SetActive(false);

                }
            }

            if (d.Name[i] == "PeasantA")
            {
                peasantABubble.SetActive(true);
                //skipBtn.SetActive(true);
                peasantAText.GetComponent<Text>().text = d.Text[i];
                yield return new WaitForSeconds(d.Wait[i]);
                peasantABubble.SetActive(false);
                skipBtn.SetActive(false);
            }
            if (d.Name[i] == "PeasantB")
            {
                peasantBBubble.SetActive(true);
                skipBtn.SetActive(true);
                peasantBText.GetComponent<Text>().text = d.Text[i];
                yield return new WaitForSeconds(d.Wait[i]);
                peasantBBubble.SetActive(false);
                skipBtn.SetActive(false);
            }


 
                
            if (d.Name[i] == "Sword")
            {
                UI.GetComponent<GameMenu>().Sword();

                // Do we need this?
                /*
                if (callBlink)
                {
                    StartCoroutine(SwordBlink());
                    if (Time.timeScale == 1)
                        yield return new WaitForSeconds(2f);
                    else
                        yield return new WaitForSeconds(0.2f);
                    callBlink = false;
                }
                */

                SwordParticle.SetActive(true);
                indexCount = i;
                swordBubble.SetActive(true);
                //skipBtn.SetActive(true);

                swordText.GetComponent<Text>().text = d.Text[i];
                WwiseInterface.Instance.PlayUISound(UIHandle.DialogueSpeechBubblePop);
                WwiseInterface.Instance.PlaySwordDialogue(StaticData.GetSwordMood(d.Sound[i]));


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
                SwordParticle.SetActive(false);
                swordBubble.SetActive(false);
                skipBtn.SetActive(false);
                if (i + 1 >= d.Dialog || d.Name[i + 1] != "Sword")
                {
                    gameMenu.sword.GetComponent<Animator>().SetTrigger("Outro");
                    duration = gameMenu.sword.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
                    yield return new WaitForSeconds(duration);
                    //StartCoroutine(Hide(duration));
                    gameMenu.sword.SetActive(false);
                }
            }

            if (d.Name[i] == "Princess")
            {
                gameMenu.Princess();
                indexCount = i;
                princessBubble.SetActive(true);
                //skipBtn.SetActive(true);
                princessText.GetComponent<Text>().text = d.Text[i];
                WwiseInterface.Instance.PlayUISound(UIHandle.DialogueSpeechBubblePop);
                WwiseInterface.Instance.PlayPrincessDialogue(StaticData.GetPrincessMood(d.Sound[i]));


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
                skipBtn.SetActive(false);
                if (i + 1 >= d.Dialog || d.Name[i+1] != "Princess")
                {
                    gameMenu.princess.GetComponent<Animator>().SetTrigger("Outro");
                    duration = gameMenu.princess.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
                    yield return new WaitForSeconds(duration);
                    //StartCoroutine(Hide(duration));
                    gameMenu.princess.SetActive(false);
                }
            }

            if (d.Name[i] == "King")
            {
                kingBubble.SetActive(true);
                //skipBtn.SetActive(true);
                kingText.GetComponent<Text>().text = d.Text[i];
                yield return new WaitForSeconds(d.Wait[i]);
                kingBubble.SetActive(false);
                skipBtn.SetActive(false);
            }

           
        }
        swordText.GetComponent<Text>().text = "";
        princessText.GetComponent<Text>().text = "";
        kingText.GetComponent<Text>().text = "";

        isSpeaking = false;
        skipBtn.SetActive(false);
        //gameMenu.sword.GetComponent<Animator>().SetTrigger("Outro");
        //gameMenu.princess.GetComponent<Animator>().SetTrigger("Outro");
        //duration = gameMenu.sword.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        gameMenu.speaking = false;

        //StartCoroutine(Hide(duration));
    }

    IEnumerator Hide(float dur)
    {
        SpeakingTime = dur;
        while (isSpeaking && SpeakingTime > 0)
        {
            SpeakingTime -= Time.deltaTime;
            yield return new WaitForEndOfFrame();

        }
        // Remember :)
        //yield return new WaitForSeconds(dur);

  
        gameMenu.sword.SetActive(false);
        gameMenu.princess.SetActive(false);
        skipBtn.SetActive(false);
            
        

      

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
        StartCoroutine(Hide(gameMenu.sword.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length));

        
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

    IEnumerator SwordBlink()
    {
        ////if(Time.timeScale)
        //while (time > 0)
        //{
        //    SwordParticle.SetActive(true);
        //    yield return new WaitForSeconds(Random.Range(0.06f, 0.16f));
        //    SwordParticle.SetActive(false);
        //    //yield return new WaitForSeconds(Random.Range(0.01f, 0.03f));
        //    yield return new WaitForEndOfFrame();
        //    yield return new WaitForEndOfFrame();
        //    yield return new WaitForEndOfFrame();

        //    time -= Time.deltaTime;
        //}

        SwordParticle.SetActive(true);
        yield return new WaitForSeconds(2f);
        SwordParticle.transform.GetChild(1).gameObject.SetActive(false);
    }
}
