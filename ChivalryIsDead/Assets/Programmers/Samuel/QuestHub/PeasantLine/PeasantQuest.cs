using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class PeasantQuest : MonoBehaviour, IPointerDownHandler
{

    //Yes this script is way more complciated than it needs to be
    public int QuestIndex;

    public delegate void DelegateOnClickEvent(int i);
    public DelegateOnClickEvent delegateOnClickEvent;
    public DelegateOnClickEvent delegateOnClickEvent2;

    public void OnPointerDown(PointerEventData eventData)
    {
        delegateOnClickEvent2(QuestIndex);
        delegateOnClickEvent(1);
    }
}
