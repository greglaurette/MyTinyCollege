namespace MyTinyCollege.Models
{
    public class OfficeAssinment
    {
        public int InstructorID { get; set; }
        public string Location { get; set; }
        public virtual Instructor Instructor { get; set; }
    }
}