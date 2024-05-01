using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Home : MonoBehaviour
{
    public static Home Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI scoreTxt;
    private void Awake ()
    {
        Instance = this;
    }

    private void Start ()
    {
        var userData = GameSystem.userdata;
        scoreTxt.text = userData.highestScore.ToString ();
    }

    public void ToGameplay()
    {
        Invoke(nameof(MoveToGamePlay), 0.25f);
    }
    public void MoveToGamePlay()
    {
        SceneManager.LoadScene("GameplayUI");
    }
}
