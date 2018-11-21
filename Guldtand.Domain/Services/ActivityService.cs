using AutoMapper;
using Guldtand.Data;
using Guldtand.Data.Entities;
using Guldtand.Domain.Helpers;
using Guldtand.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
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

        public IEnumerable<ActivityDTO> GetAll(string customerId, string userId)
        {
            if (!string.IsNullOrEmpty(customerId) && !string.IsNullOrEmpty(userId))
            {
                return GetAllForCustomerAndUser(customerId, userId);
            }

            if (!string.IsNullOrEmpty(customerId))
            {
                return GetAllForCustomer(customerId);
            }

            if (!string.IsNullOrEmpty(userId))
            {
                return GetAllForUser(userId);
            }

            return _mapper.Map<IEnumerable<ActivityDTO>>(_context.Activities);
        }

        public IEnumerable<ActivityDTO> GetAllFuture(string customerId, string userId)
        {
            if (!string.IsNullOrEmpty(customerId) && !string.IsNullOrEmpty(userId))
            {
                return GetAllForCustomerAndUserFuture(customerId, userId);
            }

            if (!string.IsNullOrEmpty(customerId))
            {
                return GetAllForCustomerFuture(customerId);
            }

            if (!string.IsNullOrEmpty(userId))
            {
                return GetAllForUserFuture(userId);
            }

            return _mapper.Map<IEnumerable<ActivityDTO>>(_context.Activities.AsOf(_dateTime.Today()));
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

        private IEnumerable<ActivityDTO> GetAllForCustomerAndUser(string customerId, string userId)
        {
            var couldParseCustomerId = int.TryParse(customerId, out int parsedCustomerId);
            var couldParseUserId = int.TryParse(userId, out int parsedUserId);

            if (couldParseCustomerId && couldParseCustomerId)
            {
                var activities = _context.Activities.UserMatch(parsedUserId).CustomerMatch(parsedCustomerId);
                return _mapper.Map<IEnumerable<ActivityDTO>>(activities);
            }

            throw new ArgumentException($"{nameof(customerId)}: {customerId} or {nameof(userId)}: {userId} could not be parsed to int.");

        }

        private IEnumerable<ActivityDTO> GetAllForCustomerAndUserFuture(string customerId, string userId)
        {
            var couldParseCustomerId = int.TryParse(customerId, out int parsedCustomerId);
            var couldParseUserId = int.TryParse(userId, out int parsedUserId);

            if (couldParseCustomerId && couldParseCustomerId)
            {
                var activities = _context.Activities.UserMatch(parsedUserId).CustomerMatch(parsedCustomerId).AsOf(_dateTime.Today());
                return _mapper.Map<IEnumerable<ActivityDTO>>(activities);
            }

            throw new ArgumentException($"{nameof(customerId)}: {customerId} or {nameof(userId)}: {userId} could not be parsed to int.");
        }

        private IEnumerable<ActivityDTO> GetAllForCustomer(string customerId)
        {
            if (int.TryParse(customerId, out int id))
            {
                var activities = _context.Activities.Include("ActivityType").CustomerMatch(id);
                return _mapper.Map<IEnumerable<ActivityDTO>>(activities);
            }

            throw new ArgumentException($"{nameof(customerId)} {customerId} could not be parsed to int.");
        }

        private IEnumerable<ActivityDTO> GetAllForCustomerFuture(string customerId)
        {
            if (int.TryParse(customerId, out int id))
            {
                var activities = _context.Activities.Include("ActivityType").CustomerMatch(id).AsOf(_dateTime.Today());
                return _mapper.Map<IEnumerable<ActivityDTO>>(activities);
            }

            throw new ArgumentException($"{nameof(customerId)} {customerId} could not be parsed to int.");
        }

        private IEnumerable<ActivityDTO> GetAllForUser(string userId)
        {
            if (int.TryParse(userId, out int id))
            {
                var activities = _context.Activities.Include("ActivityType").CustomerMatch(id);
                return _mapper.Map<IEnumerable<ActivityDTO>>(activities);
            }

            throw new ArgumentException($"{nameof(userId)} {userId} could not be parsed to int.");
        }

        private IEnumerable<ActivityDTO> GetAllForUserFuture(string userId)
        {
            if (int.TryParse(userId, out int id))
            {
                var activities = _context.Activities.Include("ActivityType").CustomerMatch(id).AsOf(_dateTime.Today());
                return _mapper.Map<IEnumerable<ActivityDTO>>(activities);
            }

            throw new ArgumentException($"{nameof(userId)} {userId} could not be parsed to int.");
        }
    }
}

