using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour {

    public Texture2D texture;
    static LoadingScreen instance;
    public static GameObject button;

    void Awake()
   {
         if (instance)
         {
             Destroy(gameObject);
             hide();
             return;
         }
         button = GameObject.FindGameObjectWithTag("Button");
         instance = this;    
         gameObject.AddComponent<GUITexture>().enabled = false;
         GetComponent<GUITexture>().texture = texture;
         transform.position = new Vector3(0.5f, 0.5f, 1f);
         //DontDestroyOnLoad(this);
         
    }

    //void Update()
    //{
    //    if (!Application.isLoadingLevel)
    //        hide();
    //}

    public static void show()
    {
         if (!InstanceExists()) 
         {
             return;
         }
         instance.GetComponent<GUITexture>().enabled = true;
        HideButton();
        
     }
         
     public static void hide()
    {
        if (!InstanceExists())
        {
            return;
        }
        instance.GetComponent<GUITexture>().enabled = false;
    }

    public static void HideButton()
    {
        button.GetComponent<Image>().enabled = false;
    }
     static bool InstanceExists()
     {
        if (!instance)
        {
            return false;
        }
        return true;

     }

}
