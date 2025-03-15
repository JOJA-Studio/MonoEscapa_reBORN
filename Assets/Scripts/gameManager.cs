using GinjaGaming.FinalCharacterController;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour
{
   // public static MenuPausa instance { get; private set; }

    [Header("Input Actions Pause")]
    [SerializeField] private InputActionReference pause;
    [Space(20)]
    [Header("CanvasGruop Pause")]
    [SerializeField] private CanvasGroup pauseScreen;
    [Space(20)]
    [Header("SFX Buttons")]
    [SerializeField] AudioSource SFXButtons;
    [SerializeField] AudioClip[] hoversAudio;

    [SerializeField] PlayerController controller;
    [SerializeField] GameObject player;


    private bool isPaused = false;

    private void OnEnable()
    {
        pause.action.Enable();
    }

    private void Start()
    {
        SetPauseScreen(0f, false);
    }

    private void Update()
    {
        if (pause.action.WasPressedThisFrame())
        {
            TogglePause();
        }
    }

    private void OnDisable()
    {
        pause.action.Disable();
    }

    private void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        SetPauseScreen(isPaused ? 1f : 0f, isPaused);
    }

    public void BotonResume()
    {
        isPaused = false;
        Time.timeScale = 1f;
        SetPauseScreen(0f, false);
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(controller);
    }
    
    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];

        controller.TeleporterCharacter(position);
    }

    private void SetPauseScreen(float alpha, bool interactable)
    {
        pauseScreen.alpha = alpha;
        pauseScreen.interactable = interactable;
        pauseScreen.blocksRaycasts = interactable;
    }

    ////////////////////FUNCIONALIDAD SONIDO EN LOS BOTONES////////////////////////////////////////
    public void HoverAudioOn()
    {
        int randSound = Random.Range(0, hoversAudio.Length);
        AudioClip clip = hoversAudio[randSound];
        SFXButtons.PlayOneShot(clip);
    }
    ////////////////////FUNCIONALIDAD SONIDO EN LOS BOTONES////////////////////////////////////////
}
