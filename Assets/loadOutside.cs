using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadOutside : MonoBehaviour
{

    private void OnTriggerEnter(Collider collider)
    {
        // TODO: fade music and white screen cool stuff
        Debug.Log("exiting the dungeon");
        SceneManager.LoadScene("Outside");
    }
}
