using System.Collections.Generic;

namespace _Assets.Scripts.Services
{
    public class ContinueData
    {
        public int SongIndex;
        public List<FruitContinueData> FruitsContinueData;
        public int CurrentFruitIndex;
        public int NextFruitIndex;
        public int Score;

        public ContinueData(int songIndex, List<FruitContinueData> fruitsContinueData, int currentFruitIndex, int nextFruitIndex, int score)
        {
            SongIndex = songIndex;
            FruitsContinueData = fruitsContinueData;
            CurrentFruitIndex = currentFruitIndex;
            NextFruitIndex = nextFruitIndex;
            Score = score;
        }

        public struct FruitContinueData
        {
            public int Index;
            public float PositionX, PositionY;

            public FruitContinueData(int index, float positionX, float positionY)
            {
                Index = index;
                PositionX = positionX;
                PositionY = positionY;
            }
        }
    }
}