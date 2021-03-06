using ServiceContracts;
using System;
using System.Diagnostics;

namespace IPSService
{
    public class Audit : IDisposable
    {
        private static EventLog customLog = null;
        private const string SourceName = "SBES-PROJ";
        private const string LogName = "SBES-PROJ-LOG";

        static Audit()
        {
            try
            {
                if (!EventLog.SourceExists(SourceName))
                    EventLog.CreateEventSource(SourceName, LogName);
                customLog = new EventLog(LogName, Environment.MachineName, SourceName);
            }
            catch (Exception e)
            {
                customLog = null;
                Console.WriteLine($"Error while trying to create log handle. Error = {e.Message}");
            }
        }

        public static void CriticalLog(Alarm alarm)
        {
            if (customLog != null)
            {
                string critical = AuditEvents.Critical;
                string message = String.Format(critical, alarm.TimeStamp.ToString(), alarm.Filename, alarm.Path);
                customLog.WriteEntry(message, EventLogEntryType.Error);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.Critical));
            }
        }

        public static void InformationLog(Alarm alarm)
        {
            if (customLog != null)
            {
                string information = AuditEvents.Information;
                string message = String.Format(information, alarm.TimeStamp.ToString(), alarm.Filename, alarm.Path);
                customLog.WriteEntry(message, EventLogEntryType.Information);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.Information));
            }
        }

        public static void WarningLog(Alarm alarm)
        {
            if (customLog != null)
            {
                string warning = AuditEvents.Warning;
                string message = String.Format(warning, alarm.TimeStamp.ToString(), alarm.Filename, alarm.Path);
                customLog.WriteEntry(message, EventLogEntryType.Warning);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.Warning));
            }
        }

        public void Dispose()
        {
            if (customLog != null)
            {
                customLog.Dispose();
                customLog = null;
            }
        }
    }
}