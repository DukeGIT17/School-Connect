﻿using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_RepositoryLayer.Interfaces
{
    public interface IParent
    {
        Task<Dictionary<string, object>> CreateAsync(Parent parent);
        Task<Dictionary<string, object>> GetById(long parentId);
        Task<Dictionary<string, object>> GetByIdNo(long parentIdNo);
    }
}