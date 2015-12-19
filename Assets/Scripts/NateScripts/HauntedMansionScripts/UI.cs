using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public static UI instance;

    private Ray ghostRay;
    public Image actionIcon;
    public Image skullIcon;
    public Image femur1;
    public Image femur2;
    public Image fearBar;

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
    }

    public void ToggleActionIcon(bool value)
    {
        actionIcon.gameObject.SetActive(value);
    }

    public void FindSkull()
    {
        skullIcon.gameObject.SetActive(true);
    }

    public void FindBone()
    {
        if (femur1.gameObject.activeSelf == false)
            femur1.gameObject.SetActive(true);
        else
        {
            if (femur2.gameObject.activeSelf == false)
                femur2.gameObject.SetActive(true);
        }
    }

    public void SetFear()
    {
        fearBar.fillAmount = NPC.fear/10;
        Debug.Log("Fear: " + NPC.fear);
    }
}
