﻿using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_RepositoryLayer.Interfaces
{
    public interface IPrincipal
    {
        Task<Dictionary<string, object>> Update(Principal principal);
        Task<Dictionary<string, object>> GetById(long principalId);
        Task<Dictionary<string, object>> GetByStaffNr(long principalStaffNr);
        Task<Dictionary<string, object>> Create(Principal principal);
        Task<Dictionary<string, object>> Remove(long principalId, long staffNr = -1);
    }
}