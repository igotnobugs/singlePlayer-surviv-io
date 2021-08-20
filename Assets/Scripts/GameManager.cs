using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour 
{
    public GameObject mainCamera;
    public UIManager UIManager;

    public RectTransform mainMenu;
    public RectTransform[] gameUi;
    public RectTransform[] playerSpecificUI;

    public static int unitsLeft = 0;
    public static int unitsTotal = 0;

    private GameObject playerTracked;

    public static List<Unit> allUnits = new List<Unit>();

    private float countDownToChangeCamera = 0;
    private bool startCameraRandom = false;
    private bool isGameStart = false;

    public GameObject youWin;

    public void Start() {
        unitsLeft = 0;
        unitsTotal = 0;
        startCameraRandom = false;
        isGameStart = false;
        countDownToChangeCamera = 0;
    }

    //Called by start button
    public void StartGame() {
        StartCoroutine(StartSequence());
    }

    private IEnumerator StartSequence() {
        //Enable game UI
        mainMenu.gameObject.SetActive(false);
        for (int i = 0; i < gameUi.Length; i++) {
            gameUi[i].gameObject.SetActive(true);
            yield return null;
        }

        //Spawn loot
        SpawnLoot[] loots = FindObjectsOfType<SpawnLoot>();
        for (int i = 0; i < loots.Length; i++) {
            loots[i].Spawn();
            yield return null;
        }

        //Spawn Units
        SpawnUnit[] units = FindObjectsOfType<SpawnUnit>();

        for (int i = 0; i < units.Length; i++) {
            units[i].Spawn();
            unitsLeft++;
            unitsTotal++;
            yield return null;
        }

        //Should only have one player, and find it
        Player player = FindObjectOfType<Player>();
        playerTracked = player.gameObject;
        player.OnDeath += TrackPlayerKiller;

        //Make camera track player
        ConstraintSource cameraSource = new ConstraintSource {
            sourceTransform = player.transform,
            weight = 1      
        };
        mainCamera.GetComponent<PositionConstraint>().AddSource(cameraSource);

        //Make UI track player stats
        UIManager.StartTracking(player);

        isGameStart = true;
        yield break;
    }

    private void Update() {
        if (!isGameStart) return;


        if (playerTracked == null) {
            if (!startCameraRandom) {
                for (int i = 0; i < playerSpecificUI.Length; i++) {
                    playerSpecificUI[i].gameObject.SetActive(false);
                }
                startCameraRandom = true;
                countDownToChangeCamera = 5.0f;
            } else {
                if (countDownToChangeCamera > 0) {
                    countDownToChangeCamera -= Time.deltaTime;
                } else {
                    int random = Random.Range(0, allUnits.Count - 1);
                    GameObject cameraTracked = allUnits[random].gameObject;
                    UIManager.StartTracking(cameraTracked.GetComponent<Unit>(), false);
                    ConstraintSource cameraSource = new ConstraintSource {
                        sourceTransform = cameraTracked.transform,
                        weight = 1
                    };
                    mainCamera.GetComponent<PositionConstraint>().SetSource(0, cameraSource);
                    countDownToChangeCamera += 5.0f;
                }
            }
        } else {
            if (allUnits.Count == 1) {
                isGameStart = false;
                youWin.gameObject.SetActive(true);
            }
        }
    }

    public void TrackPlayerKiller() {
        Unit playerKiller = playerTracked.GetComponent<Unit>().lastHitBy;
        UIManager.ShowGameOver(playerKiller.UnitName);      
        UIManager.StartTracking(playerKiller, false);
        ConstraintSource cameraSource = new ConstraintSource {
            sourceTransform = playerKiller.gameObject.transform,
            weight = 1
        };
        mainCamera.GetComponent<PositionConstraint>().SetSource(0, cameraSource);
    }

    //Called by game over button
    public void ResetScene() {
        SceneManager.LoadScene(0);
    }
}
