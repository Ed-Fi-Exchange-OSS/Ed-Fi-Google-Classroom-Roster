using System.Collections.Generic;

namespace WISEroster.Mvc.Models
{
    public class DeleteCourseMessage
    {
        public int LeaId { get; set; }
        public string UserEmail { get; set; }
        public List<string> Ids { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}