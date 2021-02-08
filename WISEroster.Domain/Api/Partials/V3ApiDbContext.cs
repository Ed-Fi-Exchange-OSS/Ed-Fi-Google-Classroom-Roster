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
            connString = Regex.Replace(connString, @"\d{4,}", schoolYear.ToString());
            Database.Connection.ConnectionString = connString;
        }
    }
}
