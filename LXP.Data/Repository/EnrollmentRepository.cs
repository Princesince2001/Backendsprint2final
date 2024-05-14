using LXP.Common.Entities;
using LXP.Data.DBContexts;
using LXP.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Data.Repository
{
   
    
        public class EnrollmentRepository : IEnrollmentRepository
        {
            private readonly LXPDbContext _lXPDbContext;

            public EnrollmentRepository(LXPDbContext lXPDbContext)
            {
               this. _lXPDbContext = lXPDbContext;
            }

            public async Task  Addenroll(Enrollment enrollment)
            {
                await _lXPDbContext.Enrollments.AddAsync(enrollment);
                await _lXPDbContext.SaveChangesAsync();

            }

            public bool AnyEnrollmentByLearnerAndCourse(Guid LearnerId, Guid CourseId )
            {
                return _lXPDbContext.Enrollments.Any(enrollment => enrollment.LearnerId == LearnerId && enrollment.CourseId == CourseId);
            }
        }
    }

