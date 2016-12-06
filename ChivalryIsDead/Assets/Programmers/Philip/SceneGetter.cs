using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneGetter : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    // This field can be accesed through our singleton instance,
    // but it can't be set in the inspector, because we use lazy instantiation
    public int number;

    // Static singleton instance
    private static SceneGetter instance;

    // Static singleton property
    public static SceneGetter Instance {
        // Here we use the ?? operator, to return 'instance' if 'instance' does not equal null
        // otherwise we assign instance to a new component and return that
        get { return instance ?? (instance = new GameObject("Singleton").AddComponent<SceneGetter>()); }
    }

    public bool isTutorial1() {
        if(SceneManager.GetActiveScene().name == "IntroLevel" || SceneManager.GetActiveScene().name == "Introlevel") {
            return true;
        }
            return false;
    }

    public bool isTutorial2() {
        if (SceneManager.GetActiveScene().name == "Tutorial_02") {
            return true;
        }
        return false;
    }

    public bool isTutorial3() {
        if (SceneManager.GetActiveScene().name == "Tutorial_03") {
            return true;
        }
        return false;
    }

    public bool isTutHubWorld() {
        if (SceneManager.GetActiveScene().name == "TutHubWorld 1" || SceneManager.GetActiveScene().name == "TutHubWorld 2") {
            return true;
        }
        return false;
    }

    public bool isHubWorld() {
        if (SceneManager.GetActiveScene().name == "ProtoHubWorld 1") {
            return true;
        }
        return false;
    }

    public bool isMainMenu() {
        if (SceneManager.GetActiveScene().name == "MainMenu") {
            return true;
        }
        return false;
    }

    public bool isMainQuest() {
        string n = SceneManager.GetActiveScene().name;
        if (n == "01UR" || 
            n == "02UR" || 
            n == "03UR" || 
            n == "04UR" || 
            n == "05UR" || 
            n == "06UR") {
            return true;
        }
        return false;
    }
}
