using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadOutside : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        // TODO: fade music and white screen cool stuff
        Debug.Log("exiting the dungeon");
        SceneManager.LoadScene("Outside");
    }
}
