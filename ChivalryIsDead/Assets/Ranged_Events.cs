using UnityEngine;
using System.Collections;

public class Ranged_Events : MonoBehaviour {

    public GameObject hand;
    public GameObject rock_prefab;
    GameObject rock;
    public RangedAI rangedScript;
    public GameObject dustParticles;
    // Use this for initialization
    string[] sounds = new string[3] { "ranged_find_stone" , "ranged_look_for_stone" , "ranged_pickup_stone" };

    void Start () {
        dustParticles.GetComponent<ParticleSystem>().Stop();
    }

    // Update is called once per frame
    void Update () {
	
	}

    public void spawnRock() {
        rock = Instantiate(rock_prefab);
        ////Vector3 handPos = hand.transform.localPosition;

        rock.transform.parent = hand.transform;
        rock.transform.localPosition = new Vector3(0.3f, -0.01f, 0);
        //rock.transform.position = hand.transform.position + new Vector3(50f,50f, 0);

        rock.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    public void throwRock() {


        rock.GetComponent<Rigidbody>().useGravity = true;
        rock.GetComponent<Rigidbody>().isKinematic = false;
        rock.GetComponent<BoxCollider>().enabled = true;
        rangedScript.FireProjectTile(ref rock);

    }

    public void dropRock() {
        if (rock == null)
            return;

        if (rock.transform.parent == hand.transform) {
            rock.GetComponent<Rigidbody>().useGravity = true;
            rock.GetComponent<BoxCollider>().enabled = true;
            rock.GetComponent<Rigidbody>().isKinematic = false;
            rock.GetComponent<Projectile>().originMonster = this.transform.parent.GetComponent<MonsterAI>();
            rock.transform.parent = null;
            Debug.Log("JFWIAO");
        }

    }
    public void setDust(int state) {
        if (state == 1)
            dustParticles.GetComponent<ParticleSystem>().Play();
        //dustParticles.SetActive(true);

        else
            dustParticles.GetComponent<ParticleSystem>().Stop();
    }

    public void callSound(int sound) {
        AkSoundEngine.PostEvent(sounds[sound], gameObject);
    }
}
