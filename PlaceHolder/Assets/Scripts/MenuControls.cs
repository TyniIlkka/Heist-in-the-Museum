using UnityEngine.UI;
using UnityEngine;

public class MenuControls : MonoBehaviour {

    [SerializeField]
    private GameObject m_goMenuView1;
    [SerializeField]
    private GameObject m_goMenuView2;
    [SerializeField]
    private Slider m_sSfxVol;
    [SerializeField]
    private Slider m_sMusicVol;

    private void Awake()
    {
        // TODO update volume
    }

    public void NewGame()
    {

    }

    public void Options()
    {

    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void MusicVol()
    {

    }

    public void SFXVol()
    {

    }


    // TODO pause menu controls?
}
