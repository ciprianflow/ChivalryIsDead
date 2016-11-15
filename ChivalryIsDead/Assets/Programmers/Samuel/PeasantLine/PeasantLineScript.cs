using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PeasantLineScript : MonoBehaviour {

    public GameObject Peasant;

	// Use this for initialization
	void Start () {

        FillPeasantLine();

	}

    void FillPeasantLine()
    {
        BezierSpline Bezier = transform.GetComponent<BezierSpline>();

        if (Bezier == null)
            return;

        int NumOfPeasants = 25;
        //float PeasantDist = 1f / NumOfPeasants;
        float PeasantDist = 0.02f;

        float height = Peasant.GetComponent<Image>().rectTransform.sizeDelta.y / 2;

        float t = 0;
        for(int i = 0; i < NumOfPeasants; i++, t += PeasantDist)
        {
            float RandX = Random.Range(0, 40);
            float RandY = Random.Range(0, 40);
            GameObject PeasantObj = Instantiate(Peasant);
            PeasantObj.transform.SetParent(this.transform);
            PeasantObj.transform.position = Bezier.GetPoint(1 - t) + new Vector3(RandX, height + RandY, 0);
        }

    }
}
