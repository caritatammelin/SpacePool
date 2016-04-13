using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpacePool
{
   

    class PlayerScore
    {
        public string Name { get; set; }
        public int Score { get; set; }

        public ObservableCollection<PlayerScore> PlayScores { get; }

        public PlayerScore()
        {
            PlayScores = new ObservableCollection<PlayerScore>();
        }

        public void AddScore(PlayerScore score)
        {
            PlayScores.Add(score);
        }

    }
}
