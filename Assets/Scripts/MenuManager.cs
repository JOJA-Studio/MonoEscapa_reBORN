using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class MenuManager : MonoBehaviour
{
    [Header("Canvas Group")]
    public CanvasGroup canvasMainMenu;
    public CanvasGroup canvasAjustes;
    public CanvasGroup canvasCreditos;
    public CanvasGroup canvasPlay;
    [Space(20)]
    [Header("Sonido SFX")]
    [SerializeField] AudioSource SFXButtons;
    [SerializeField] AudioClip[] hoversAudio;


    ////////////////////FUNCIONALIDAD DE CAMBIO DE ALPHA PARA LOS DIVERSOS CANVAS////////////////////////////////////////
    private void Start()
    {
        MainMenu(); 
    }

    public void MainMenu()
    {
        EnableCanvasGroup(canvasMainMenu);
    }

    public void AjustesPlaceholder()
    {
        EnableCanvasGroup(canvasAjustes);
    }

    public void CreditosPlaceholder()
    {
        EnableCanvasGroup(canvasCreditos);
    }

    public void PlayPlaceholder()
    {
        EnableCanvasGroup(canvasPlay);
    }

    private void EnableCanvasGroup(CanvasGroup activeCanvas)
    {
        CanvasGroup[] canvasGroups = { canvasMainMenu, canvasAjustes, canvasCreditos, canvasPlay };

        foreach (var canvas in canvasGroups)
        {
            bool isActive = (canvas == activeCanvas);
            canvas.alpha = isActive ? 1 : 0;
            canvas.interactable = isActive;
            canvas.blocksRaycasts = isActive;
        }
    }
    ////////////////////FUNCIONALIDAD DE CAMBIO DE ALPHA PARA LOS DIVERSOS CANVAS////////////////////////////////////////




    ////////////////////FUNCIONALIDAD BOTONES DE CAMBIAR DE NIVEL Y SALIR DEL GAME////////////////////////////////////////
    public void StartGame()
    {
        SceneManager.LoadScene("Level1");
    }

    public void DayNightGame()
    {
        SceneManager.LoadScene("DayNightCycle");
    }

    public void EnemyTestDemo()
    {
        SceneManager.LoadScene("copylvl1");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    ////////////////////FUNCIONALIDAD BOTONES DE CAMBIAR DE NIVEL Y SALIR DEL GAME////////////////////////////////////////




    ////////////////////FUNCIONALIDAD SONIDO EN LOS BOTONES////////////////////////////////////////
    public void HoverAudioOn()
    {
        int randSound = Random.Range(0, hoversAudio.Length);
        AudioClip clip = hoversAudio[randSound];
        SFXButtons.PlayOneShot(clip);
    }
    ////////////////////FUNCIONALIDAD SONIDO EN LOS BOTONES////////////////////////////////////////
}
