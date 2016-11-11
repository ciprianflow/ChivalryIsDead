using UnityEngine;
using System.Collections;

public class INITMONSTERTESTSCRIPT : MonoBehaviour {

	void Awake()
    {
        MonsterAI m = transform.GetComponent<MonsterAI>();
           
    }

    void Start()
    {
        MonsterAI m = transform.GetComponent<MonsterAI>();
        m.InitMonster();
        m.targetObject = StaticIngameData.player;
        m.playerAction = StaticIngameData.playerAction;
    }
}
