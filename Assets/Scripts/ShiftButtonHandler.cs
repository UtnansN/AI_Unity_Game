using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class ShiftButtonHandler : MonoBehaviour
{

    private Button _upButton;
    private Button _downButton;
    private Button _leftButton;
    private Button _rightButton;
    
    void Start()
    {
        _upButton = transform.GetChild(0).GetComponent<Button>();
        _downButton = transform.GetChild(2).GetComponent<Button>();

        GameObject horizontalButtons = transform.GetChild(1).gameObject;
        _leftButton = horizontalButtons.transform.GetChild(0).GetComponent<Button>();
        _rightButton = horizontalButtons.transform.GetChild(1).GetComponent<Button>();
        
        _upButton.onClick.AddListener(() => { GameManager.Instance.PerformMovement(MovementDirection.Up, true); });
        _downButton.onClick.AddListener(() => { GameManager.Instance.PerformMovement(MovementDirection.Down, true); });
        _leftButton.onClick.AddListener(() => { GameManager.Instance.PerformMovement(MovementDirection.Left, true); });
        _rightButton.onClick.AddListener(() => { GameManager.Instance.PerformMovement(MovementDirection.Right, true); });
    }

    public void EnableMoveButtons()
    {
        _upButton.interactable = true;
        _downButton.interactable = true;
        _leftButton.interactable = true;
        _rightButton.interactable = true;
    }

    public void DisableMoveButtons()
    {
        _upButton.interactable = false;
        _downButton.interactable = false;
        _leftButton.interactable = false;
        _rightButton.interactable = false;
    }
}
