
namespace ScooterRental
{
    public class EventJournal
    {
        private static string _path = Environment.CurrentDirectory;
        private static string _filePath = Path.GetFullPath(Path.Combine(_path, @"..\..\..\"));
        private readonly string _logFilePath = _filePath + "logs.txt";

        public EventJournal()
        {

        }

        public void LogEvent(string eventMessage)
        {
            if (!File.Exists(_logFilePath))
            {
                File.Create(_logFilePath);
                WriteLog(eventMessage);
            }
            else
            {
                WriteLog(eventMessage);
            }
        }

        private void WriteLog(string eventMessage)
        {
            // Create or append to the log file
            using (StreamWriter writer = File.AppendText(_logFilePath))
            {
                writer.WriteLine($"{DateTime.Now}: {eventMessage}");
            }
        }
    }
}
