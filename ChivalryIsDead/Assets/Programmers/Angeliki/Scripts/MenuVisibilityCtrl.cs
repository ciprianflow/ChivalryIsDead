using UnityEngine;
using UnityEngine.UI;

public class MenuVisibilityCtrl : MonoBehaviour
{
    [SerializeField]
    bool shouldStartVisible;

    GameObject _myGameObj;
    GameObject _invisibleBG;
    public GameObject menu;
    public GameObject dampenLightPanel;

    void Awake()
    {
        setupInvisibleBG();
        _myGameObj = gameObject;
        if (!shouldStartVisible)
            hide();
    }

    void setupInvisibleBG()
    {
        _invisibleBG = new GameObject("InvisibleBG");

        InvisibleBGCtrl tempInvisibleBGCtrl = _invisibleBG.AddComponent<InvisibleBGCtrl>();
        tempInvisibleBGCtrl.setParentCtrl(this);

        Image tempImage = _invisibleBG.AddComponent<Image>();
        tempImage.color = new Color(1f, 1f, 1f, 0f);

        RectTransform tempTransform = _invisibleBG.GetComponent<RectTransform>();
        tempTransform.anchorMin = new Vector2(0f, 0f);
        tempTransform.anchorMax = new Vector2(1f, 1f);
        tempTransform.offsetMin = new Vector2(0f, 0f);
        tempTransform.offsetMax = new Vector2(0f, 0f);
        tempTransform.SetParent(GetComponentsInParent<Transform>()[1], false);
        tempTransform.SetSiblingIndex(transform.GetSiblingIndex()); // put it right behind this panel in the hierarchy
    }

    void OnEnable()
    {
        _invisibleBG.SetActive(true);
    }

    public void hide()
    {
        if(menu == null)
        {
            _myGameObj.SetActive(false);
        }
        else
        {
            menu.GetComponent<GameMenu>().Pause();
        }
        
        _invisibleBG.SetActive(false);
        if(dampenLightPanel != null)
        {
            dampenLightPanel.SetActive(false);
        }


    }
}