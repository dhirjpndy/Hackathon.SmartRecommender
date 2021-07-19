using Hackathon.SmartRecommender.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon.SmartRecommender.Domain.Providers
{
    public interface IAIMLReccomenderApiProvider
    {
        Task<List<AIMLReccomenderDetails>> GetTimeReccommenders(double studioId, string category);
    }
}
