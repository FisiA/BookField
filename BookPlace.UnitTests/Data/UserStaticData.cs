using BookPlace.Core.DTO.User;

namespace BookPlace.UnitTests.Data
{
    public static class UserStaticData
    {
        public static RegisterUserDTO GetRegisterUserDtoDetails()
        {
            return new RegisterUserDTO
            {
                UserName = "test",
                Email = "test@email.com",
                Name = "Test",
                Surname = "User",
                Password = "test123"
            };
        }

        public static LoginUserDTO GetLoginUserDtoDetails()
        {
            return new LoginUserDTO
            {
                UserName = "dell_user",
                Password = "test@123_980"
            };
        }

        public static List<UserDTO> GetUsers()
        {
            return new List<UserDTO>
            {
                new UserDTO
                {
                    Id = "fc15b06f-9d40-4fd3-8caa-54d09be238bf",
                    UserName = "dell_user",
                    Email = "dell@email.com",
                    Name = "Dell",
                    Surname = "User",
                },
                new UserDTO
                {
                    Id = "9d3a6b40-3983-4905-9d85-322f26bbc1b5 ",
                    UserName = "acer_user",
                    Email = "acer@email.com",
                    Name = "Acer",
                    Surname = "User",
                },
                new UserDTO
                {
                    Id = "3878fd48-e988-41ad-ba83-35b2e85fd4c5",
                    UserName = "hp_user",
                    Email = "hp@email.com",
                    Name = "HP",
                    Surname = "User",
                }
            };
        }
    }
}
