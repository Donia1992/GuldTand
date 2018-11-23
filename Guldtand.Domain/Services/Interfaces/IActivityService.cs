using System.Collections.Generic;
using Guldtand.Domain.Models;

namespace Guldtand.Domain.Services
{
    public interface IActivityService
    {
        ActivityDTO Create(ActivityDTO activityDto);
        void Delete(int id);
        IEnumerable<ActivityDTO> GetAll(int customerId, int userId);
        IEnumerable<ActivityDTO> GetAllFuture(int customerId, int userId);
        ActivityDTO GetById(int id);
        void Update(ActivityDTO activityDto, string password = null);
    }
}