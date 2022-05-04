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

    [SerializeField] int gameSceneIndex;
    [SerializeField] int menuSceneIndex;

    AtomScript player;

    [SerializeField] GameObject winScreen;
    [SerializeField] GameObject loseScreen;

    List<int> queuedLockAnimations = new List<int>();

    public bool isRunning { private set; get; }


    public int[] unlockedElements { private set; get; }

    public bool elementIsUnlocked(Element element)
    {
        return elementIsUnlocked(element.atomicNumber);
    }

    public bool elementIsUnlocked(int element)
    {
        if (queuedLockAnimations.Contains(element)) return false;

        foreach(int e in unlockedElements)
        {
            if (e == element) return true;
        }

        return false;
    }


    

    private void Awake()
    {
        if(gameManager == null)
        {
            gameManager = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += GameLoaded;
            SceneManager.sceneLoaded += MenuLoaded;


            unlockedElements = Storage.GetData("UnlockedElements", new int[] { });
            return;
        }

        Destroy(gameObject);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(menuSceneIndex);
    }

    public void RestartLevel()
    {
        StartGame();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneIndex);
        isRunning = true;
    }

    void MenuLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex != menuSceneIndex) return;

        if(queuedLockAnimations.Count > 0)
        {
            PeriodicTable table = FindObjectOfType<PeriodicTable>();
            foreach(int atomicNumber in queuedLockAnimations)
            {
                StaticElementDisplay sed = table.GetElement(atomicNumber);
                Debug.Log(sed);
                sed?.UnLock();
            }

            queuedLockAnimations.Clear();
        }
    }

    void GameLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex != gameSceneIndex) return;
        player = FindObjectOfType<AtomScript>();
        player.onElectronsAdded += ElectronPickedUp;
        player.onNeutronsAdded += NeutronPickedUp;
        player.onProtonsAdded += ProtonPickedUp;
    }

    public void EndGame(EndCondition condition)
    {
        isRunning = false;
        player.electronProtonAnimation?.revert();
        player.isotopeAnimation?.revert();


        switch (condition)
        {
            case EndCondition.WIN:
                Win();
                break;
            case EndCondition.LOSS:
                Lose();
                break;
        }
    }


    void Win()
    {
        Debug.Log("You won");
        //Time.timeScale = 0;
        if (!elementIsUnlocked(targetElement))
        {
        unlockedElements = new List<int>(unlockedElements) { targetElement.atomicNumber }.ToArray();
        Storage.SaveData(unlockedElements, "UnlockedElements");
        queuedLockAnimations.Add(targetElement.atomicNumber);
        }

        GameObject screen = Instantiate(winScreen, FindObjectOfType<Canvas>().gameObject.transform);
    }

    void Lose()
    {
        Debug.Log("You lost");
        GameObject screen = Instantiate(loseScreen, FindObjectOfType<Canvas>().gameObject.transform);
    }


    private void OnDestroy()
    {
        if (gameManager != this) return;
        SceneManager.sceneLoaded -= GameLoaded;
        SceneManager.sceneLoaded -= MenuLoaded;
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
