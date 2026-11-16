using UnityEngine;
using UnityEngine.UI;

public class ErrorWindow : MonoBehaviour
{
    [SerializeField] private GameObject errorWindow;
    [SerializeField] private Button okButton;

    public Button GetEvent() => okButton;
    public void ShowErrorWindow() => errorWindow.SetActive(true);
    public void CloseErrorWindow() => errorWindow.SetActive(false);
}
