using System;
using System.Collections.Generic;
using System.Text;

namespace Btf.Data.Model.Base
{
    /**
     * Define base entity interface
     *  Common fields are included in this interface
     * */
    public interface IEntity
    {
        int Id { get; set; }
        int CreatedBy { get; set; }
        int UpdatedBy { get; set; }
        DateTime CreatedOn { get; set; }
        DateTime UpdatedOn { get; set; }

        /**
         * This field is used for Soft Delete
         * */
        bool IsActive { get; set; }

    }
}
