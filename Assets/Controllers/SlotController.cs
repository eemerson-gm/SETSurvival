using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotController : MonoBehaviour
{

    public GameObject Icon;

    public void AddItem(int item)
    {
        Icon.SetActive(true);
        //Icon.GetComponent<Image>().sprite = 
    }

    public void ClearItem()
    {
        Icon.SetActive(false);
    }
}
