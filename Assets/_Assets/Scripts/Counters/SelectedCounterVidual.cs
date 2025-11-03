using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class SelectedCounterVidual : MonoBehaviour
{
    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] visualGameObjectArray;

    private void Start()
    {
        Player.Instance.OnSelectedCounterChanged += Player_onSelectedCounterChanged;
    }

    private void Player_onSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if (e.selectedCounter == baseCounter)
        {

            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        foreach(GameObject gameObject in visualGameObjectArray)
        { 
            gameObject.SetActive(true);
        }
    }

    private void Hide()
    {
        foreach (GameObject gameObject in visualGameObjectArray)
        { 
            gameObject.SetActive(false); 
        }
    }
}

