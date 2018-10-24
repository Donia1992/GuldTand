namespace Guldtand.Domain.Models.DTOs
{
    public class XrayBlobDTO : IXrayBlobDTO
    {
        public string _content;

        public XrayBlobDTO(string content)
        {
            _content = content;
        }
    }
}
