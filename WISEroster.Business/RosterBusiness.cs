using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WISEroster.Business.Models;
using WISEroster.Domain.Api;

namespace WISEroster.Business
{
    public interface IRosterBusiness
    {
        IQueryable<string> GetSessions(short schoolYear, int leaId, int? schoolCategory, int[] schools);
        IList<CourseCodeAndTitle> GetCourseTitles(short schoolYear, int leaId, int? schoolCategory, int[] schools, string sessionName);
        IQueryable<Descriptor> GetGrades(short schoolYear, int leaId, int? schoolCategory, int[] schools);
        IQueryable<StaffModel> GetStaff(short schoolYear, int leaId, int? schoolCategory, int[] schools, string sessionName);
        IList<CourseCodeAndTitle> GetCourseTitlesByTeacher(short schoolYear, int leaId, int? schoolCategory, int[] schools, string sessionName, int? selectedTeacher);
    }

    public class RosterBusiness : IRosterBusiness
    {
        private readonly IV3ApiDbContext _context;

        public RosterBusiness(IV3ApiDbContext context)
        {
            _context = context;
        }

        public IQueryable<string> GetSessions(short schoolYear, int leaId, int? schoolCategory, int[] schools)
        {
            _context.ChangeSchoolYear(schoolYear);
            var q = _context.Sessions.AsNoTracking().AsQueryable();
            if (schools != null && schools.Length > 0 && schools.Any(s=>s>0))
            {
                q = q.Where(s => schools.Contains(s.SchoolId));
            }
            else
            {
                q = q.Where(s => s.School.LocalEducationAgencyId == leaId);

                if (schoolCategory != null)
                {
                    q = q.Where(s => s.School.SchoolCategories.Any(c => c.SchoolCategoryDescriptorId == schoolCategory));
                }

            }

            return q.Select(s=>s.SessionName).Distinct();
        }

        public IList<CourseCodeAndTitle> GetCourseTitles(short schoolYear, int leaId, int? schoolCategory, int[] schools, string sessionName)
        {
            _context.ChangeSchoolYear(schoolYear);
            var q = _context.CourseOfferings.AsNoTracking().AsQueryable();
            if (schools != null && schools.Length > 0 && schools.Any(s => s > 0))
            {
                q = q.Where(s => schools.Contains(s.SchoolId));
            }
            else
            {
                q = q.Where(s => s.School.LocalEducationAgencyId == leaId);

                if (schoolCategory != null)
                {
                    q = q.Where(s => s.School.SchoolCategories.Any(c => c.SchoolCategoryDescriptorId == schoolCategory));
                }

            }

            if (sessionName != null)
            {
                q = q.Where(s => s.SessionName==sessionName);
            }

            return q.Select(s =>new CourseCodeAndTitle{CourseCode = s.LocalCourseCode, CourseTitle = s.LocalCourseTitle}).Distinct().ToList();
        }

        public IQueryable<Descriptor> GetGrades(short schoolYear, int leaId, int? schoolCategory, int[] schools)
        {
            _context.ChangeSchoolYear(schoolYear);
            var q = _context.SchoolGradeLevels.AsNoTracking().AsQueryable();
            if (schools != null && schools.Length > 0 && schools.Any(s => s > 0))
            {
                q = q.Where(s => schools.Contains(s.SchoolId));
            }
            else
            {
                q = q.Where(s => s.School.LocalEducationAgencyId == leaId);

                if (schoolCategory != null)
                {
                    q = q.Where(s => s.School.SchoolCategories.Any(c => c.SchoolCategoryDescriptorId == schoolCategory));
                }

            }

            return q.Select(s => s.GradeLevelDescriptor.Descriptor).Distinct();
        }

        public IQueryable<StaffModel> GetStaff(short schoolYear, int leaId, int? schoolCategory, int[] schools, string sessionName)
        {
            _context.ChangeSchoolYear(schoolYear);
            var q = _context.StaffSectionAssociations.AsNoTracking().AsQueryable();
            if (schools != null && schools.Length > 0 && schools.Any(s => s > 0))
            {
                q = q.Where(s => schools.Contains(s.SchoolId));
            }
            else
            {
                q = q.Where(s => s.Section.CourseOffering.School.LocalEducationAgencyId == leaId);

                if (schoolCategory != null)
                {
                    q = q.Where(s => s.Section.CourseOffering.School.SchoolCategories.Any(c => c.SchoolCategoryDescriptorId == schoolCategory));
                }

            }
            if (sessionName != null)
            {
                q = q.Where(s => s.SessionName == sessionName);
            }
            return q.Select(s => new StaffModel { FirstName = s.Staff.FirstName, LastSurname = s.Staff.LastSurname, StaffUsi = s.StaffUSI }).Distinct();
        }

        public IList<CourseCodeAndTitle> GetCourseTitlesByTeacher(short schoolYear, int leaId, int? schoolCategory, int[] schools, string sessionName, int? selectedTeacher)
        {
            _context.ChangeSchoolYear(schoolYear);
            var q = _context.CourseOfferings.AsNoTracking().AsQueryable();
            if (schools != null && schools.Length > 0 && schools.Any(s => s > 0))
            {
                q = q.Where(s => schools.Contains(s.SchoolId));
            }
            else
            {
                q = q.Where(s => s.School.LocalEducationAgencyId == leaId);

                if (schoolCategory != null)
                {
                    q = q.Where(s => s.School.SchoolCategories.Any(c => c.SchoolCategoryDescriptorId == schoolCategory));
                }

            }

            if (sessionName != null)
            {
                q = q.Where(s => s.SessionName == sessionName);
            }

            if (selectedTeacher != null)
            {
                q = q.Where(s => s.Sections.SelectMany(sec => sec.StaffSectionAssociations.Select(c => c.StaffUSI == selectedTeacher)).FirstOrDefault());
            }

            return q.Select(s => new CourseCodeAndTitle { CourseCode = s.LocalCourseCode, CourseTitle = s.LocalCourseTitle }).Distinct().ToList();
        }
    }
}
