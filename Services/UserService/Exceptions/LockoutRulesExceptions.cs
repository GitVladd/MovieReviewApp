namespace UserService.Exceptions
{
	public class LockoutTooManyAttemptsException : Exception
	{
		public LockoutTooManyAttemptsException(int maxAttempts, TimeSpan lockoutTime)
			: base($"Too many failed login attempts. Account is locked for {lockoutTime.TotalMinutes} minutes after {maxAttempts} failed attempts.") { }
	}

	public class LockoutNotAllowedForNewUsersException : Exception
	{
		public LockoutNotAllowedForNewUsersException()
			: base("Lockout is not allowed for new users.") { }
	}
}
