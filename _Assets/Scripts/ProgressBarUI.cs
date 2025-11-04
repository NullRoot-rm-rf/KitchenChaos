using UnityEngine.UI;
using UnityEngine;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private Image progressBarImage;
    [SerializeField] private GameObject gameObjectWithProgress;

    private IHasProgress iHasProgress;

    private void Awake()
    {
        iHasProgress = gameObjectWithProgress.GetComponent<IHasProgress>();
    }

    private void Start()
    {
        iHasProgress.OnProgressChanged += IHasProgress_OnProgressChanged;
        progressBarImage.fillAmount = 0f;
        Hide();
    }

    private void IHasProgress_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
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
