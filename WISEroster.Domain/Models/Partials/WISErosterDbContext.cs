using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WISEroster.Domain.Models
{
    public partial interface IWISErosterDbContext
    {
        Task<int> SecureItems_DeleteAsync(string itemKey);
        Task<int> SecureItems_UpsertAsync(string itemKey, string itemValue);
    }

    public partial class WISErosterDbContext
    {
        public async Task<int> SecureItems_DeleteAsync(string itemKey)
        {
            var itemKeyParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@ItemKey", SqlDbType = System.Data.SqlDbType.NVarChar, Direction = System.Data.ParameterDirection.Input, Value = itemKey, Size = 100 };
            if (itemKeyParam.Value == null)
                itemKeyParam.Value = System.DBNull.Value;

            return await Database.ExecuteSqlCommandAsync(System.Data.Entity.TransactionalBehavior.DoNotEnsureTransaction, "EXEC [sec].[SecureItems_Delete] @ItemKey", itemKeyParam);

        }

        public async Task<int> SecureItems_UpsertAsync(string itemKey, string itemValue)
        {
            var kl = itemKey.Length;
            var vl = itemValue.Length;

            var itemKeyParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@ItemKey", SqlDbType = System.Data.SqlDbType.NVarChar, Direction = System.Data.ParameterDirection.Input, Value = itemKey, Size = 100 };
            if (itemKeyParam.Value == null)
                itemKeyParam.Value = System.DBNull.Value;

            var itemValueParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@ItemValue", SqlDbType = System.Data.SqlDbType.NVarChar, Direction = System.Data.ParameterDirection.Input, Value = itemValue, Size = 1024 };
            if (itemValueParam.Value == null)
                itemValueParam.Value = System.DBNull.Value;


            return await Database.ExecuteSqlCommandAsync(System.Data.Entity.TransactionalBehavior.DoNotEnsureTransaction, "EXEC [sec].[SecureItems_Upsert] @ItemKey, @ItemValue", itemKeyParam, itemValueParam);


        }

    }
}
