namespace UserService.Exceptions
{
	public class PasswordRequiresDigitException : Exception
	{
		public PasswordRequiresDigitException() : base("Password must contain at least one digit.") { }
	}

	public class PasswordRequiresLowercaseException : Exception
	{
		public PasswordRequiresLowercaseException() : base("Password must contain at least one lowercase letter.") { }
	}

	public class PasswordRequiresUppercaseException : Exception
	{
		public PasswordRequiresUppercaseException() : base("Password must contain at least one uppercase letter.") { }
	}

	public class PasswordRequiresNonAlphanumericException : Exception
	{
		public PasswordRequiresNonAlphanumericException() : base("Password must contain at least one non-alphanumeric character.") { }
	}

	public class PasswordTooShortException : Exception
	{
		public PasswordTooShortException(int minLength) : base($"Password must be at least {minLength} characters long.") { }
	}

	public class PasswordRequiresUniqueCharsException : Exception
	{
		public PasswordRequiresUniqueCharsException(int uniqueChars) : base($"Password must contain at least {uniqueChars} unique characters.") { }
	}
}
