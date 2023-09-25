using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHands : SingletonMonobehaviour<PlayerHands>
{
    [SerializeField] GameObject[] handsObjects;
    [SerializeField] GameObject[] sceneObjects;

    [SerializeField] Transform initialPosition;

    [SerializeField] Transform objectParent; 

    [HideInInspector] public int indexOfHandObject = -1;
    [HideInInspector] public int curTypeOfUsage = -1;//1pickable, 2intrectableWithPickable



    protected override void Awake()
    {
        base.Awake();

    }

    public void SetItem(int index, int type)
    {
        if (curTypeOfUsage != -1) DropItem();

        indexOfHandObject = index;
        curTypeOfUsage = type;

        ToggleHandObject(true);
    }
    public void UnSetItem()
    {
        indexOfHandObject = -1;
        curTypeOfUsage = -1;
    }

    public void DropItem()
    {
        ToggleHandObject(false);
        GameObject sceneObj = Instantiate(sceneObjects[indexOfHandObject], initialPosition.position, Quaternion.identity, objectParent);
        Rigidbody sceneObjRB = sceneObj.GetComponent<Rigidbody>();
        sceneObjRB.AddForce(transform.forward.normalized * 5, ForceMode.Impulse);

        UnSetItem();
    }
    public bool UseItem(int index)
    {
        if (index == indexOfHandObject + 100)
        {
            ToggleHandObject(false);

            return true;
        }

        return false;
    }


    public void ToggleHandObject(bool val)
    {
        handsObjects[indexOfHandObject].SetActive(val);
    }
}
