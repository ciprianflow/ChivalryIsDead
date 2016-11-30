using UnityEngine;
using UnityEngine.UI;

public class colorLerp : MonoBehaviour {

    public Color c1;
    public Color c2;
    public float transistionTime = 1f;

    float t;

    Image img;

    void Awake()
    {
        img = GetComponent<Image>();
        img.color = c1;
    }

    // Update is called once per frame
    void Update () {

        t += Time.deltaTime / transistionTime;
        img.color = Color.Lerp(c1, c2, t);

        if (t >= 1)
        {
            t = 0;
            Color c3 = c1;
            c1 = c2;
            c2 = c3;
        }


	}
}
