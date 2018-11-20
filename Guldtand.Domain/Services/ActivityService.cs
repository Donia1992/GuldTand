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
                throw new AppException($"No Customer with Id {activityDto.UserId} in database.");

            if (!_context.ActivityTypes.Any(x => x.Id == activityDto.TypeId))
                throw new AppException($"No ActivityType with Id {activityDto.UserId} in database.");

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

            if (!string.IsNullOrEmpty(customerId) || !string.IsNullOrEmpty(userId))
            {
                (var role, var id) = GetRoleAndId(customerId, userId);

                return GetAllByRoleAndId(role, id);
            }
            else
            {
                return _mapper.Map<IEnumerable<ActivityDTO>>(_context.Activities);
            }
        }

        public IEnumerable<ActivityDTO> GetAllFuture(string customerId, string userId)
        {
            if (!string.IsNullOrEmpty(customerId) || !string.IsNullOrEmpty(userId))
            {
                (var role, var id) = GetRoleAndId(customerId, userId);

                return GetAllByRoleAndIdFuture(role, id);
            }
            else
            {
                return _mapper.Map<IEnumerable<ActivityDTO>>(_context.Activities.AsOf(_dateTime.Today()));
            }
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

        private IEnumerable<ActivityDTO> GetAllByRoleAndId(string role, int id)
        {
            if (role == "user")
                return GetAllForUser(id);

            else if (role == "customer")
                return GetAllForCustomer(id);

            else throw new AppException($"{nameof(role)} {role} invalid.");
        }

        private IEnumerable<ActivityDTO> GetAllByRoleAndIdFuture(string role, int id)
        {
            if (role == "user")
                return GetAllForUserFuture(id);

            else if (role == "customer")
                return GetAllForCustomerFuture(id);

            else throw new AppException($"{nameof(role)} {role} invalid.");
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
            else
            {
                throw new ArgumentException($"CustomerId: {customerId} or UserId: {userId} could not be parsed to int.");
            }            
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
            else
            {
                throw new ArgumentException($"CustomerId: {customerId} or UserId: {userId} could not be parsed to int.");
            }
        }

        private IEnumerable<ActivityDTO> GetAllForCustomer(int customerId)
        {
            var activities = _context.Activities.Include("ActivityType").CustomerMatch(customerId);
            return _mapper.Map<IEnumerable<ActivityDTO>>(activities);
        }

        private IEnumerable<ActivityDTO> GetAllForCustomerFuture(int customerId)
        {
            var activities = _context.Activities.Include("ActivityType").CustomerMatch(customerId).AsOf(_dateTime.Today());
            return _mapper.Map<IEnumerable<ActivityDTO>>(activities);
        }

        private IEnumerable<ActivityDTO> GetAllForUser(int dentistId)
        {
            var activities = _context.Activities.Include("ActivityType").UserMatch(dentistId);
            return _mapper.Map<IEnumerable<ActivityDTO>>(activities);
        }

        private IEnumerable<ActivityDTO> GetAllForUserFuture(int dentistId)
        {
            var activities = _context.Activities.Include("ActivityType").UserMatch(dentistId).AsOf(_dateTime.Today());
            return _mapper.Map<IEnumerable<ActivityDTO>>(activities);
        }

        private (string, int) GetRoleAndId(string customerId, string userId)
        {
            if (!string.IsNullOrEmpty(customerId))
            {
                if (int.TryParse(customerId, out int id))
                {
                    return ("customer", id);
                }
                else
                {
                    throw new ArgumentException($"CustomerId {id} not parseable to int.");
                }
            }

            else
            {
                if (int.TryParse(userId, out int id))
                {
                    return ("user", id);
                }
                else
                {
                    throw new ArgumentException($"UserId {id} not parseable to int.");
                }
            }
        }
    }
}

