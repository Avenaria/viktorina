using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace viktorina
{
    public enum QuizType // перечесление  групп по вопросам 
    {
        History,
        Physics,
        Mixed
    }
    public class Quizzes : List<Quiz>
    {
    }
    public class Quiz // сама игра
    {

        public QuizType Type { get; set; }
        public string Title { get; set; }
        public List<Question> Questions { get; set; }

        public Quiz()
        {
        }

        public Quiz(QuizType type, string title, List<Question> questions)
        {
            Type = type;
            Title = title;
            Questions = questions;
        }
    }
}
