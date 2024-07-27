﻿using BlogApp.Data.Abstract;
using BlogApp.Entity;

namespace BlogApp.Data.Concrete.EfCore
{

    public class EfUserRepository : IUserRepository
    {
        public BlogContext _context;
        public EfUserRepository(BlogContext context)
        {
            _context = context;
        }
        public IQueryable<User> Users => _context.Users;

        public void CreateUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
    }
}