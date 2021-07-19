using System;
using System.Collections.Generic;
using System.Text;

namespace Hackathon.SmartRecommender.Domain.Models
{
    public class AIMLReccomenderDetails
    {
        public double Studio_Id { get; set; }

        public string Category { get; set; }

        public List<Time_Slot_Insights> Time_Slot_Insights { get; set; }
    }

    public class Time_Slot_Insights
    {
        public DateTime Time_Slot_Start { get; set; }
        public DateTime Time_Slot_End { get; set; }

        public int Rank { get; set; }
    }
}
