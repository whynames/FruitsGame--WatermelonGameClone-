namespace _Assets.Scripts.Services.StateMachine
{
    public enum GameStateType : byte
    {
        None = 0,
        LoadSavedData = 1,
        Init = 2,
        Game = 3,
        GameOver = 4,
        GameOverAndMainMenu = 11,
        SaveData = 5,
        ResetAndRetry = 6,
        ResetAndMainMenu = 7,
        GamePause = 8,
        GameResume = 9,
        ContinueGame = 10
    }
}