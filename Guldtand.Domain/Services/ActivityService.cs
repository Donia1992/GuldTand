using AutoMapper;
using Guldtand.Data;
using Guldtand.Data.Entities;
using Guldtand.Domain.Helpers;
using Guldtand.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace Guldtand.Domain.Services
{
    public class ActivityService : IActivityService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IDateTimeProvider _dateTime;

        public ActivityService(DataContext context, IMapper mapper, IDateTimeProvider dateTime)
        {
            _context = context;
            _mapper = mapper;
            _dateTime = dateTime;
        }

        public ActivityDTO Create(ActivityDTO activityDto)
        {
            var activity = _mapper.Map<Activity>(activityDto);

            if (!_context.Users.Any(x => x.Id == activityDto.UserId))
                throw new AppException($"No User with Id {activityDto.UserId} in database.");

            if (!_context.Customers.Any(x => x.Id == activityDto.CustomerId))
                throw new AppException($"No Customer with Id {activityDto.CustomerId} in database.");

            if (!_context.ActivityTypes.Any(x => x.Id == activityDto.TypeId))
                throw new AppException($"No ActivityType with Id {activityDto.TypeId} in database.");

            _context.Activities.Add(activity);
            _context.SaveChanges();

            return _mapper.Map<ActivityDTO>(activity);
        }

        public IEnumerable<ActivityDTO> GetAll(int customerId, int userId)
        {
            var activities = _context.Activities.UserMatch(userId).CustomerMatch(customerId);

            return _mapper.Map<IEnumerable<ActivityDTO>>(activities);
        }

        public IEnumerable<ActivityDTO> GetAllFuture(int customerId, int userId)
        {
            var activities = _context.Activities.UserMatch(userId).CustomerMatch(customerId).AsOf(_dateTime.Today());

            return _mapper.Map<IEnumerable<ActivityDTO>>(activities);
        }

        public ActivityDTO GetById(int id)
        {
            var activity = _context.Activities.Find(id);
            return _mapper.Map<ActivityDTO>(activity);
        }

        public void Update(ActivityDTO activityDto, string password = null)
        {

            var activity = _context.Activities.Find(activityDto.Id);

            if (activity == null)
                throw new AppException("Activity not found");

            _context.Activities.Update(activity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var activity = _context.Activities.Find(id);
            if (activity != null)
            {
                _context.Activities.Remove(activity);
                _context.SaveChanges();
            }
        }
    }
}

