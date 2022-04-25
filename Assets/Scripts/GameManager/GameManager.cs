using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum EndCondition
{
    WIN,
    LOSS
}

public class GameManager : MonoBehaviour
{
    public Element targetElement;

    public static GameManager gameManager;

    [SerializeField] Object gameScene;

    AtomScript player;


    public int[] unlockedElements { private set; get; }

    public bool elementIsUnlocked(Element element)
    {
        return elementIsUnlocked(element.atomicNumber);
    }

    public bool elementIsUnlocked(int element)
    {
        foreach(int e in unlockedElements)
        {
            if (e == element) return true;
        }

        return false;
    }


    public bool isRunning { private set; get; }

    private void Awake()
    {
        if(gameManager == null)
        {
            gameManager = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += GameLoaded;


            unlockedElements = Storage.GetData("UnlockedElements", new int[] { });
            return;
        }

        Destroy(gameObject);
    }


    public void StartGame()
    {
        SceneManager.LoadScene(gameScene.name);
    }

    void GameLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != gameScene.name) return;
        player = FindObjectOfType<AtomScript>();
        player.onElectronsAdded += ElectronPickedUp;
        player.onNeutronsAdded += NeutronPickedUp;
        player.onProtonsAdded += ProtonPickedUp;
    }

    public void EndGame(EndCondition condition)
    {
        switch (condition)
        {
            case EndCondition.WIN:
                Debug.Log("You won");
                Time.timeScale = 0;
                unlockedElements = new List<int>(unlockedElements) { targetElement.atomicNumber }.ToArray();
                Storage.SaveData(unlockedElements, "UnlockedElements");
                break;
            case EndCondition.LOSS:
                break;
        }
    }


    private void OnDestroy()
    {
        if (gameManager != this) return;
        SceneManager.sceneLoaded -= GameLoaded;
        if (player == null) return;
        player.onElectronsAdded -= ElectronPickedUp;
        player.onNeutronsAdded -= NeutronPickedUp;
        player.onProtonsAdded -= ProtonPickedUp;
    }




    void ProtonPickedUp(uint before, uint after, uint change, AtomScript atomScript)
    {
        CheckWin();
    }

    void ElectronPickedUp(uint before, uint after, uint change, AtomScript atomScript)
    {
        CheckWin();
    }

    void NeutronPickedUp(uint before, uint after, uint change, AtomScript atomScript)
    {
        CheckWin();
    }

    void CheckWin()
    {
        if(player.getProtons() == targetElement.atomicNumber && player.getNeutrons() == targetElement.numberOfNeutrons && player.getElectrons() == targetElement.atomicNumber)
        {
            EndGame(EndCondition.WIN);
        }
    }


}
