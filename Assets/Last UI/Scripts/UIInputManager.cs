using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using UnityEngine.EventSystems;


public class UIInputManager : MonoBehaviour
{
    private LastUIInputActions lastuiInputActions;
    private StateManager stateManager;

    [Tooltip("Assign whatever button you want it to start with selected in here.")]
    public Button FirstSelectedButton;

    [HideInInspector] public GameObject SelectedButton;

   


    private void OnEnable()
    {
        lastuiInputActions.Enable();
    }

    private void OnDisable()
    {
        lastuiInputActions.Disable();
    }

    public void Awake()
    {
        lastuiInputActions = new LastUIInputActions();
        stateManager = StateManager.GetInstance();
        

        SelectedButton = FirstSelectedButton.gameObject;

    }
    



    void Start()
    {
        FirstSelectedButton.Select();

        lastuiInputActions.LastUI.Cancel.performed += ctx => ifCancelPressed();
        lastuiInputActions.LastUI.Cancel.performed += ctx => Debug.Log("Go Back Pressed");
        lastuiInputActions.LastUI.Navigate.performed += ctx => changeSliderValue(ctx.ReadValue<Vector2>());
        lastuiInputActions.LastUI.Submit.performed += ctx => SubmitPerformed();



    }

    private void Update()
    {
        SelectedButton = EventSystem.current.currentSelectedGameObject;
    }

    void ifCancelPressed()
    {
        if (stateManager.ActiveCanvas.canvasType.canGoPreviousCanvas == true) 
        { 
            StartCoroutine(stateManager.PlayPreviousCanvasAnimation()); 
        }
    }

    void changeSliderValue(Vector2 direction)
    {
       // Debug.Log(direction);


        if (SelectedButton.TryGetComponent(out ItemController controller))
        {



            switch (SelectedButton.GetComponent<ItemController>().itemType)
            {
               
                    case ItemController.itemTypes.HorizontalSelector:

                    if (direction.x == -1)
                    {
                        SelectedButton.transform.GetChild(0).GetChild(0).GetComponent<Button>().onClick.Invoke();
                    }

                    if (direction.x == 1)
                    {
                        SelectedButton.transform.GetChild(0).GetChild(1).GetComponent<Button>().onClick.Invoke();
                    }


                    return;


            }
        }
        
        
    }

    void SubmitPerformed()
    {
        


        if (SelectedButton.TryGetComponent(out ItemController controller))
        {



            switch (SelectedButton.GetComponent<ItemController>().itemType)
            {
                case ItemController.itemTypes.Toggle:

                   if(SelectedButton.GetComponentInChildren<Toggle>().isOn == true)
                    {
                        SelectedButton.GetComponentInChildren<Toggle>().isOn = false;
                    } else
                    {
                        SelectedButton.GetComponentInChildren<Toggle>().isOn = true;
                    }


                    return;



            }
        }


}

    public void SelectObject(Selectable select)
    {
        select.Select();
    }











}
