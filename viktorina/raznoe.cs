using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Text.Unicode;


namespace viktorina
{
    public abstract class Reader<T>
    {
        public abstract T Read();

    }
    public class JSONReader<T> : Reader<T> where T : class
    {
        private string _fileName;

        public JSONReader(string fileName)
        {
            _fileName = fileName;
        }

        public override T Read()
        {
            if (File.Exists(_fileName))
            {
                string jsonString = File.ReadAllText(_fileName);
                T data = JsonSerializer.Deserialize<T>(jsonString);
                return data;
            }
            return null;
        }
    }
    public abstract class Writer<T>
    {
        public abstract void Write(T data);
    }
    public class JSONWriter<T> : Writer<T> where T : class
    {
        private string _fileName;

        public JSONWriter(string fileName)
        {
            _fileName = fileName;
        }

        public override void Write(T data)
        {
            string jsonString = JsonSerializer.Serialize<T>(data, new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            });
            File.WriteAllText(_fileName, jsonString);
        }
    }
    public class UserManager
    {
        private Reader<Users> _usersReader;
        private Writer<Users> _usersWriter;
        public Users Users { get; set; }
        public User CurUser { get; set; }

        public UserManager(Reader<Users> usersReader, Writer<Users> usersWriter)
        {
            _usersReader = usersReader;
            _usersWriter = usersWriter;
            Users = _usersReader.Read() == null ? new Users() : _usersReader.Read();
        }

        public void DisplaySignUp()
        {
            string login, password, birthday = "";
            bool isSignUp = true;
            do
            {
                Console.Clear();
                if (!isSignUp)
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Пользователь с таким логином уже существует!");
                    Console.ResetColor();
                    Console.WriteLine("Попробуйте снова");
                }
                else
                {
                    Console.WriteLine();
                    Console.Write(">  Введите дату рождения в формате(yy-mm-dd): ");
                    birthday = Console.ReadLine();
                }
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("Придумайте логин и пароль");
                Console.ResetColor();
                Console.Write(">  Логин: ");
                login = Console.ReadLine();
                Console.Write(">  Пароль: ");
                password = Console.ReadLine();
                isSignUp = Users.SignUp(login, password, birthday);
            } while (!isSignUp);
            _usersWriter.Write(Users);
            CurUser = Users.FindUser(login);
        }
        public void DisplaySignIn()
        {
            string login, password;
            bool isSignIn = true;
            do
            {
                Console.Clear();
                if (!isSignIn)
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Неверный логин или пароль!");
                    Console.ResetColor();
                    Console.WriteLine("Попробуйте снова");
                }
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("Введите логин и пароль для входа");
                Console.ResetColor();
                Console.Write(">  Логин: ");
                login = Console.ReadLine();
                Console.Write(">  Пароль: ");
                password = Console.ReadLine();
                isSignIn = Users.SignIn(login, password);
            } while (!isSignIn);
            CurUser = Users.FindUser(login);
        }
        public void DisplayChangePassword()
        {
            string newPassword, password;
            bool passwordCorrect = true;
            do
            {
                Console.Clear();
                if (!passwordCorrect)
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Неверный пароль!");
                    Console.ResetColor();
                    Console.WriteLine("Попробуйте снова");
                }
                Console.WriteLine();
                Console.Write(">  Введите ваш старый пароль: ");
                password = Console.ReadLine();
                passwordCorrect = Users.CheckPassword(CurUser.Login, password);
            } while (!passwordCorrect);
            Console.Write(">  Введите новый пароль: ");
            newPassword = Console.ReadLine();
            Users.ChangeUserPassword(CurUser.Login, newPassword);
            _usersWriter.Write(Users);
            CurUser = Users.FindUser(CurUser.Login);
        }
        public void DisplayChangeBirthday()
        {
            string newBirthday;
            Console.Clear();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine($"Текущая дата рождения: {CurUser.Birthday.ToShortDateString()}");
            Console.ResetColor();
            Console.Write(">  Введите новую дату рождения в формате(yy-mm-dd): ");
            newBirthday = Console.ReadLine();
            Users.ChangeUserBirthday(CurUser.Login, newBirthday);
            _usersWriter.Write(Users);
            CurUser = Users.FindUser(CurUser.Login);
        }
    }
    public class QuizManager
    {
        private Reader<Quizzes> _quizzesReader;
        private Quizzes _quizzes;
        public QuizManager(Reader<Quizzes> quizzesReader)
        {
            _quizzesReader = quizzesReader;
            _quizzes = _quizzesReader.Read() == null ? new Quizzes() : _quizzesReader.Read();
            int numQuestions = GetAllQuestions().Count < 20 ? GetAllQuestions().Count : 20;
            _quizzes.Add(GetMixedQuiz(numQuestions));
        }
        public Score StartQuiz(QuizType quizType, string title, User curUser)
        {
            Quiz curQuiz = _quizzes.Find((quiz) => quiz.Type == quizType && quiz.Title == title);
            List<Question> questions = curQuiz.Questions;

            int countRightAnswers = 0;
            for (int i = 0; i < questions.Count; i++)
            {
                Console.Clear();
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine($"\"{curQuiz.Title}\"");
                Console.ResetColor();

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Если правильных ответов несколько, введите их через пробелы (пример: 1 2)");
                Console.ResetColor();


                Question question = questions[i];

                Console.WriteLine();
                Console.WriteLine($" {i + 1}) {question.Text}");
                Console.WriteLine();

                List<Answer> answers = question.Answers;
                for (int j = 0; j < answers.Count; j++)
                {
                    Answer answer = answers[j];
                    Console.WriteLine($"   {j + 1} - {answer.Text}");
                }
                Console.WriteLine();
                Console.Write(">  ");
                List<int> userAnswers = Console.ReadLine().Split(' ').
                                    Where(a => !string.IsNullOrWhiteSpace(a)).
                                    Select(a => int.Parse(a)).ToList();
                countRightAnswers += userAnswers.FindAll(a => answers[a - 1].IsCorect).Count;
            }
            return new Score(curUser.Login, curQuiz, countRightAnswers);
        }
        private Quiz GetMixedQuiz(int numQuestions)
        {
            return new Quiz(QuizType.Mixed, "Смешаная викторина", GetRandomQuestions(numQuestions));
        }
        public List<Question> GetAllQuestions()
        {
            List<Question> res = new List<Question>();
            foreach (var quiz in _quizzes)
            {
                foreach (var question in quiz.Questions)
                {
                    res.Add(question);
                }
            }
            return res;
        }
        public List<Question> GetRandomQuestions(int num)
        {
            List<Question> res = new List<Question>();
            List<Question> allQuestions = GetAllQuestions();
            Random rnd = new Random();
            for (int i = 0; i < num; i++)
            {
                int randInd = rnd.Next(0, allQuestions.Count - 1);
                Question randQuestion = allQuestions[randInd];
                res.Add(randQuestion);
            }
            return res;
        }
        public List<string> GetQuizzesTitles(QuizType type)
        {
            List<string> res = new List<string>();
            foreach (var quiz in _quizzes)
            {
                if (quiz.Type == type)
                    res.Add(quiz.Title);
            }
            return res;
        }
    }
    public class ScoreManager
    {
        private Reader<Scores> _scoresReader;
        private Writer<Scores> _scoresWriter;
        private Scores _scores;

        public ScoreManager(Reader<Scores> scoresReader, Writer<Scores> scoresWriter)
        {
            _scoresReader = scoresReader;
            _scoresWriter = scoresWriter;
            _scores = _scoresReader.Read() == null ? new Scores() : _scoresReader.Read();
        }
        public void AddScore(Score score)
        {
            if (_scores.CheckScoreExists(score.UserLogin, score.QuizTitle))
                _scores.Remove(_scores.FindScore(score.UserLogin, score.QuizTitle));
            _scores.Add(score);
            _scoresWriter.Write(_scores);
        }
        public List<Score> GetTop(string quizTitle)
        {
            List<Score> scoresQuiz = _scores.FindAll((s) => s.QuizTitle == quizTitle);
            List<Score> topScores = scoresQuiz.OrderByDescending((s) => s.RightAnswers).ToList();
            return topScores;
        }
        public List<Score> GetScoresUser(string login)
        {
            return _scores.FindAll((s) => s.UserLogin == login);
        }
        public void DispayScoresUser(string login)
        {
            Console.Clear();
            Console.WriteLine();
            List<Score> scoresUser = GetScoresUser(login);
            if (scoresUser.Count == 0)
            {
                Console.WriteLine();
                Console.WriteLine("Нет результатов!");
                Console.WriteLine("Похоже вы не прошли еще ни одной викторины.");
            }
            else
            {
                foreach (var score in scoresUser)
                {
                    Console.WriteLine($"  {score.QuizTitle} - {score}");
                }
            }
        }
        public void DispayTopScores(int topAmount, string quizTitle)
        {
            Console.Clear();
            Console.WriteLine();
            List<Score> topScores = GetTop(quizTitle);
            if (topScores.Count == 0)
            {
                Console.WriteLine();
                Console.WriteLine("Нет результатов по данной векторине!");
            }
            else
            {
                topAmount = topAmount > topScores.Count ? topScores.Count : topAmount;
                for (int i = 0; i < topAmount; i++)
                {
                    Score score = topScores[i];
                    Console.WriteLine($"{i + 1})  {score.UserLogin} - {score}");
                }
            }
        }
        public void DispayUserScInTop(string quizTitle, Score userScore)
        {
            Console.WriteLine();
            List<Score> topScores = GetTop(quizTitle);
            int indUserSc = topScores.IndexOf(userScore);
            int delta = 3;
            int start = indUserSc > delta ? indUserSc - delta : 0;
            int end = indUserSc < topScores.Count - delta - 1 ? indUserSc + delta : topScores.Count - 1;
            for (int i = start; i <= end; i++)
            {
                Score score = topScores[i];
                if (score.UserLogin == userScore.UserLogin)
                {
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine($"  {i + 1})  {score.UserLogin} - {score}");
                    Console.ResetColor();
                }
                else
                    Console.WriteLine($"{i + 1})  {score.UserLogin} - {score}");
            }
        }
    }

}
