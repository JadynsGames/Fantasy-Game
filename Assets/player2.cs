using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class player2 : MonoBehaviour
{
    [SerializeField]
    private LayerMask pickableLayerMask;

    [SerializeField]
    private Transform playerCameraTransform;

    [SerializeField]
    private Transform pickUpParent;

    [SerializeField]
    private Transform dropArea;

    [SerializeField]
    private GameObject pickUpUI;

    [SerializeField]
    private GameObject inHandItem;

    [SerializeField]
    private Text tooltip;

    private bool tooltipsEnabled = true;


    [SerializeField]
    [Min(1)]
    private float hitRange = 3;

    private RaycastHit hit;


    // Update is called once per frame
    void LateUpdate()
    {

        Debug.DrawRay(playerCameraTransform.position, playerCameraTransform.forward * hitRange, Color.red);
        if (hit.collider != null)
        {
            hit.collider.GetComponent<Highlight>()?.ToggleHighlight(false);
            pickUpUI.SetActive(false);
        }


        if (inHandItem != null)
        {
                if (Input.GetMouseButtonDown(0))
                {
                // drop item
                        inHandItem.transform.SetParent(dropArea.transform, false);
                        inHandItem.transform.SetParent(null);
                        inHandItem.GetComponent<Rigidbody>().isKinematic = false;
                        inHandItem.layer = 7;
                        inHandItem = null;
                    

                }
                return;
        }

        // highlight stuff to show we are allowed to pick it up
        if (Physics.Raycast(playerCameraTransform.position,
            playerCameraTransform.forward,
            out hit,
            hitRange,
            pickableLayerMask))
        {
            pickUpUI.SetActive(true);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (hit.collider != null)
            {
                Debug.Log(hit.collider.name);
                inHandItem = hit.collider.gameObject;
                inHandItem.transform.position = Vector3.zero;
                inHandItem.transform.rotation = Quaternion.identity;
                inHandItem.transform.SetParent(pickUpParent.transform, false);
                inHandItem.GetComponent<Rigidbody>().isKinematic = true;
                inHandItem.layer = 8;
            }
            Debug.Log("Pressed left-click.");
        }

        // hide tooltips in corner
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (tooltipsEnabled)
            {
                tooltipsEnabled = false;
                tooltip.gameObject.SetActive(false);
            }
            else
            {
                tooltipsEnabled = true;
                tooltip.gameObject.SetActive(true);
            }
        }
    }
}