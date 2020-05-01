using Core.Interfaces;
using Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Services
{
    public class UserService : IUserRepository
    {
        private ShoppingPortalEntities _context;

        public UserService(ShoppingPortalEntities context)
        {
            this._context = context;
        }

        public List<UserDetail> GetUsers()
        {
            List<UserDetail> userDetails = null;
            try
            {
                userDetails = _context.UserDetails.ToList();
            }
            catch (Exception)
            {

            }
            return userDetails;
        }

        public UserDetail GetUser(string userId)
        {
            UserDetail result = null;
            if (userId != "")
            {

                result = _context.UserDetails.Where(x => x.UserId == userId).FirstOrDefault();
            }
            return result;
        }

        public bool Delete(string userId)
        {
            UserDetail userExists = null;
            try
            {
                if (userId != "")
                {
                    userExists = GetUser(userId);

                    if (userExists != null)
                    {
                        _context.UserDetails.Remove(userExists);

                        if (_context.SaveChanges() > 0)
                            return true;
                    }
                }
            }
            catch (Exception)
            {

            }
            return false;
        }

        public UserDetail AddOrUpdate(UserDetail userDetail)
        {
            UserDetail userExists = null;
            try
            {
                if (userDetail != null)
                {
                    if (userDetail.UserId != null && userDetail.UserId != "")
                    {
                        userExists = GetUser(userDetail.UserId);

                        if (userExists != null)
                        {
                            userExists.UserId = userDetail.UserId;
                            userExists.FirstName = userDetail.FirstName;
                            userExists.LastName = userDetail.LastName;
                            userExists.MiddleName = userDetail.MiddleName;
                            //userExists.PhoneNumber = userDetail.PhoneNumber;
                            //userExists.Email = userDetail.Email;
                            userExists.IsActive = userDetail.IsActive;
                            //userDetail.ModifiedDate = DateTime.UtcNow();

                            if (_context.SaveChanges() > 0)
                                return userExists;
                        }
                        else
                        {
                            //userDetail.CreatedDate = DateTime.Now();
                            _context.UserDetails.Add(userDetail);

                            if (_context.SaveChanges() > 0)
                                return userDetail;
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
            return userExists;
        }

        public bool AddUsers(List<UserDetail> users)
        {
            try
            {
                if (users.Count > 0)
                {
                    _context.UserDetails.AddRange(users);

                    if (_context.SaveChanges() > 0)
                        return true;
                }
            }
            catch (Exception)
            {

            }
            return false;
        }

        public bool UpdateStatus(string userId, bool status)
        {
            UserDetail userExists = null;
            try
            {
                if (userId != "")
                {
                    userExists = GetUser(userId);

                    if (userExists != null)
                    {
                        userExists.IsActive = status;

                        if (_context.SaveChanges() > 0)
                            return true;
                    }
                }
            }
            catch (Exception)
            {

            }
            return false;
        }
    }

    
}
