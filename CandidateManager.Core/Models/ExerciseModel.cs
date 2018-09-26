namespace CandidateManager.Core.Models
{
    public class ExerciseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public byte[] FileData { get; set; }
    }
}
