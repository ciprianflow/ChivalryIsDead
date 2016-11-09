using UnityEngine;
using System.Collections;

public class INITMONSTERTESTSCRIPT : MonoBehaviour {

	void Awake()
    {
        MonsterAI m = transform.GetComponent<MonsterAI>();

        m.InitMonster();
    }

    void Start()
    {
        MonsterAI m = transform.GetComponent<MonsterAI>();

        m.targetObject = StaticData.player;
    }
}
