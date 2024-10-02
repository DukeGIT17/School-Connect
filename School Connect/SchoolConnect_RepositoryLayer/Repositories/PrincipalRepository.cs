using SchoolConnect_DomainLayer.Data;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_RepositoryLayer.CommonAction;
using SchoolConnect_RepositoryLayer.Interfaces;

namespace SchoolConnect_RepositoryLayer.Repositories
{
    public class PrincipalRepository : IPrincipal
    {
        private readonly SchoolConnectDbContext _context;
        private readonly ISignInRepo _signInRepo;
        private Dictionary<string, object> _returnDictionary;

        public PrincipalRepository(SchoolConnectDbContext context, ISignInRepo signInRepo)
        {
            _context = context;
            _signInRepo = signInRepo;
            _returnDictionary = [];
        }

        public async Task<Dictionary<string, object>> Create(Principal principal)
        {
            try
            {
                var result = _context.Principals.FirstOrDefault(p => p.StaffNr == principal.StaffNr);
                if (result != null)
                    throw new Exception($"A principal with the staff number '{principal.StaffNr}' already exists within the database.");

                var school = _context.Schools.FirstOrDefault(s => s.Id == principal.SchoolID);
                if (school == null)
                    throw new Exception($"School with the ID '{principal.SchoolID}' could not be found.");

                principal.PrincipalSchoolNP = school!;

                _returnDictionary = await _signInRepo.CreateUserAccountAsync(principal.EmailAddress, principal.Role, principal.PhoneNumber.ToString());
                if (!(bool)_returnDictionary["Success"]) throw new Exception(_returnDictionary["ErrorMessage"] as string);

                await _context.AddAsync(principal);
                await _context.SaveChangesAsync();

                _returnDictionary["Success"] = true;
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message + "\ninner exception: " + ex.InnerException;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> GetById(long principalId)
        {
            try
            {
                return await CommonActions.GetActorById(principalId, new Principal(), _context);
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = true;
                _returnDictionary["ErrorMessage"] = ex.Message;
                return _returnDictionary;
            }
        }

        public Task<Dictionary<string, object>> GetByStaffNr(long principalStaffNr)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, object>> Remove(long principalId, long? staffNr = -1)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, object>> Update(Principal principal)
        {
            throw new NotImplementedException();
        }
    }
}
