using System.Runtime.InteropServices;

namespace Forum
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var forum = new ForumBoard();
            var stats = new ForumStatistics();
            forum.Subscribe(stats);

            while (true)
            {
                Console.WriteLine("1. Dodaj użytkownika\n2. Dodaj pytanie\n3. Dodaj odpowiedź\n4. Wyświetl statystyki\n5. Wyświetl pytania\n0. Wyjdź");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Podaj nazwę użytkownika: ");
                        var userName = Console.ReadLine();
                        forum.AddUser(new User(userName));
                        break;
                    case "2":
                        Console.Write("Podaj nazwę użytkownika: ");
                        userName = Console.ReadLine();
                        if (!forum.Users.Any(u => u.UserName == userName))
                        {
                            Console.WriteLine("Użytkownik nie istnieje.");
                            break;
                        }
                        Console.Write("Podaj tekst pytania: ");
                        var questionText = Console.ReadLine();
                        var question = new Question(questionText, Guid.NewGuid().ToString(), userName);
                        forum.AddQuestion(question);
                        break;
                    case "3":
                        Console.Write("Podaj nazwę użytkownika: ");
                        userName = Console.ReadLine();
                        if (!forum.Users.Any(u => u.UserName == userName))
                        {
                            Console.WriteLine("Użytkownik nie istnieje.");
                            break;
                        }
                        Console.Write("Podaj ID pytania: ");
                        var questionId = Console.ReadLine();
                        if (!forum.Questions.Any(q => q.QuestionID == questionId))
                        {
                            Console.WriteLine("Pytanie nie istnieje.");
                            break;
                        }
                        Console.Write("Podaj tekst odpowiedzi: ");
                        var answerText = Console.ReadLine();
                        var answer = new Answer(answerText, Guid.NewGuid().ToString(), userName, questionId);
                        forum.AddAnswer(answer);
                        break;
                    case "4":
                        Console.WriteLine($"Liczba pytań: {stats.TotalQuestions}");
                        Console.WriteLine($"Liczba odpowiedzi: {stats.TotalAnswers}");
                        Console.WriteLine($"Średnia liczba odpowiedzi na pytanie: {stats.AvarageAnswersPerQuestion}");
                        break;
                    case "5":
                        foreach (var q in forum.GetQuestions())
                        {
                            Console.WriteLine($"ID: {q.QuestionID}, User: {q.AskingUser}, Text: {q.Questiontext}");
                        }
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Nieznana opcja.");
                        break;
                }
            }
        }
    }
    
    public class Question
    {
        public string Questiontext {get; set;}
        public  string QuestionID { get; set;}
        public string AskingUser { get; set;}
        public Question(string questionText, string questionId, string askingUser)
        { 
            Questiontext = questionText;
            QuestionID = questionId;
            AskingUser = askingUser;
        }
    }
    public class Answer
    {
       public string Answertext { get; set;}
       public string AnswerID { get; set;}
       public string RespondingUser { get; set;}
       public string RelatedQuestionID { get; set;}

       public Answer(string answerText, string answerID, string respondingUser, string relatedQuestionId)
        {
            Answertext = answerText;
            AnswerID = answerID;
            RespondingUser = respondingUser;
            RelatedQuestionID = relatedQuestionId;

        }
    }
    public class ForumUpdateEventArgs : EventArgs
    {
        public string UpdateType { get; set; }
        public string ID { get; set; }
        public string Text { get; set; }
        public string UserName { get; set; }

        public ForumUpdateEventArgs(string updateType, string id, string text, string userName)
        {
            UpdateType = updateType;
            ID = id;
            Text = text;
            UserName = userName;
        }
    }
}