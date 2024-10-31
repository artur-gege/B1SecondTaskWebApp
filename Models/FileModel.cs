namespace B1SecondTaskWebAPI.Models
{
    public class FileModel
    {
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public DateTime UploadedDate { get; set; }

        public ICollection<DataModel> FilesData { get; set; } = new List<DataModel>();
    }
}
