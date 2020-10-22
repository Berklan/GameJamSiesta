using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public string[] msg;
    public Sprite[] img;
    public AudioClip[] aud;

    private int index;
    private bool active = false;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            if (SceneManager.GetActiveScene().name == "GameOver")
            {
                Text text = GameObject.Find("Message").GetComponent<Text>();
                Image image = GameObject.Find("Image").GetComponent<Image>();
                AudioSource audio = GameObject.Find("Audio").GetComponent<AudioSource>();
                text.text = msg[index];
                image.sprite = img[index];
                audio.clip = aud[index];
                Text gOT = GameObject.Find("GameOverText").GetComponent<Text>();

                if (audio.clip != null && !audio.isPlaying)
                {
                    Debug.Log(audio.clip);
                    audio.Play();
                }

                if (index > 3)
                {
                    gOT.text = "Victory";
                    gOT.color = Color.green;
                }
                else
                {
                    gOT.text = "Game Over";
                    gOT.color = Color.red;
                }
            }
        }
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void GameOverMessage(string tag)
    {
        switch(tag)
        {
            case "time":
                index = 0;
                break;

            case "awake":
                index = 1;
                break;

            case "firecrackers out":
                index = 2;
                break;

            case "holding firecrackers":
                index = 3;
                break;

            case "success firecrackers":
                index = 4;
                break;

            case "success water":
                index = 5;
                break;

            case "success vuvuzela":
                index = 6;
                break;

            case "success trap":
                index = 7;
                break;

            case "success foam":
                index = 8;
                break;
        }

        active = true;

        SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
    }
}
