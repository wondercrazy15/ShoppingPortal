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
    }
}
