using UnityEngine.UI;
using UnityEngine;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private Image progressBarImage;
    [SerializeField] private CuttingCounter cuttingCounter;

    private void Start()
    {
        cuttingCounter.OnProgressChanged += CuttingCounter_OnProgressChanged;
        progressBarImage.fillAmount = 0f;
        Hide();
    }

    private void CuttingCounter_OnProgressChanged(object sender, CuttingCounter.OnProgressChangedEventArgs e)
    { 
        progressBarImage.fillAmount = e.progressNormalized;
        if (progressBarImage.fillAmount == 1f || progressBarImage.fillAmount == 0f) Hide();

        else Show();
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
