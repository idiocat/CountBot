namespace CountBot.Utilities
{
    public static class NumberCounter
    {
        public static string Count(string message)
        {
            foreach (char n in message) { if (!char.IsDigit(n) && !char.IsWhiteSpace(n) && n != '.' && n != '-') { throw new FormatException(); } }
            List<string> numbers = message.Split(' ').ToList<string>();
            List<int> toClear = [];
            for (int i = 0; i < numbers.Count(); ++i) { if (String.IsNullOrEmpty(numbers[i])) { toClear.Add(i); } }
            toClear.Reverse();
            foreach (int i in toClear) { numbers.RemoveAt(i); }
            return numbers.Sum(n => double.Parse(n)).ToString();
        }
    }
}
