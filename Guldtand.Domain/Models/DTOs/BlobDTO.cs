namespace Guldtand.Domain.Models.DTOs
{
    public class BlobDTO : IBlobDTO
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
