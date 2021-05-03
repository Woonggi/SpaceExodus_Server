using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject startMenu;
    public InputField killField;
    public Text message;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroyging object!");
            Destroy(this);
        }
    }

    public void HostServer()
    {
        int goalKills = 0;
        if (int.TryParse(killField.text, out goalKills) == true)
        {
            if (goalKills > 0)
            {
                startMenu.SetActive(false);
                killField.interactable = false;
                Server.goalKills = goalKills;
                Server.Start(10, 26950);
                Destroy(killField);
            }
            else
            {
                message.text = "Kills must be greater than 0";
                StartCoroutine("ResetMessage");
            }

        }
        else
        {
            message.text = "Kills must be numeric value.";
            StartCoroutine("ResetMessage");
        }
    }

    public IEnumerator ResetMessage ()
    {
        yield return new WaitForSeconds(2.0f);
        message.text = "";
    }
}
