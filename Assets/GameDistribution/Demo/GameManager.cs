using System;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_WEBGL && !UNITY_EDITOR

public class GameManager : MonoBehaviour
{

    // public Text gameControlText;
    // public Text rewardedAdText;
    // public Text rewardPlayerText;

    public Text scoreText;
    public Text levelText;

    void Awake()
    {
        GameDistribution.OnResumeGame += OnResumeGame;
        GameDistribution.OnPauseGame += OnPauseGame;
        GameDistribution.OnPreloadRewardedVideo += OnPreloadRewardedVideo;
        GameDistribution.OnRewardedVideoSuccess += OnRewardedVideoSuccess;
        GameDistribution.OnRewardedVideoFailure += OnRewardedVideoFailure;
        GameDistribution.OnRewardGame += OnRewardGame;
    }

    public void OnResumeGame()
    {
        // RESUME MY GAME
        Time.timeScale = 0;


    }

    public void OnPauseGame()
    {
        // PAUSE MY GAME
        Time.timeScale = 0;
    }
    public void OnRewardGame()
    {
        // REWARD PLAYER HERE

    }
    public void OnRewardedVideoSuccess()
    {

    }

    public void OnRewardedVideoFailure()
    {

    }

    public void OnPreloadRewardedVideo(int loaded)
    {
        // FEEDBACK ABOUT PRELOADED AD

    }

    public void ShowAd()
    {
        GameDistribution.Instance.ShowAd();
    }

    public void ShowRewardedAd()
    {
        GameDistribution.Instance.ShowRewardedAd();
    }

    public void PreloadRewardedAd()
    {
        GameDistribution.Instance.PreloadRewardedAd();
    }

    public void SendGameEvent()
    {
        //You can push your data here how ever you want 
        ////////////////////////          Example 1          ////////////////////////
        // int level = Int32.Parse(Regex.Replace(levelText, "[^0-9]", ""));
        // int score = Int32.Parse(Regex.Replace(scoreText, "[^0-9]", ""));
        // var obj = new EventData<GameEventData>();
        // var data = new GameEventData();
        // data.level = level;
        // data.score = score;
        // obj.data = data;
        // obj.eventName = "game_event";
        ////////////////////////          Example 2          ////////////////////////
        var obj = new EventData<MileStoneData>();
        var data = new MileStoneData();
        data.isAuthorized = true;
        data.milestoneDescription = "Test Description";
        obj.data = data;
        obj.eventName = "track-milestone";
        GameDistribution.Instance.SendEvent(JsonUtility.ToJson(obj));
    }
}
#endif