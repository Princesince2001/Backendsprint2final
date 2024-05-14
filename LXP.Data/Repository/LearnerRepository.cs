using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LXP.Data.DBContexts;
using LXP.Data.IRepository;
using System.Threading.Tasks;
using LXP.Common.Entities;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace LXP.Data.Repository
{
    public class LearnerRepository : ILearnerRepository
    {
        private readonly LXPDbContext _lXPDbContext;
        public LearnerRepository(LXPDbContext context) {

            _lXPDbContext = context;
        }

        public void AddLearner(Learner learner) {

            _lXPDbContext.Learners.Add(learner);


            _lXPDbContext.SaveChanges();


        }
        //public Task<bool> AnyLearnerByEmail(string email)
        //{
        //    return _lXPDbContext.Learners.AnyAsync(learner=>learner.Email==email);
        //}



        public async Task<bool> AnyLearnerByEmail(string email)
        {
            return  _lXPDbContext.Learners.Any(l => l.Email == email);
        }


        public Learner GetLearnerByLearnerEmail(string email)
        {
            return _lXPDbContext.Learners.FirstOrDefault(learner => learner.Email == email);
        }

        public async Task<List<Learner>> GetAllLearner()
        {
            return _lXPDbContext.Learners.ToList();
        }

        public Learner GetLearnerDetailsByLearnerId(Guid LearnerId)

        {

            return _lXPDbContext.Learners.Find(LearnerId);


        }

        public async Task UpdateLearner(Learner learner)
        {
            _lXPDbContext.Learners.Update(learner);
            await _lXPDbContext.SaveChangesAsync();
        }


    }

}

