using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sdf23 : MonoBehaviour
{
    public Button writingButton;
    public Button quizButton;

    void Start()
    {

        {
            writingButton.interactable = true;
        }
        {
            quizButton.interactable = true;
        }
    }

    public void StartListening()
    {
       // LevelSceneController.instance.StartListening();
    }
    public void StartWriting()
    {
     //   LevelSceneController.instance.StartWriting();
    }
    public void StartQuiz()
    {
       // LevelSceneController.instance.StartQuiz();
    }
}
