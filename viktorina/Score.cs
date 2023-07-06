using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace viktorina
{
    public class Score // рейтинг
    {
        public string UserLogin { get; set; }
        public string QuizTitle { get; set; }
        public int RightAnswers { get; set; }
        public int Max { get; set; }
        public Score()
        {
        }
        public Score(string userLogin, Quiz quiz, int rightAnswers)
        {
            UserLogin = userLogin;
            QuizTitle = quiz.Title;
            RightAnswers = rightAnswers;
            Max = CountMax(quiz);
        }
        public int CountMax(Quiz quiz)
        {
            int max = 0;
            foreach (var question in quiz.Questions)
            {
                foreach (var answer in question.Answers)
                {
                    if (answer.IsCorect)
                        max++;
                }
            }
            return max;
        }

        public override string ToString()
        {
            return $"{RightAnswers} из {Max} ({RightAnswers * 100 / Max} %)";
        }
    }
    public class Scores : List<Score>
    {
        public Scores()
        {

        }

        public bool CheckScoreExists(string userLogin, string quizTitle)//проверка показателей
        {
            return FindScore(userLogin, quizTitle) != null;
        }
        public Score FindScore(string userLogin, string quizTitle)
        {
            return this.FirstOrDefault(s => s.UserLogin == userLogin && s.QuizTitle == quizTitle);
        }
    }
}
