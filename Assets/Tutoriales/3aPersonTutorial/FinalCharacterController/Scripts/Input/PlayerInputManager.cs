using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GinjaGaming.FinalCharacterController
{
    [DefaultExecutionOrder(-3)]
    public class PlayerInputManager : MonoBehaviour
    {

        public static PlayerInputManager Instance;
        public PlayerControlsTutorial PlayerControls { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            { 
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            PlayerControls = new PlayerControlsTutorial();
            PlayerControls.Enable();
        }

        private void OnDisable()
        {
            PlayerControls.Disable();
        }
    }
}
