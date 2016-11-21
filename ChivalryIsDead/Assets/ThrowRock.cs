using UnityEngine;
using System.Collections;

public class ThrowRock : MonoBehaviour {

    public GameObject hand;
    public GameObject rock_prefab;
    GameObject rock;
    public RangedAI rangedScript;
    public GameObject dustParticles;
    // Use this for initialization


    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void spawnRock() {
        rock = Instantiate(rock_prefab);
        ////Vector3 handPos = hand.transform.localPosition;
        rock.transform.parent = hand.transform;
        rock.transform.localPosition = new Vector3(0.3f, -0.01f, 0);
        rock.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    public void throwRock() {


        rock.GetComponent<Rigidbody>().useGravity = true;
        rock.GetComponent<BoxCollider>().enabled = true;
        rangedScript.FireProjectTile(ref rock);

    }

    public void dropRock() {
        if (rock.transform.parent == hand.transform) {
            rock.GetComponent<Rigidbody>().useGravity = true;
            rock.GetComponent<BoxCollider>().enabled = true;
            rock.transform.parent = null;
        }

    }
    public void setDust(int state) {
        if(state == 1)
            dustParticles.SetActive(true);
        else 
            dustParticles.SetActive(false);
        
    }
}
