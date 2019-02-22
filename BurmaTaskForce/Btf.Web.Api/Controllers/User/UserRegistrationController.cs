using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Btf.Services.UserService;
using Btf.Web.Api.Controllers.Base;
using Btf.Data.Model.User;
using Btf.Web.Api.DTO;

namespace Btf.Web.Api.Controllers
{
    [Produces("application/json")]
    public class UserRegistrationController : BaseController
    {
        public UserRegistrationController(IUserService userService) : base(userService)
        {
        }

        [HttpGet]
        [Route("api/user/questions/all")]
        public IActionResult GetQuestions()
        {

            List<QuestionDto> questions = new List<QuestionDto>()
                {
                    new QuestionDto () { Id= 0, Question = "What was the name of your elementary/primary school?"},
                    new QuestionDto()  { Id= 1, Question = "Who is your best friend?"},
                    new QuestionDto()  { Id= 2, Question = "Who is/was your favorite teacher"},
                    new QuestionDto()  { Id= 3, Question = "In which city or town does your nearest sibling live?"},
                    new QuestionDto()  { Id= 4, Question = "What is your pet’s name?"}
                };

            return Ok(questions);
        }

        [HttpPost]
        [Route("api/user/registration/init")]
        public async Task<IActionResult> Post([FromBody]User newUser)
        {

            await UserService.AddUserAsync(newUser);
            return Ok("Please check your email for verification key.");

        }
        

        [HttpGet]
        [Route("api/user/email/available/{email}")]
        public async Task<IActionResult> IsEmailAvailable(string email)
        {

            var isAvailable = await UserService.IsEmailAvailable(email);
            return Ok(isAvailable);
        }

        [HttpPut]
        [Route("api/user/password/verifyandchange")]
        public async Task<IActionResult> VerifyAndChangePassword([FromBody] ChangePasswordDto changedPassword)
        {

            await UserService.VerifyAndChangePassword(changedPassword.VerificationKey, changedPassword.NewPassword);

            return Ok();
        }

        [HttpPut]
        [Route("api/user/password/question/verifyandchange")]
        public async Task<IActionResult> VerifyAndChangePasswordWithQuestions([FromBody] ChangePasswordQuestionsDto changedPassword)
        {
            await UserService.VerifyAndChangePassword(changedPassword.VerificationKey, changedPassword.NewPassword, changedPassword.SecretQuestion1,
                changedPassword.SecretQuestion2, changedPassword.SecretAnswer1, changedPassword.SecretAnswer2);

            return Ok();
        }

        [HttpGet]
        [Route("api/user/questions/{emailAddress}")]
        public async Task<IActionResult> GetSecurityQuestions(string emailAddress)
        {
            //LoadTokenInfo();
            var questions = await UserService.GetSecurityQuestions(emailAddress);
            return Ok(questions);
        }

        [HttpPut]
        [Route("api/user/questions/verify")]
        public async Task<IActionResult> VerifySecurityAnswers([FromBody] AnswersDto answers)
        {
            //LoadTokenInfo();

            var valid = await UserService.VerifySecurityAnswers(answers.EmailAddress, answers.Answer1, answers.Answer2);

            if (valid) return Ok();
            else return BadRequest();

        }
    }
}