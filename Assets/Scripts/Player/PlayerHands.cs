using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHands : SingletonMonobehaviour<PlayerHands>
{
    [Header("SerializeFields")]
    [SerializeField] CanvasGroup dropButtonCG;
    [SerializeField] TextMeshProUGUI nameOfObjText;

    [SerializeField] GameObject[] handsObjects;
    [SerializeField] GameObject[] sceneObjects;

    [SerializeField] Transform initialPosition;

    [SerializeField] Transform objectParent; 

    [HideInInspector] public int index = -1;
    [HideInInspector] public int typeOfUsage = -1;//1pickable, 2intrectableWithPickable
    [HideInInspector] public string nameOfObj;
    [HideInInspector] public int indexOfHandObj;

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

    public void SetItem(Interactable interactable)
    {
        if (index != -1) DropItem();

        index = interactable.index;
        typeOfUsage = interactable.type;
        nameOfObj = interactable.nameOfObj;
        indexOfHandObj = interactable.indexOfHandObj;

        nameOfObjText.text = nameOfObj;
        GameManager.I.Open(dropButtonCG, 0.4f);

        ToggleHandObject(true);
    }
    public void UnSetItem()
    {
        index = -1;
        typeOfUsage = -1;
        nameOfObj = null;
        indexOfHandObj = -1;
        nameOfObjText.text = null;

        GameManager.I.Close(dropButtonCG, 0.01f);
    }

    public void DropItem()
    {
        ToggleHandObject(false);
        GameObject sceneObj = Instantiate(sceneObjects[indexOfHandObj], initialPosition.position, Quaternion.identity, objectParent);

        Interactable interactable = sceneObj.GetComponent<Interactable>();
        interactable.index = index;
        interactable.type = typeOfUsage;
        interactable.nameOfObj = nameOfObj;
        interactable.indexOfHandObj = indexOfHandObj;

        Rigidbody sceneObjRB = sceneObj.GetComponent<Rigidbody>();
        sceneObjRB.AddForce(transform.forward.normalized * 4, ForceMode.Impulse);
    }
    public bool UseItem(int i)
    {
        if (index != -1 && i == index + 100)
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
        handsObjects[index].SetActive(val);
    }
}
