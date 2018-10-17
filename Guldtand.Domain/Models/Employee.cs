using System;
using System.Collections.Generic;
using System.Text;

namespace Guldtand.Domain.Models
{
    public abstract class Employee : IEmployee
    {
        public virtual int Uid { get; set; }
        public virtual string Password { get; set; }
        public virtual string Name { get; set; }
    }

    public class Dentist : Employee
    {

    }

    public class Administrator : Employee
    {

    }
}
