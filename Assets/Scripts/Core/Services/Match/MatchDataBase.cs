using UnityEngine;

namespace Core.Services.Match
{
    public class MatchDataBase : IMatchData
    {
        public void Init()
        {
           Debug.Log("MatchDataBase.Init");
        }

        public void UpdateRound()
        {
            Debug.Log("MatchDataBase.UpdateRound");
        }
    } 
}