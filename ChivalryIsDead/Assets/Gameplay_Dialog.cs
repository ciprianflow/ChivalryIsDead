using UnityEngine;
using System.Collections;

public class Gameplay_Dialog : MonoBehaviour
{
    public GameObject UI;

    GameObject player;
    GameObject[] enemies;
    GameObject enemySpeaker;
    public GameObject enemyBillboard;

    public GameObject playerTauntBubble;
    public GameObject enemyTauntBubble;

    public GameObject suicideTut;
    Animator suicideTutAnim;

    string princessMood;
    string swordMood;
    bool isnotAFK;

    public GameObject skipBtn;

    // Use this for initialization
    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        player = GameObject.FindGameObjectWithTag("Player");
        isnotAFK = false;

        if (PlayerPrefs.GetInt("SuicideLevel") == 1)
        {


            suicideTutAnim = suicideTut.GetComponent<Animator>();
            yield return new WaitForSeconds(6f);
            Time.timeScale = 0.1f;

            suicideTut.SetActive(true);
            skipBtn.SetActive(false);
            //suicideTutAnim.speed = 1f;
            swordAnimator.speed = 10f;
            swordBubbleAnimator.speed = 10f;
            suicideTutAnim.SetBool("playLearnSuicide", true);
            this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 11);

            PlayerPrefs.SetInt("SuicideTut", 1);
            PlayerPrefs.SetInt("SuicideLevel", 0);
        }

        if (SceneGetter.Instance.isDestroyQuest())
        {
            this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 16);

        }
        else if (SceneGetter.Instance.isWellQuest())
        {
            this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 15);
        }
        else if (SceneGetter.Instance.isBakeryQuest())
        {
            this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 14);
        }
        else if (SceneGetter.Instance.isFarmhouseQuest())
        {
            this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 13);
        }
    }

    // Update is called once per frame
    void Update()
    {

        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    StartCoroutine("PlayerTaunt");
        //    StartCoroutine("EnemyTaunted");
        //    StartCoroutine("EnemyFeedback");
        //}

        //if (Input.GetKeyDown(KeyCode.I))
        //{
        //    StartCoroutine("SwordFeedback");
        //}

        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    StartCoroutine("PrincessFeedback");
        //}
    }

    

    public void PrincessFeedback()
    {
        MoodUpdate();

        if (princessMood == "Crazy")
        {
            this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 0);
        }

    }

    public void SwordFeedback()
    {
        MoodUpdate();

        if (swordMood == "Happy")
        {
            this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 1);
        }

    }

    public void EnemyFeedback()
    {
        foreach(GameObject enemy in enemies)
        {
            enemySpeaker = enemies[0];
            if (Vector3.Distance(player.transform.position, enemy.transform.position) < Vector3.Distance(player.transform.position, enemySpeaker.transform.position))
            {
                enemySpeaker = enemy;
            } 

        }

        enemyBillboard.GetComponent<CameraBillboard>().speaker = enemySpeaker;
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 2);

    }

    public void MoodUpdate()
    {
        princessMood = "Crazy";
        swordMood = "Happy";


    }

    // Feedback

    public void HitThreeWrongTargets()
    {

    }

    public IEnumerator HalfTime()
    {
        yield return new WaitForSeconds(1f);
        if (SceneGetter.Instance.isDestroyQuest())
        {
            this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 20);

        }
        else if (SceneGetter.Instance.isWellQuest())
        {
            this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 19);
        }
        else if (SceneGetter.Instance.isBakeryQuest())
        {
            this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 18);
        }
        else if (SceneGetter.Instance.isFarmhouseQuest())
        {
            this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 17);
        }
        TimerObjectScript.isReminded = true;
    }

    public IEnumerator WakeUp()
    {
        Vector3 startingPos = player.transform.position;

        yield return new WaitForSeconds(1f);
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 4);
        skipBtn.SetActive(false);
        while (!isnotAFK)
        {
            if (Vector3.Distance(startingPos, player.transform.position) > 0.01f)
                isNotAFK();
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitUntil(HaveProceeded);
        gameObject.GetComponent<DialogObject>().StopDialog();
        isnotAFK = false;
    }

    bool HaveProceeded()
    {
        return isnotAFK;
    }

    void isNotAFK()
    {
        isnotAFK = true;
    }

    public IEnumerator WrongOverreact()
    {
        yield return new WaitForSeconds(1f);
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 5);
    }

    public IEnumerator YouHitSheep()
    {
        yield return new WaitForSeconds(1f);
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 3);
    }

    public IEnumerator StopAttacking()
    {
        yield return new WaitForSeconds(1f);
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 0);
    }

    public IEnumerator SpamingTaunt()
    {
        yield return new WaitForSeconds(1f);
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 8);
    }

    public IEnumerator LowCombo()
    {
        yield return new WaitForSeconds(1f);
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 10);
    }

    public IEnumerator NoSheepKilled()
    {
        yield return new WaitForSeconds(1f);
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 2);
    }

    public IEnumerator NoTaunting()
    {
        yield return new WaitForSeconds(1f);
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 6);
    }

    public IEnumerator NoOverreacting()
    {
        yield return new WaitForSeconds(1f);
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 7);
    }
    public IEnumerator NoGettingHit()
    {
        yield return new WaitForSeconds(1f);
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 9);
    }

    public IEnumerator TauntSuicide()
    {
        yield return new WaitForSeconds(1f);
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 12);
    }



    public IEnumerator PlayerTaunt()
    {
        playerTauntBubble.SetActive(true);
        yield return new WaitForSeconds(2f);
        playerTauntBubble.SetActive(false);
    }

    public IEnumerator EnemyTaunted()
    {
        enemyTauntBubble.SetActive(true);
        yield return new WaitForSeconds(2f);
        enemyTauntBubble.SetActive(false);
    }
}
