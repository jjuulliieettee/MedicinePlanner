﻿using MedicinePlanner.Data.Models;
using System;
using System.Threading.Tasks;

namespace MedicinePlanner.Core.Repositories.Interfaces
{
    public interface IUserRepo
    {
        Task<User> GetById(Guid id);
        Task<User> GetByEmail(string email);
        User Add(User user);
    }
}
