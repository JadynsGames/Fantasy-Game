using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public static class info
{
    public static bool tooltipsEnabled = true;
}

public class Player : MonoBehaviour
{
    [SerializeField]
    private LayerMask pickableLayerMask;

    [SerializeField]
    private LayerMask usableLayerMask;

    [SerializeField]
    private LayerMask defaultLayerMask;

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
    private GameObject myShield;
    [SerializeField]
    private GameObject myStaff;
    [SerializeField]
    private GameObject myCrown;

    [SerializeField]
    private AudioSource soundEffect;
    [SerializeField]
    private AudioSource voiceline;
    [SerializeField]
    private AudioSource music;

    [SerializeField]
    private Image fadeScreen;
    [SerializeField]
    private Text gameOver;
    [SerializeField]
    private Text tooltip;
    [SerializeField]
    private Text subtitles;
    [SerializeField]
    private Text closeText;

    [SerializeField]
    [Min(1)]
    private float hitRange = 3;

    private int count = 0;
    private float timeRemaining = 60f;
    public Text timeText;

    // for whether we want a fade out or in
    private float desiredAlpha;
    private float currentAlpha;

    private RaycastHit hit;

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    IEnumerator voiceover(int num)
    {
        // remove tooltip for the time being
        tooltip.gameObject.SetActive(false);

        yield return new WaitForSeconds(1);

        if (info.tooltipsEnabled)
        {
            subtitles.gameObject.SetActive(true);
        }

        if (num == 1)
        {
            voiceline.clip = (AudioClip)Resources.Load("voiceline1");
            voiceline.Play();
            yield return new WaitForSeconds(0.6f);
            subtitles.text = "“MWAHA HA HA HAHA HA HA!”";
            yield return new WaitForSeconds(2);
            subtitles.text = "“After years of trying to capture you, I have finally done it.”";
            yield return new WaitForSeconds(4.2f);
            subtitles.text = "“You'll NEVER escape this dungeon...”";
            yield return new WaitForSeconds(3.5f);
        }
        else if (num == 2)
        {
            voiceline.clip = (AudioClip)Resources.Load("voiceline2");
            voiceline.Play();
            yield return new WaitForSeconds(0.3f);
            subtitles.text = "“Uhh... Darn it. Didn't think you'd figure that one out.”";
            yield return new WaitForSeconds(4);
            subtitles.text = "“But it's okay. My door locking magic is still strong.”";
            yield return new WaitForSeconds(4);
            subtitles.text = "“As long as you don't drink that green potion I left sitting out upstairs...”";
            yield return new WaitForSeconds(5);
            subtitles.text = "“Dang it! Why'd I have to say that??”";
            yield return new WaitForSeconds(2);
        }
        else if (num == 3)
        {
            voiceline.clip = (AudioClip)Resources.Load("voiceline3");
            voiceline.Play();

            subtitles.text = "“AH! You may have disabled my evil magic,”";
            yield return new WaitForSeconds(3);
            subtitles.text = "“but that potion is too dangerous for you anyway.”";
            yield return new WaitForSeconds(3);
            subtitles.text = "“If you don't get into sunlight in one minute,”";
            yield return new WaitForSeconds(2);
            subtitles.text = "“you'll fall asleep, and you'll never escape this dungeon!”";
            yield return new WaitForSeconds(3);
            subtitles.text = "“The exit door is back where you started by the way, but you'll never make it in time.”";
            yield return new WaitForSeconds(4);
            subtitles.text = "“MWA HAHA HA HA...”";
            yield return new WaitForSeconds(3);
        }

        subtitles.text = "";
        yield return new WaitForSeconds(1);

        if (info.tooltipsEnabled && !tooltip.gameObject.activeInHierarchy)
        {
            // fade in objective
            Color c = tooltip.color;
            c.a = 0f;
            tooltip.color = c;
            tooltip.gameObject.SetActive(true);

            for (float t = 0f; c.a < 1f; t += Time.deltaTime)
            {
                c.a = c.a + 0.005f;
                tooltip.color = c;
                yield return null;
            }
        }
    }

    IEnumerator tooClose()
    {
        // fade in close info
        Color c = closeText.color;
        c.a = 1f;
        closeText.color = c;
        closeText.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        for (float t = 0f; c.a > 0f; t += Time.deltaTime)
        {
            c.a = c.a - 0.01f;
            closeText.color = c;
            yield return null;
        }
        closeText.gameObject.SetActive(false);
    }

    private void Start()
    {
        // fade in, play veer voiceline, music, and then todo list on side
        desiredAlpha = 0f;
        music.clip = (AudioClip)Resources.Load("5. Redemption - Dungeon Dash OST");
        music.Play();
        StartCoroutine(voiceover(1));
    }

    void FinalCountdown()
    {
        Debug.Log("drinking");
        soundEffect.clip = (AudioClip)Resources.Load("drink");
        soundEffect.Play();
        StartCoroutine(voiceover(3));
        music.clip = (AudioClip)Resources.Load("1. Fortitude - Dungeon Dash OST");
        music.Play();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // fading stuff
        if (desiredAlpha != fadeScreen.color.a)
        {
            currentAlpha = Mathf.MoveTowards(fadeScreen.color.a, desiredAlpha, 0.25f * Time.deltaTime);
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, currentAlpha);
        }

        if (count == 6)
        {
            // timer
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                if (timeRemaining < 0)
                {
                    timeRemaining = 0;
                }
                DisplayTime(timeRemaining);
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                count++;
                // game over, you lose
                timeText.text = "00:00";
                GameObject.Find("Crosshair").SetActive(false);
                desiredAlpha = 1.0f;
                gameOver.gameObject.SetActive(true);
                music.clip = (AudioClip)Resources.Load("6. Despair - Dungeon Dash OST");
                music.Play();
            }
        }

        // hide tooltips in corner
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (info.tooltipsEnabled)
            {
                info.tooltipsEnabled = false;
                tooltip.gameObject.SetActive(false);
                subtitles.gameObject.SetActive(false);
            }
            else
            {
                info.tooltipsEnabled = true;
                tooltip.gameObject.SetActive(true);
                // don't do subtitles here, they might interfere with tooltip
            }
        }

        Debug.DrawRay(playerCameraTransform.position, playerCameraTransform.forward * hitRange, Color.red);
        if (hit.collider != null && count != 5)
        {
            hit.collider.GetComponent<Highlight>()?.ToggleHighlight(false);
            pickUpUI.SetActive(false);
        }


        if (inHandItem != null)
        {
            if (count != 5 && Physics.Raycast(playerCameraTransform.position,
            playerCameraTransform.forward,
            out hit,
            hitRange,
            usableLayerMask))
            {
                Debug.Log("we see something usable");
                string name = hit.collider.gameObject.name;
                string myHand = inHandItem.gameObject.name;

                if (info.tooltipsEnabled)
                {
                    // check if to display message
                    if ((name == "King" && myHand == "Crown1") || name == "Priest" && myHand == "Staff1" || name == "Warrior" && myHand == "Shield1")
                    {
                        pickUpUI.GetComponent<Text>().text = "Left Click to Place";
                        pickUpUI.SetActive(true);
                    }
                    else if (name == "Cauldron_Full" && myHand == "Flask")
                    {
                        pickUpUI.GetComponent<Text>().text = "Left Click to Scoop";
                        pickUpUI.SetActive(true);
                    }
                }

                if (Input.GetMouseButtonDown(0))
                {
                    //string name = hit.collider.gameObject.name;
                    //string myHand = inHandItem.gameObject.name;
                    if (name == "King" && myHand == "Crown1")
                    {
                        Destroy(inHandItem.gameObject);
                        inHandItem = null;
                        myCrown.SetActive(true);
                        soundEffect.clip = (AudioClip)Resources.Load("crown");
                        soundEffect.Play();
                        count++;
                        pickUpUI.SetActive(false);
                    }
                    else if (name == "Priest" && myHand == "Staff1")
                    {
                        Destroy(inHandItem.gameObject);
                        inHandItem = null;
                        myStaff.SetActive(true);
                        soundEffect.clip = (AudioClip)Resources.Load("staff");
                        soundEffect.Play();
                        count++;
                        pickUpUI.SetActive(false);
                    }
                    else if (name == "Warrior" && myHand == "Shield1")
                    {
                        Destroy(inHandItem.gameObject);
                        inHandItem = null;
                        myShield.SetActive(true);
                        soundEffect.clip = (AudioClip)Resources.Load("shield");
                        soundEffect.Play();
                        count++;
                        pickUpUI.SetActive(false);
                    }
                    else if (name == "Cauldron_Full" && myHand == "Flask")
                    {
                        Destroy(inHandItem.gameObject);
                        inHandItem = null;
                        // play SCOOPING sound effect
                        // now we load in a green potion prefab
                        Object prefab = Resources.Load("Potion", typeof(GameObject));
                        GameObject clone = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
                        inHandItem = clone;
                        inHandItem.layer = 8;
                        inHandItem.transform.SetParent(pickUpParent.transform, false);
                        soundEffect.clip = (AudioClip)Resources.Load("scoop");
                        soundEffect.Play();
                        if (count == 4)
                        {
                            tooltip.text = "Objective:\nDrink the escape elixir";
                            pickUpUI.GetComponent<Text>().text = "Left Click to Drink";
                            if (info.tooltipsEnabled)
                            {
                                pickUpUI.SetActive(true);
                            } else
                            {
                                pickUpUI.SetActive(false);
                            }

                            count++;
                        }
                    }
                }
                else
                {
                    return;
                }
            } else
            {
                // drop our item
                if (Input.GetMouseButtonDown(0))
                {
                    if (count == 5 && inHandItem.gameObject.name == "Potion(Clone)")
                    {
                        pickUpUI.SetActive(false);
                        FinalCountdown();
                        Destroy(inHandItem.gameObject);
                        inHandItem = null;
                        // play drinking sound effect
                        Object prefab = Resources.Load("Flask", typeof(GameObject));
                        GameObject clone = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
                        inHandItem = clone;
                        inHandItem.layer = 8;
                        inHandItem.transform.SetParent(pickUpParent.transform, false);
                        count++;
                        tooltip.text = "Objective:\nReturn to the first room and escape the dungeon";
                        // ^^ countdown begins after this
                        // open doors
                        GameObject.Find("Door_Iron_Right").transform.Rotate(0.0f, 135.0f, 0.0f, Space.Self);
                        GameObject.Find("Door_Wooden_Round_Right").transform.Rotate(0.0f, -225.0f, 0.0f, Space.Self);
                    } else if (Physics.Raycast(playerCameraTransform.position,
            playerCameraTransform.forward,
            out hit,
            hitRange, defaultLayerMask))
                    {
                        // here we are against the wall. Can't drop there then, so play sound effect and stop it
                        Debug.Log("too close to wall to drop");
                        soundEffect.clip = (AudioClip)Resources.Load("error");
                        soundEffect.Play();
                        // if tooltips are off we don't show this
                        if (info.tooltipsEnabled)
                        {
                            StartCoroutine(tooClose());
                        }
                    } else
                    {
                        Debug.Log("here");
                        inHandItem.transform.SetParent(dropArea.transform, false);
                        inHandItem.transform.SetParent(null);
                        // 7 -> pickable layer
                        inHandItem.layer = 7;
                        inHandItem.GetComponent<Rigidbody>().isKinematic = false;
                        inHandItem = null;
                    }
                
                }
                return;
            }
        }

        // highlight stuff to show we are allowed to pick it up
        if (Physics.Raycast(playerCameraTransform.position,
            playerCameraTransform.forward,
            out hit,
            hitRange,
            pickableLayerMask))
        {
            hit.collider.GetComponent<Highlight>()?.ToggleHighlight(true);
            pickUpUI.GetComponent<Text>().text = "Left Click to Pick Up";
            if (info.tooltipsEnabled)
            {
                pickUpUI.SetActive(true);
            }
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
                // 7 is a pickable item, 8 is an in-hand item
                inHandItem.layer = 8;
                if (hit.collider.name == "Flask" && count == 4)
                {
                    tooltip.text = "Objective:\nScoop up some escape elixir";
                }
            }
            Debug.Log("Pressed left-click.");
        }

        // check where we are
        if (count == 3)
        {
            Debug.Log("Advance to the next stage!");
            tooltip.text = "Objective:\nFind the brewery and pick up a glass bottle";
            GameObject.Find("MyDoor1").transform.eulerAngles = Vector3.zero;
            StartCoroutine(voiceover(2));
            soundEffect.clip = (AudioClip)Resources.Load("door");
            soundEffect.Play();
            int x = Random.Range(0, 2);
            if (x == 1)
            {
                music.clip = (AudioClip)Resources.Load("4. Progress - Dungeon Dash OST");
            } else
            {
                music.clip = (AudioClip)Resources.Load("3. Motivation - Dungeon Dash OST");
            }
            music.Play();
            count++;
        }
    }
}