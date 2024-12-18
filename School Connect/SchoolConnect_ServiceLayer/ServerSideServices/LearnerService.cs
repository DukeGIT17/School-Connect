﻿using Microsoft.AspNetCore.Http;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_RepositoryLayer.CommonAction;
using SchoolConnect_RepositoryLayer.Interfaces;
using SchoolConnect_ServiceLayer.IServerSideServices;

namespace SchoolConnect_ServiceLayer.ServerSideServices
{
    public class LearnerService : ILearnerService
    {
        private readonly ILearner _learnerRepo;
        private Dictionary<string, object> _returnDictionary;

        public LearnerService(ILearner learnerRepo)
        {
            _learnerRepo = learnerRepo;
            _returnDictionary = [];
        }

        public async Task<Dictionary<string, object>> LoadLearnersAsync(IFormFile file, long schoolId)
        {
            try
            {
                return await _learnerRepo.BatchLoadLearnersFromExcelAsync(file, schoolId);
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> CreateAsync(Learner learner)
        {
            try
            {
                _returnDictionary = CommonActions.AttemptObjectValidation(learner);
                if (!(bool)_returnDictionary["Success"])
                    return _returnDictionary;

                foreach (var parent in learner.Parents)
                {
                    if (parent.Parent != null)
                    {
                        _returnDictionary = CommonActions.AttemptObjectValidation(parent.Parent);
                        if (!(bool)_returnDictionary["Success"])
                            return _returnDictionary;
                    }
                }

                _returnDictionary = await _learnerRepo.CreateAsync(learner);
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetLearnerByIdAsync(long id)
        {
            try
            {
                return await _learnerRepo.GetLearnerByIdAsync(id);
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetLearnerByIdNoAsync(string idNo)
        {
            try
            {
                return await _learnerRepo.GetLearnerByIdNoAsync(idNo);
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = true;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> UpdateAsync(Learner learner)
        {
            try
            {
                return await _learnerRepo.UpdateAsync(learner);
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetLearnersByMainTeacherAsync(long teacherId)
        {
            try
            {
                return await _learnerRepo.GetLearnersByMainTeacherAsync(teacherId);
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetLearnersByClassIdAsync(int classId)
        {
            try
            {
                return await _learnerRepo.GetLearnersByClassID(classId);
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }
    }
}
