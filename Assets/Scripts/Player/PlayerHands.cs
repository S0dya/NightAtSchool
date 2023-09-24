using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHands : MonoBehaviour
{
    [SerializeField] GameObject[] handsObjects;
    [SerializeField] GameObject[] sceneObjects;

    [SerializeField] Transform initialPosition;

    [SerializeField] Transform objectParent; 

    [HideInInspector] public int indexOfHandObject;
    [HideInInspector] public int curTypeOfUsage;



    public void SetItem(int index, int type)
    {
        indexOfHandObject = index;
        curTypeOfUsage = type;
    }

    public void DropItem()
    {
        GameObject sceneObj = Instantiate(sceneObjects[indexOfHandObject], initialPosition.position, Quaternion.identity, objectParent);
        Rigidbody sceneObjRB = sceneObj.GetComponent<Rigidbody>();
        //sceneObjRB.AddForce();
    }
}
