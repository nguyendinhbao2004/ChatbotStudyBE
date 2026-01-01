using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBotApplication.Dto.Course
{
    public class CourseResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string InstructorId { get; set; }
        public string SubjectName { get; set; } // Flatten: Lấy tên môn học thay vì ID
        public string Status { get; set; }// Trả về string cho dễ đọc (Draft/Published)
        public string Level { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set;
    }
}
