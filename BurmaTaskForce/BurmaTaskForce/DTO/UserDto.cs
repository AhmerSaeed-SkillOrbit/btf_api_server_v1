using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Btf.Data.Model.User;

namespace Btf.Web.Api.DTO
{
    public class UserDto
    {
        public UserDto(User user)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            MobileNumber = user.MobileNumber;
            Address = user.Address;
            Address1 = user.Address1;
            ZipCode = user.ZipCode;
            //SecretAnswer1 = user.SecretAnswer1;
            //SecretAnswer2 = user.SecretAnswer2;
            //SecretQuestion1 = user.SecretQuestion1;
            //SecretQuestion2 = user.SecretQuestion2;

            SecretAnswer1 = "";
            SecretAnswer2 = "";
            SecretQuestion1 = "";
            SecretQuestion2 = "";

            UserGUID = user.UserGUID;
            CreatedBy = user.CreatedBy;
            UpdatedBy = user.UpdatedBy;
            CreatedOn = user.CreatedOn;
            UpdatedOn = user.UpdatedOn;
            IsActive = user.IsActive;
        }

       

        

        public int Id { get; private set; }
        public int EndPointId { get; set; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public string MobileNumber { get; }
        public string Address { get; }
        public string Address1 { get; set; }
        public string ZipCode { get; }
        public string SecretAnswer1 { get; }
        public string SecretAnswer2 { get; }
        public string SecretQuestion1 { get; }
        public string SecretQuestion2 { get; }
        public string UserGUID { get; private set; }
        public int CreatedBy { get; private set; }
        public int UpdatedBy { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public DateTime UpdatedOn { get; private set; }
        public bool IsActive { get; private set; }
    }
}
