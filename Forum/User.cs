namespace Forum
{
    public class User
    {
        public string UserName { get; set; }
        public bool ObserveAll { get; set; }
        public bool ObserveOwn { get; set; }
        public List<string> AskedQuestions { get; private set; }
        public List<string> AnsweredQuestions { get; private set; }
        public User(string userName)
        {
            UserName = userName;
            ObserveAll = false;
            ObserveOwn = false;
            AskedQuestions = new List<string>();
            AnsweredQuestions = new List<string>();
        }
        public void HandleForumUpdate(object sender, ForumUpdateEventArgs args)
        {
            if (ObserveAll)
            {
                if (args.UpdateType == "Question")
                {
                    Console.WriteLine($"Użytkownik {args.UserName} dodał pytanie {args.ID}");
                }
                else if (args.UpdateType == "Answer")
                {
                   
                    // Potrzebujemy dostępu do pytania powiązanego z tą odpowiedzią, aby uzyskać nazwę użytkownika, który zadał pytanie
                    var forum = (ForumBoard)sender;
                    var relatedQuestion = forum.GetQuestions().Find(q => q.QuestionID == args.ID);
                    if (relatedQuestion != null)
                    {
                        Console.WriteLine($"Użytkownik {args.UserName} udzielił odpowiedzi {args.ID} na pytanie {relatedQuestion.QuestionID} zadane przez użytkownika {relatedQuestion.AskingUser}");
                    }
                    
                }
            }
            else if (ObserveOwn && (AskedQuestions.Contains(args.ID) || UserName == args.UserName))
            {
                if (args.UpdateType == "Question" && UserName == args.UserName)
                {
                    Console.WriteLine($"Dodałeś pytanie {args.ID}");
                }
                else if (args.UpdateType == "Answer")
                {
                    // Potrzebujemy dostępu do pytania powiązanego z tą odpowiedzią, aby sprawdzić, czy to nasze pytanie
                    var forum = (ForumBoard)sender;
                    var relatedQuestion = forum.GetQuestions().Find(q => q.QuestionID == args.ID);
                    if (relatedQuestion != null && relatedQuestion.AskingUser == UserName)
                    {
                        Console.WriteLine($"Użytkownik {args.UserName} udzielił odpowiedzi {args.ID} na Twoje pytanie {relatedQuestion.QuestionID}");
                    }
                }
            }

        }
        public void ObserveAllQuestions(bool observe)
        {
            ObserveAll = observe;
        }
        public void ObserveOwnQuestions(bool observe)
        {
            ObserveOwn = observe;
        }
    }
}