using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WISEroster.Domain.Api;

namespace WISEroster.Business
{
    public interface IOrganizationBusiness
    {
        EducationOrganization GetEducationOrganization(short schoolYear, int edOrgId);
        IQueryable<EducationOrganization> GetEducationOrganizations(short schoolYear, List<int> edOrgIds);
        IQueryable<EducationOrganization> GetLeas(short schoolYear);
        IQueryable<School> GetSchoolsForLea(short schoolYear,int leaId, int? schoolCategory);
        IQueryable<Descriptor> GetSchoolCategoriesForLea(short schoolYear, int leaId);
    }

    public class OrganizationBusiness : IOrganizationBusiness
    {
        private readonly IV3ApiDbContext _context;

        public OrganizationBusiness(IV3ApiDbContext context)
        {
            _context = context;
        }

        public EducationOrganization GetEducationOrganization(short schoolYear, int edOrgId)
        {
            _context.ChangeSchoolYear(schoolYear);
            return _context.EducationOrganizations.FirstOrDefault(e => e.EducationOrganizationId == edOrgId);
        }

        public IQueryable<EducationOrganization> GetEducationOrganizations(short schoolYear, List<int> edOrgIds)
        {
            _context.ChangeSchoolYear(schoolYear);
            return _context.EducationOrganizations.Where(e => edOrgIds.Contains(e.EducationOrganizationId));
        }

        public IQueryable<EducationOrganization> GetLeas(short schoolYear)
        {
            _context.ChangeSchoolYear(schoolYear);
            return _context.EducationOrganizations.Where(e=>e.LocalEducationAgency!=null && e.LocalEducationAgency.Schools.Any(s=>s.LocalEducationAgencyId!= s.SchoolId));//don't include choice schools
        }

        public IQueryable<School> GetSchoolsForLea(short schoolYear, int leaId, int? schoolCategory)
        {
            _context.ChangeSchoolYear(schoolYear);
            var q= _context.Schools.Where(s => s.LocalEducationAgencyId==leaId);
            if (schoolCategory != null)
            {
                q= q.Where(s => s.SchoolCategories.Any(c => c.SchoolCategoryDescriptorId == schoolCategory));
            }

            return q.Include(s=>s.EducationOrganization);
        }

        public IQueryable<Descriptor> GetSchoolCategoriesForLea(short schoolYear, int leaId)
        {
            _context.ChangeSchoolYear(schoolYear);
            return _context.SchoolCategoryDescriptors.Where(d=>d.SchoolCategories.Any(c=>c.School.LocalEducationAgencyId==leaId)).Select(c=>c.Descriptor);
        }
    }
}
