using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LXP.Common.Entities;

namespace LXP.Data.IRepository
{
    public interface ILearnerRepository
    {
        void AddLearner(Learner learner);
        Task<bool> AnyLearnerByEmail(string email);
        Learner GetLearnerByLearnerEmail(string email);

        Task<List<Learner>> GetAllLearner();

        //Task UpdateAllLearner(Learner learner);

        Learner GetLearnerDetailsByLearnerId(Guid LearnerId);

        Task UpdateLearner(Learner learner);
    }
}
