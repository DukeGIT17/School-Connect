﻿using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_Web_App.IServices
{
    public interface ISchoolService
    {
        Task<Dictionary<string, object>> RegisterSchool(School school);
        Task<Dictionary<string, object>> GetSchools();
    }
}