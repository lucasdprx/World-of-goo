using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _chooseLevel;
    [SerializeField] private GameObject _optionMenu;
    [SerializeField] private GameObject _creditMenu;
    public void SetUiChooseLevel(bool state)
    {
        _mainMenu.SetActive(!state);
        _chooseLevel.SetActive(state);
    }
    public void SetUiOption(bool state)
    {
        _mainMenu.SetActive(!state);
        _optionMenu.SetActive(state);
    }
    public void SetUiCredit(bool state)
    {
        _mainMenu.SetActive(!state);
        _creditMenu.SetActive(state);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
