namespace UserService.Exceptions
{
	public class UserRequiresUniqueEmailException : Exception
	{
		public UserRequiresUniqueEmailException()
			: base("User email must be unique.") { }
	}

	public class UsernameMustBeUniqueException : Exception
	{
		public UsernameMustBeUniqueException()
			: base("The username must be unique.")
		{
		}
	}

	public class InvalidUserNameCharactersException : Exception
	{
		public InvalidUserNameCharactersException(string allowedCharacters)
			: base($"Username contains invalid characters. Allowed characters are: {allowedCharacters}") { }
	}


}
