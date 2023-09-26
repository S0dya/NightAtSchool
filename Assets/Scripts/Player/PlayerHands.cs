using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHands : SingletonMonobehaviour<PlayerHands>
{
    [Header("SerializeFields")]
    [SerializeField] CanvasGroup dropButtonCG;

    [SerializeField] GameObject[] handsObjects;
    [SerializeField] GameObject[] sceneObjects;

    [SerializeField] Transform initialPosition;

    [SerializeField] Transform objectParent; 

    [HideInInspector] public int indexOfHandObject = -1;
    [HideInInspector] public int curTypeOfUsage = -1;//1pickable, 2intrectableWithPickable

    [Header("after interaction")]
    [SerializeField] CanvasGroup afterInteractionCG;
    [SerializeField] TextMeshProUGUI afterInteractionText;

    protected override void Awake()
    {
        base.Awake();

    }

    void Start()
    {
        UnSetItem();
    }

    public void SetItem(int index, int type)
    {
        if (indexOfHandObject != -1) DropItem();

        indexOfHandObject = index;
        curTypeOfUsage = type;
        GameManager.I.Open(dropButtonCG, 0.4f);

        ToggleHandObject(true);
    }
    public void UnSetItem()
    {
        indexOfHandObject = -1;
        curTypeOfUsage = -1;
        GameManager.I.Close(dropButtonCG, 0.01f);
    }

    public void DropItem()
    {
        ToggleHandObject(false);
        GameObject sceneObj = Instantiate(sceneObjects[indexOfHandObject], initialPosition.position, Quaternion.identity, objectParent);
        Rigidbody sceneObjRB = sceneObj.GetComponent<Rigidbody>();
        sceneObjRB.AddForce(transform.forward.normalized * 4, ForceMode.Impulse);
    }
    public bool UseItem(int index)
    {
        if (index == indexOfHandObject + 100)
        {
            ToggleHandObject(false);
            UnSetItem();

            return true;
        }

        afterInteractionText.text = "closed";
        GameManager.I.FadeInAndOut(afterInteractionCG, 0.8f, 1f);
        return false;
    }


    public void ToggleHandObject(bool val)
    {
        handsObjects[indexOfHandObject].SetActive(val);
    }
}
