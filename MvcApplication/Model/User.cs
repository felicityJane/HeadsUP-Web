using System;

namespace MvcApplication.Model
{
    public class User
    {
        public User() { }

        public User(string userName, string device)
        {
            this.UserName = userName;
            this.Device = device;
        }
        public string UserName { get; set; }

        public string Device { get; set; }

    }
}