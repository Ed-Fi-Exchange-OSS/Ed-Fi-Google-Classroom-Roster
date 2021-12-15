using System.Configuration;
using System.Text.RegularExpressions;

namespace WISEroster.Domain.Api
{
    public interface IV3ApiDbContext: IEdfiApiV3DbContext
    {
        void ChangeSchoolYear(short schoolYear);
    }
    public partial class V3ApiDbContext: IV3ApiDbContext
    {
        public void ChangeSchoolYear(short schoolYear)
        {
            var connString = ConfigurationManager.ConnectionStrings["ApiV3DbContext"].ConnectionString;
            connString = Regex.Replace(connString, @"EdFi_Ods_\d{4,}", $"EdFi_Ods_{schoolYear}");
            Database.Connection.ConnectionString = connString;
        }
    }
}
