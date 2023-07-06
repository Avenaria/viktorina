using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace viktorina
{
    public class User
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public DateTime Birthday { get; set; }

        public User()
        {
        }

        public User(string login, string password, DateTime birthday) //пользватель регистарция 
        {
            Login = login;
            Password = password;
            Birthday = birthday;
        }
    }

    public class Users : List<User>// проверка пользвателя 
    {
        public bool SignUp(string login, string password, string birthday)
        {
            if (CheckUserExists(login))//проверкана строковое имя пользователя, строковый пароль
                return false;
            this.Add(new User(login, password, DateTime.Parse(birthday)));
            return true;
        }
        public bool SignIn(string login, string password)// регистрация
        {
            User user = FindUser(login);
            if (!CheckUserExists(login))
                return false;
            return user.Password == password;
        }
        public void ChangeUserPassword(string login, string newPassword) //заменить пароль
        {
            User user = FindUser(login);
            this.Add(new User(login, newPassword, user.Birthday));
            this.Remove(user);
        }

        public void ChangeUserBirthday(string login, string newBirthday)//поменять дату
        {
            User user = FindUser(login);
            this.Add(new User(login, user.Password, DateTime.Parse(newBirthday)));
            this.Remove(user);
        }
        public bool CheckPassword(string login, string password)//проверка пароля 
        {
            return FindUser(login).Password == password;
        }
        private bool CheckUserExists(string login)//cписок пользв
        {
            return FindUser(login) != null;
        }
        public User FindUser(string login)
        {
            return this.FirstOrDefault(u => u.Login == login);
        }
    }
}
