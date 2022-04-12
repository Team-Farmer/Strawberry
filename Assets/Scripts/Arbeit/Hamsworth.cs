using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hamsworth : MonoBehaviour
{
    // Start is called before the first frame update
    void FixedUpdate()
    {
        for (int i = 0; i < GameManager.instance.farmList.Count; i++)
        {
            if (DataController.instance.gameData.berryFieldData[i].hasWeed)
            {
                StartCoroutine(DeleteWeedByHamsworth(GameManager.instance.farmList[i]));
            }
        }
    }
    IEnumerator DeleteWeedByHamsworth(Farm farm)
    {
        yield return new WaitForSeconds(0.75f);
        farm.weed.DeleteWeed();        
    }
}
