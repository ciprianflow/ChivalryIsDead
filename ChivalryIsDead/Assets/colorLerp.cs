using UnityEngine;
using UnityEngine.UI;

public class colorLerp : MonoBehaviour {

    public Color c1;
    public Color c2;
    public float transistionTime = 1f;

    float t;
    float oldTime = 0f;

    Image img;

    void OnEnable()
    {
        img = GetComponent<Image>();
        img.color = c1;
        oldTime = Time.realtimeSinceStartup;
    }

    // Update is called once per frame
    void Update () {

        t += (Time.realtimeSinceStartup - oldTime) / transistionTime;
        img.color = Color.Lerp(c1, c2, t);

        if (t >= 1)
        {
            t = 0;
            Color c3 = c1;
            c1 = c2;
            c2 = c3;
        }

        oldTime = Time.realtimeSinceStartup;

    }
}
