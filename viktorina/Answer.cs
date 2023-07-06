using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace viktorina
{
    public class Answer /// класс ответов
    {
        public string Text { get; set; }
        public bool IsCorect { get; set; }

        public Answer() // конструктор по умолчанию без него ругается 
        {

        }

        public Answer(string text, bool isCorect)
        {
            Text = text;
            IsCorect = isCorect;
        }
    }
    public class Question // вопросы 
    {
        public string Text { get; set; }
        public List<Answer> Answers { get; set; }

        public Question()
        {

        }

        public Question(string text, List<Answer> answers)
        {
            Text = text;
            Answers = answers;
        }
    }
}
