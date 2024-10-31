using _Assets.Scripts.Services.Datas;
using _Assets.Scripts.Services.Datas.Player;

namespace _Assets.Scripts.Services
{
    public class LeaderBoardService
    {
        private readonly IPlayerDataLoader _playerDataLoader;

        private LeaderBoardService(IPlayerDataLoader playerDataLoader)
        {
            _playerDataLoader = playerDataLoader;
        }

        public int GetScore(int index)
        {
            if (index > _playerDataLoader.GameDatas.Count - 1)
            {
                return 0;
            }

            return _playerDataLoader.GameDatas[index].Score;
        }
    }
}