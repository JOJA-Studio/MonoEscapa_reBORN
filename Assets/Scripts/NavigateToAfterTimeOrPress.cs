using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class NavigateToAfterTimeOrPress : MonoBehaviour
{
    [SerializeField] string NextScene;
    [SerializeField] InputActionReference SkipKB;
    [SerializeField] InputActionReference SkipGP;
    private float timeNextScene = 5;
    private bool CheckCallScene;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnEnable()
    {
        SkipKB.action.Enable();
        SkipGP.action.Enable();
    }

    void Awake()
    {
        Invoke("NavigateToNextScreen", timeNextScene);
    }

    // Update is called once per frame
    void Update()
    {
        if (SkipKB.action.triggered || SkipGP.action.triggered)
        {
            NavigateToNextScreen();
        }
    }

    void OnDisable()
    {
        SkipKB.action.Disable();
        SkipGP.action.Enable();
    }

    void NavigateToNextScreen()
    {
        if (!CheckCallScene)
        {
            CheckCallScene = true;
            SceneManager.LoadScene(NextScene);
        }

    }
}


//boleano dentro de llamar a la escena que dira si se a accedido o no a esa escena