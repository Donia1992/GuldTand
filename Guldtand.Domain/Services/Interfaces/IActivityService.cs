using System.Collections.Generic;
using Guldtand.Domain.Models;

namespace Guldtand.Domain.Services
{
    public interface IActivityService
    {
        ActivityDTO Create(ActivityDTO activityDto);
        void Delete(int id);
        IEnumerable<ActivityDTO> GetAll(string customerId, string userId);
        IEnumerable<ActivityDTO> GetAllFuture(string customerId, string userId);
        ActivityDTO GetById(int id);
        void Update(ActivityDTO activityDto, string password = null);
    }
}