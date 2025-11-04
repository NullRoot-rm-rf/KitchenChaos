using System.Runtime.CompilerServices;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] private GameObject particals;
    [SerializeField] private GameObject stoveOnVisual;
    [SerializeField] private StoveCounter stove;

    private void Start()
    {
        stove.OnStateChange += Stove_OnStateChange;
    }

    private void Stove_OnStateChange(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        bool showVisual = e.state == StoveCounter.CookingStates.Cooking || e.state == StoveCounter.CookingStates.Cooked;
        particals.SetActive(showVisual);
        stoveOnVisual.SetActive(showVisual);
    }
}
