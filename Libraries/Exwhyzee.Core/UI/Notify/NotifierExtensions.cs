namespace Exwhyzee.UI.Notify
{
    public static class NotifierExtensions
    {
        /// <summary>
        /// Adds a new UI notification of type Information
        /// </summary>
        /// <seealso cref="Exwhyzee.UI.Notify.INotifier.Add()"/>
        /// <param name="message">The message to display</param>
        public static void Information(this INotifier notifier, string message)
        {
            notifier.Add(NotifyType.Information, message);
        }

        /// <summary>
        /// Adds a new UI notification of type Warning
        /// </summary>
        /// <seealso cref="Exwhyzee.UI.Notify.INotifier.Add()"/>
        /// <param name="message">The message to display</param>
        public static void Warning(this INotifier notifier, string message)
        {
            notifier.Add(NotifyType.Warning, message);
        }

        /// <summary>
        /// Adds a new UI notification of type Error
        /// </summary>
        /// <seealso cref="Exwhyzee.UI.Notify.INotifier.Add()"/>
        /// <param name="message">The message message to display</param>
        public static void Error(this INotifier notifier, string message)
        {
            notifier.Add(NotifyType.Error, message);
        }

        /// <summary>
        /// Adds a new UI notification of type Success
        /// </summary>
        /// <seealso cref="Exwhyzee.UI.Notify.INotifier.Add()"/>
        /// <param name="message">The message to display</param>
        public static void Success(this INotifier notifier, string message)
        {
            notifier.Add(NotifyType.Success, message);
        }
    }
}
