using System;
using System.Collections.Generic;
using System.Text;

namespace Guldtand.Domain.Models
{
    public abstract class Customer : ICustomer
    {
        public virtual int Uid { get; set; }
        public virtual string Firstname { get; set; }
        public virtual string Lastname { get; set; }
        public virtual string Socialsecurity_number { get; set; }
        public virtual string Phone { get; set; }
        public virtual string Email { get; set; }
        public virtual string Street { get; set; }
        public virtual string Zip { get; set; }
        public virtual string City { get; set; }
        public virtual bool Insurance { get; set; }
    }

    public class Person : Customer
    {

    }

}

