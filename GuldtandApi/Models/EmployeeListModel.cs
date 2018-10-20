using Guldtand.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
