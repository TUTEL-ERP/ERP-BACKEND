namespace server.Dto
{




    public class LoginUserReqDto
    {
        public string Email { get; set; }
        public string Password { get; set; }

    }


    public class LoginUserResDto
    {
        public string AccessToken { get; set; }
         
        public string RefreshToken { get; set; }
        
        public string Username { get; set; } 
        public int  UserId { get; set; }

        public string Role { get; set; }




    }
}
