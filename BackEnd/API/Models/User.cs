﻿namespace FinalAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public User(String username, String email, String password)
        {
            this.Username = username;
            this.Email = email;
            this.Password = password;
        }
    }

}