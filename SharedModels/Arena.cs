using System.ComponentModel.DataAnnotations.Schema;

namespace SharedModels
{
    public class Arena
    {
        public string Id { get; set; }
        public string Name { get; set; }
        
        public string Creator { get; set; }

        public string Opponent { get; set; }
        [NotMapped]
        public Challenge CurrentChallenge { get; set; }

        public string ChallengeName { get; set; }
        

        public bool IsFull => !string.IsNullOrEmpty(Creator) && !string.IsNullOrEmpty(Opponent);
    }
}