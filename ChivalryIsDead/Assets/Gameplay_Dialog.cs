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

    string princessMood;
    string swordMood;

    // Use this for initialization
    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine("PlayerTaunt");
            StartCoroutine("EnemyTaunted");
            StartCoroutine("EnemyFeedback");
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            StartCoroutine("SwordFeedback");
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine("PrincessFeedback");
        }
    }

    

    public IEnumerator PrincessFeedback()
    {
        MoodUpdate();

        if (princessMood == "Crazy")
        {
            this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 0);
            //WwiseInterface.Instance.PlayPrincessDialogue(PrincessDialogueHandle.Happy);
        }

        yield return null;
    }

    public IEnumerator SwordFeedback()
    {
        MoodUpdate();

        if (swordMood == "Happy")
        {
            this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 1);
            //WwiseInterface.Instance.PlaySwordDialogue(SwordDialogueHandle.HappyLong);
        }

        yield return null;
    }

    public IEnumerator EnemyFeedback()
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

        yield return null;
    }

    public void MoodUpdate()
    {
        princessMood = "Crazy";
        swordMood = "Happy";


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
