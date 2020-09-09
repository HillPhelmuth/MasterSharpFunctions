using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ChallengeFunction.Models
{
    [Serializable]
    public class UserAppData
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ChallengeSuccessData { get; set; }
        public virtual List<UserSnippet> Snippets { get; set; }
        public virtual List<ArenaDuel> CompletedDuels { get; set; }
        [NotMapped]
        public List<int> ChallengeSuccessIds
        {
            get
            {
                var idList = ChallengeSuccessData?.Split(',').ToList();
                var list = new List<int>();
                foreach (var id in idList ?? new List<string>())
                {
                    var didParse = int.TryParse(id, out int val);
                    if (didParse) list.Add(val);
                }

                return list;
            }
            set => ChallengeSuccessData = string.Join(',', value);
        }
    }

    public class UserSnippet
    {
        public int ID { get; set; }
        public int UserAppDataID { get; set; }
        public string Name { get; set; }
        public string Snippet { get; set; }
    }
}
