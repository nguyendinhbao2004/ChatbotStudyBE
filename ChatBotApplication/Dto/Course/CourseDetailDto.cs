namespace ChatBotApplication.Dto.Course
{
    public class CourseDetailDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Level { get; set; } // Trả về string thay vì enum int cho FE dễ hiển thị
        public string ImageUrl { get; set; }
        public DateTime CreatedDate { get; set; }

        // Thông tin bảng liên kết (Subject)
        public Guid SubjectId { get; set; }
        public string SubjectName { get; set; }

        // Thông tin người tạo (Instructor)
        public Guid InstructorId { get; set; }
        public string InstructorName { get; set; }
    }
}