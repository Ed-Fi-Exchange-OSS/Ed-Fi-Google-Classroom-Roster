using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WISEroster.Business.Models;
using WISEroster.Domain.Models;

namespace WISEroster.Business
{
    public interface ISetupBusiness
    {
        OrgGcPreference GetGcPreference(int edOrgId);
        void UpsertGcPreference(OrgGcPreference orgGc);
        ClientCredentials GetClientCredentials(int edOrgId);
        string GetClientEmail(int edOrgId);
        void UpsertClientCredentials(ClientCredentials creds);
       
        Task ClearSecureItemsForDistrict(string prefix, string email);
        Task<int> DeleteSecureItem(string key);
        List<SecureItems_ReadReturnModel> GetSecureItemValue(string key);
        Task UpsertSecureItemAsync(string key, string value);
    }

    public class SetupBusiness : ISetupBusiness
    {
        private readonly IWISErosterDbContext _context;

        public SetupBusiness(IWISErosterDbContext context)
        {
            _context = context;
        }

        public OrgGcPreference GetGcPreference(int edOrgId)
        {
            return _context.OrgGcPreferences.FirstOrDefault(u => u.EducationOrganizationId == edOrgId);

        }

        public void UpsertGcPreference(OrgGcPreference orgGc)
        {
            var existing = _context.OrgGcPreferences.FirstOrDefault(u => u.EducationOrganizationId == orgGc.EducationOrganizationId);
            if (existing != null)
            {
                existing.GcUserEmail = orgGc.GcUserEmail;
                existing.AllowExternalDomains = orgGc.AllowExternalDomains;
            }
            else
            {
                _context.OrgGcPreferences.Add(orgGc);
            }

            _context.SaveChanges();
        }

        public ClientCredentials GetClientCredentials(int edOrgId)
        {
            var clientItemKey = FormatItemKey(edOrgId,
                SecureItemType.ClientId);
            var key = _context.SecureItems_Read(clientItemKey).FirstOrDefault();

            var secretItemKey = FormatItemKey(edOrgId,
                SecureItemType.ClientSecret);
            var secret = _context.SecureItems_Read(secretItemKey).FirstOrDefault();

            return new ClientCredentials
            {
                EducationOrganizationId = edOrgId,
                ClientId = key?.ItemValue,
                ClientSecret = secret?.ItemValue
            };
        }

        public string GetClientEmail(int edOrgId)
        {
            var email = _context.OrgGcPreferences.Where(u => u.EducationOrganizationId == edOrgId).Select(u=>u.GcUserEmail).FirstOrDefault();
            return email;
        }

        public void UpsertClientCredentials(ClientCredentials creds)
        {
            var clientItemKey = FormatItemKey(creds.EducationOrganizationId,
                SecureItemType.ClientId);
            _context.SecureItems_Upsert(clientItemKey, creds.ClientId);

            var secretItemKey = FormatItemKey(creds.EducationOrganizationId,
                SecureItemType.ClientSecret);
            _context.SecureItems_Upsert(secretItemKey, creds.ClientSecret);

        }

        private string FormatItemKey(int edOrgId, SecureItemType type)
        {
            return string.Format("{0}_{1}", edOrgId.ToString(), type.ToString());
        }

        #region GCIDataStore
        public async Task<int> DeleteSecureItem(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Key MUST have a value");
            }
           return await _context.SecureItems_DeleteAsync(key);

        }

        public List<SecureItems_ReadReturnModel> GetSecureItemValue(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Key MUST have a value");
            }

           return _context.SecureItems_Read(key);
   
        }

        public async Task UpsertSecureItemAsync(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Key MUST have a value");
            }

            await _context.SecureItems_UpsertAsync(key, value);
        }

        public async Task ClearSecureItemsForDistrict(string prefix, string email)
        {
            if (_context.SecureItems.Any(s => s.ItemKey.StartsWith(prefix)))
            {
                _context.SecureItems.RemoveRange(_context.SecureItems.Where(s => s.ItemKey.StartsWith(prefix) || s.ItemKey.Contains(email)));
                await _context.SaveChangesAsync();
            }

        }
        #endregion


    }
}
