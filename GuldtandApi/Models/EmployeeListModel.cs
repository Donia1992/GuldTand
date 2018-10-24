using Guldtand.Domain.Models;
using System.Collections.Generic;

namespace GuldtandApi.Models
{
    public class EmployeeListModel
    {
        public List<IEmployee> Content { get; set; }

        public EmployeeListModel(List<IEmployee> content)
        {
            Content = content;
        }
    }
}
