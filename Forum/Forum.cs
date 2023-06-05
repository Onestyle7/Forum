namespace Forum
{
    public class ForumBoard
    {
        public List<Question> Questions;
        public List<Answer> Answers;
        public List<User> Users;
        public delegate void ForumUpdateHandler(object sender, ForumUpdateEventArgs args);

        public event ForumUpdateHandler QuestionPosted;
        public event ForumUpdateHandler AnswerPosted;
        public void OnQuestionPosted(Question question)
        {
            QuestionPosted?.Invoke(this, new ForumUpdateEventArgs("Question", question.QuestionID, question.Questiontext, question.AskingUser));
        }
        public void OnAnswerPosted(Answer answer)
        {
            AnswerPosted?.Invoke(this, new ForumUpdateEventArgs("Answer", answer.AnswerID, answer.Answertext, answer.RespondingUser));
        }
        public ForumBoard()
        {
            Questions = new List<Question>();
            Answers = new List<Answer>();
            Users = new List<User>();
        }
        public void AddUser(User user)
        {
            if(Users.Any (u => u.UserName == user.UserName))
            {
                throw new InvalidOperationException("A user with the same name already exists.");
            }
            else
            {
                Users.Add(user);
                Subscribe(user);
            }
           
        }
        public void AddQuestion(Question question)
        {

            var user = Users.Find(u => u.UserName == question.AskingUser);
            if (user != null)
            {
                user.AskedQuestions.Add(question.QuestionID);
                Questions.Add(question);
                OnQuestionPosted(question);
            }
            else
            {
                Console.WriteLine("User not found. Please provide a valid user.");
            }

        }
        public void AddAnswer(Answer answer)
        {
            var user = Users.Find(u => u.UserName == answer.RespondingUser);
            if (user != null)
            {
                user.AnsweredQuestions.Add(answer.AnswerID);
                Answers.Add(answer);
                OnAnswerPosted(answer);
            }
            else
            {
                Console.WriteLine("User not found. Please provide a valid user.");
            }
        }

        public void Subscribe(User user)
        {
            QuestionPosted += user.HandleForumUpdate;
            AnswerPosted += user.HandleForumUpdate;
        }
        public void Subscribe(ForumStatistics stats)
        {
            QuestionPosted += stats.HandleForumUpdate;
            AnswerPosted += stats.HandleForumUpdate;
        }
        public void Unsubscribe(ForumStatistics stats)
        {
            QuestionPosted -= stats.HandleForumUpdate;
            AnswerPosted -= stats.HandleForumUpdate;
        }
      

        public void Unsubscribe(User user)
        {
            QuestionPosted -= user.HandleForumUpdate;
            AnswerPosted -= user.HandleForumUpdate;
        }
        public List<Question> GetQuestions()
        {
            return Questions;
        }
        public List<Answer> GetAnswers()
        {
            return Answers;
        }
        public List<Question> GetQuestionsByUser(string userName)
        {
            return Questions.FindAll(q => q.AskingUser == userName);
        }
        public List<Answer> GetAnswersByUser(string userName)
        {
            return Answers.FindAll (a => a.RespondingUser == userName);
        }
        public List<Answer> GetAnswersForQuestion(string questionId)
        {
            return Answers.FindAll(a => a.RelatedQuestionID == questionId);
        }

    }
}