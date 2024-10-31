using System.Collections.Generic;

namespace _Assets.Scripts.Services.Datas.Player
{
    public interface IPlayerDataLoader
    {
        List<PlayerData> GameDatas { get; }
        void SaveData();
        void LoadData();
    }
}