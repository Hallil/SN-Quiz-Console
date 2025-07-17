namespace CSA_Quiz
{
    class Question
    {
        public string Text { get; set; }
        public List<string> Options { get; set; }
        public string CorrectAnswer { get; set; }
    }

    class Program
    {
        static void Main()
        {
            string file = "vragen.txt";
            var questions = ReadQuestions(file);

            // Shuffle the questions
            var random = new Random();
            questions = questions.OrderBy(q => random.Next()).ToList();

            int correct = 0;

            for (int i = 0; i < questions.Count; i++)
            {
                var question = questions[i];
                Console.WriteLine($"\nQuestion {i + 1}: {question.Text}");
                foreach (var opt in question.Options)
                    Console.WriteLine(opt);

                Console.Write("Your answer (for multiple letters, without spaces, e.g.: BCD): ");
                string input = Console.ReadLine().ToUpper().Replace(" ", "");

                if (input == question.CorrectAnswer)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Correct! 🎉");
                    correct++;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Wrong! The correct answer is: {question.CorrectAnswer}");
                }
                Console.ResetColor();
            }

            double percentage = (double)correct / questions.Count * 100;
            Console.WriteLine($"\nEnd of quiz! You got {correct} out of {questions.Count} correct ({percentage:0.##}%)");
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        static List<Question> ReadQuestions(string pad)
        {
            var questions = new List<Question>();
            var lines = File.ReadAllLines(pad);

            int i = 0;
            while (i < lines.Length)
            {
                // Find start of a question
                while (i < lines.Length && !lines[i].StartsWith("Question:"))
                    i++;
                if (i >= lines.Length) break;

                var question = new Question();
                question.Text = lines[i].Substring(6).Trim();
                question.Options = new List<string>();

                i++;
                // Read answer options
                while (i < lines.Length && (lines[i].StartsWith("A.") || lines[i].StartsWith("B.") || lines[i].StartsWith("C.") || lines[i].StartsWith("D.") || lines[i].StartsWith("E.") || lines[i].StartsWith("F.") || lines[i].StartsWith("G.") || lines[i].StartsWith("H.")))
                {
                    question.Options.Add(lines[i]);
                    i++;
                }
                // Read the correct answer
                if (i < lines.Length && lines[i].StartsWith("Suggested answer:"))
                {
                    question.CorrectAnswer = lines[i].Substring("Suggested answer:".Length).Trim().ToUpper();
                    i++;
                }
                // Skip empty lines
                while (i < lines.Length && string.IsNullOrWhiteSpace(lines[i]))
                    i++;

                questions.Add(question);
            }
            return questions;
        }
    }
}