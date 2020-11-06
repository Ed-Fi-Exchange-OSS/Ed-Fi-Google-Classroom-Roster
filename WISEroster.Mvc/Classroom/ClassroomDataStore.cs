using Google.Apis.Util.Store;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using WISEroster.Business;

namespace WISEroster.Mvc.Classroom
{
    public class ClassroomDataStore : IDataStore
    { 
        private readonly ISetupBusiness _setupBusiness;
        public ClassroomDataStore(ISetupBusiness setupBusiness)
        {
             _setupBusiness = setupBusiness;
        }
        public async Task StoreAsync<T>(string key, T value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Key MUST have a value");
            }
          
            var generatedKey = FormatItemKey(key, typeof(T));
            string json = JsonConvert.SerializeObject(value);

            await _setupBusiness.UpsertSecureItemAsync(generatedKey, json);
        }

        public async Task DeleteAsync<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Key MUST have a value");
            }
           
            var generatedKey = FormatItemKey(key, typeof(T));
            await _setupBusiness.DeleteSecureItem(generatedKey);
        }

        public Task<T> GetAsync<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Key MUST have a value");
            }
          
            var generatedKey = FormatItemKey(key, typeof(T));
         
            var item = _setupBusiness.GetSecureItemValue(generatedKey).FirstOrDefault();
            T value = item == null ? default(T) : JsonConvert.DeserializeObject<T>(item.ItemValue);
            return Task.FromResult<T>(value);
          
        }

        public async Task ClearAsync()
        {
            throw new NotImplementedException();
        }

        private string FormatItemKey(string key, Type t)
        {
            return string.Format("{0}-{1}", key, t.FullName);
        }

       
    }
}