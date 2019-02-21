using System;
using System.Collections.Generic;
using System.Text;

namespace Btf.Data.Model.Base
{
    /**
     * Base entity -- Implementation of IEntity Interface
     * 
     **/
    public class BaseEntity : IEntity
    {
        public int Id { get ; set; }
        public int CreatedBy { get ; set ; }
        public int UpdatedBy { get ; set ; }
        public DateTime CreatedOn { get ; set ; }
        public DateTime UpdatedOn { get ; set ; }
        public bool IsActive { get; set; }
    }
}
