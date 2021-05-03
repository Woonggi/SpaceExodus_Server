using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour
{
    // Start is called before the first frame update
    // Update is called once per frame
    bool sceneChange = false;
    int mod = 1;
    void Update()
    {
        float alpha = GetComponent<SpriteRenderer>().color.a;

        if (alpha > 1)
        {
            mod *= -1;
        }
        else if (alpha < 0)
        {
            mod *= -1;
            sceneChange = true;
        }
        alpha = (mod * Time.deltaTime * 250f) / 255f;

        GetComponent<SpriteRenderer>(). color += new Color(0, 0, 0, alpha);
        if (sceneChange == true)
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
