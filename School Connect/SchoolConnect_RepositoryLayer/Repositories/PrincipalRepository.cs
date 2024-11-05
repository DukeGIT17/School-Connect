using Microsoft.EntityFrameworkCore;
using SchoolConnect_DomainLayer.Data;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_RepositoryLayer.Interfaces;
using static SchoolConnect_RepositoryLayer.CommonAction.CommonActions;

namespace SchoolConnect_RepositoryLayer.Repositories
{
    public class PrincipalRepository : IPrincipalRepo
    {
        private readonly SchoolConnectDbContext _context;
        private readonly ISignInRepo _signInRepo;
        private readonly IGroupRepo _groupRepo;
        private Dictionary<string, object> _returnDictionary;

        public PrincipalRepository(SchoolConnectDbContext context, ISignInRepo signInRepo, IGroupRepo groupRepo)
        {
            _context = context;
            _signInRepo = signInRepo;
            _groupRepo = groupRepo;
            _returnDictionary = [];
        }

        public async Task<Dictionary<string, object>> CreateAsync(Principal principal)
        {
            try
            {
                var result = _context.Principals.FirstOrDefault(p => p.StaffNr == principal.StaffNr);
                if (result != null)
                    throw new($"A principal with the staff number '{principal.StaffNr}' already exists within the database.");

                var school = _context.Schools.FirstOrDefault(s => s.Id == principal.SchoolID);
                if (school == null)
                    throw new($"School with the ID '{principal.SchoolID}' could not be found.");

                principal.PrincipalSchoolNP = school!;

                _returnDictionary = await _groupRepo.AddToGroup(principal.StaffNr, principal.SchoolID, "All");
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);

                _returnDictionary = await _signInRepo.CreateUserAccountAsync(principal.EmailAddress, principal.Role, principal.PhoneNumber.ToString());
                if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);

                if (principal.ProfileImageFile is not null)
                {
                    _returnDictionary = SaveFile($"{principal.Name} {principal.Surname} - {principal.PrincipalSchoolNP.Name}", "Profile Images Folder/Principals", principal.ProfileImageFile);
                    if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                    principal.ProfileImage = _returnDictionary["FileName"] as string;
                }
                else
                    principal.ProfileImage = "Default Pic.jpeg";

                await _context.AddAsync(principal);
                await _context.SaveChangesAsync();

                _returnDictionary["Success"] = true;
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message + "\nInner exception: " + ex.InnerException;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetById(long principalId)
        {
            try
            {
                var principal = await _context.Principals.Include(s => s.PrincipalSchoolNP).ThenInclude(a => a.SchoolGroupsNP).FirstOrDefaultAsync(p => p.Id == principalId);
                if (principal == null) throw new("Could not find a principal with the provided ID");

                _returnDictionary["Success"] = true;
                _returnDictionary["Result"] = principal;
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public Task<Dictionary<string, object>> GetByStaffNr(string principalStaffNr)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, object>> Remove(long principalId)
        {
            throw new NotImplementedException();
        }

        public async Task<Dictionary<string, object>> UpdateAsync(Principal principal)
        {
            try
            {
                var existingPrincipal = await _context.Principals.FirstOrDefaultAsync(p => p.Id == principal.Id);
                if (existingPrincipal == null) throw new("Could not find a principal with the specified ID.");

                if (principal.EmailAddress != existingPrincipal.EmailAddress)
                {
                    _returnDictionary = await _signInRepo.ChangeEmailAddressAsync(existingPrincipal.EmailAddress, principal.EmailAddress);
                    if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                }

                if (principal.PhoneNumber != existingPrincipal.PhoneNumber)
                {
                    _returnDictionary = await _signInRepo.ChangePhoneNumberAsync(existingPrincipal.PhoneNumber.ToString(), principal.PhoneNumber.ToString(), principal.EmailAddress);
                    if (!(bool)_returnDictionary["Success"]) throw new(_returnDictionary["ErrorMessage"] as string);
                }

                existingPrincipal.EmailAddress = principal.EmailAddress;
                existingPrincipal.PhoneNumber = principal.PhoneNumber;

                await _context.SaveChangesAsync();

                _returnDictionary["Success"] = true;
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message + "\nInner Exception: " + ex.InnerException;
                return _returnDictionary;
            }
        }
    }
}
