using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using VContainer;

namespace _Assets.Scripts.Services.UIs
{
    public class LeaderBoardUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI[] scores;
        [Inject] private LeaderBoardService _leaderBoardService;

        private void Start()
        {
            var list = new List<int>(5);
            for (int i = 0; i < scores.Length; i++)
            {
                if (_leaderBoardService.GetScore(i) != 0)
                {
                    list.Add(_leaderBoardService.GetScore(i));
                }
                else
                {
                    scores[i].text = string.Empty;
                }
            }

            var sortedList = list.OrderByDescending(i1 => i1).Distinct().ToList();

            for (int i = 0; i < sortedList.Count; i++)
            {
                scores[i].text = sortedList[i].ToString();
            }
        }
    }
}