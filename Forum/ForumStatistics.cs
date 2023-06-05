namespace Forum
{
    public class ForumStatistics
    {
        public int TotalQuestions { get; private set;}
        public int TotalAnswers { get; private set;}
        public float AvarageAnswersPerQuestion { get => TotalQuestions != 0? (float)TotalAnswers / TotalQuestions : 0; }
        public int UnansweredQuestions { get; private set; }
        public int QuestionsWithAtLeastOneAnswer { get { return TotalQuestions - UnansweredQuestions; } }
        private Dictionary<string, bool> QuestionHasAnswer;
        public ForumStatistics()
        {
            TotalQuestions = 0;
            TotalAnswers = 0;
            UnansweredQuestions = 0;
            QuestionHasAnswer = new Dictionary<string, bool>();
        }
        public void HandleForumUpdate(object sender, ForumUpdateEventArgs args)
        {
           
            if (args.UpdateType == "Question")
            {
                TotalQuestions++;
                UnansweredQuestions++;
                QuestionHasAnswer[args.ID] = false;
            }
            else if (args.UpdateType == "Answer")
            {
                TotalAnswers++;
                if (QuestionHasAnswer.ContainsKey(args.ID) && QuestionHasAnswer[args.ID] == false)
                {
                    QuestionHasAnswer[args.ID] = true;
                    UnansweredQuestions--;
                }
            }
        }
    }
}