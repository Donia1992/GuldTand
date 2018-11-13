namespace Guldtand.Domain.Models
{
    public class BlobDTO
    {
        public string Name;
        public string Url;

        public BlobDTO(string name, string url)
        {
            Name = name;
            Url = url;
        }
    }
}
