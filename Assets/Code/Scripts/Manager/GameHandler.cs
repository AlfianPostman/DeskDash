using UnityEngine;
using TMPro;

public class GameHandler : MonoBehaviour
{
    public int totalSoap;
    [ReadOnly, SerializeField]public int currentSoap = 0;

    public float timerDuration = 60f;
    private float currentTime;
    public bool isTimerRunning = false;

    NavMeshObjectSpawner spawner;

    public GameObject losingCanvas;
    public GameObject winningCanvas;

    [SerializeField] TMP_Text text;

    private void Start() 
    {
        spawner = GetComponent<NavMeshObjectSpawner>();
        currentTime = timerDuration;
    }

    private void Update() 
    {
        if (isTimerRunning)
        {
            // Decrease the timer
            currentTime -= Time.deltaTime;

            // Clamp the timer to not go below zero
            currentTime = Mathf.Max(currentTime, 0f);

            text.text = "" + currentTime;

            if (currentSoap == totalSoap)
            {
                isTimerRunning = false;
                Winning();
            }

            // Check if the timer has finished
            if (currentTime <= 0f)
            {
                isTimerRunning = false;
                OnTimerEnd();
            }
        }
    }

    void OnTimerEnd()
    {
        Losing();
    }

    public void SetTimer(float timer)
    {
        timerDuration = timer;
    }

    public void Play(int amount)
    {
        totalSoap = amount;
        spawner.SpawnObjects(totalSoap);
    }

    public void Losing()
    {
        losingCanvas.SetActive(true);
    }

    public void Winning()
    {
        winningCanvas.SetActive(true);
    }
}
