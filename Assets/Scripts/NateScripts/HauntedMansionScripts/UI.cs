using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public static UI instance;

    private Ray ghostRay;
    public Image actionIcon;
    public Image attackIcon;
    public Image skullIcon;
    public Image femur1;
    public Image femur2;
    public Image fearBar;
    bool skullFound = false;
    bool firstBoneFound = false;
    bool secondBoneFound = false;

    void Start()
    {
        instance = this;

        skullIcon.gameObject.SetActive(false);
        femur1.gameObject.SetActive(false);
        femur2.gameObject.SetActive(false);
    }

    void Update()
    {
        SetFear();
        if (skullFound && firstBoneFound && secondBoneFound)
        {
            //load win state
        }
    }

    public void ToggleActionIcon(bool value)
    {
        actionIcon.gameObject.SetActive(value);
    }

    public void ToggleAttackIcon(bool value)
    {
        attackIcon.gameObject.SetActive(value);
    }

    public void FindSkull()
    {
        skullIcon.gameObject.SetActive(true);
        skullFound = true;
    }

    public void FindBone()
    {
        if (femur1.gameObject.activeSelf == false)
        {
            femur1.gameObject.SetActive(true);
            firstBoneFound = true;
        }
        else
        {
            if (femur2.gameObject.activeSelf == false)
            {
                femur2.gameObject.SetActive(true);
                secondBoneFound = true;
            }
        }
    }

    public void SetFear()
    {
        fearBar.fillAmount = NPC.fear/10;
        Debug.Log("Fear: " + NPC.fear);
    }
}
