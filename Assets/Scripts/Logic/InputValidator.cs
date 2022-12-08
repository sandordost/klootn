
public class InputValidator
{
	public readonly int minPlayerNameSize = 3;
	public readonly int maxPlayerNameSize = 20;
	public readonly int minPasswordSize = 6;
	public readonly int maxPasswordSize = 20;

	private readonly char[] unpermittedNameCharacters =
		{ ' ', '\\', '/', ';', ':', '\'', '\"', '|', ',', '_', };

	public ValidationResult ValidatePlayerName(string playerName)
	{
		//Validate name
		if (playerName.Length < minPlayerNameSize)
		{
			return ValidationResult.ShortStringLength;
		}
		else if (playerName.Length > maxPlayerNameSize)
		{
			return ValidationResult.LargeStringLength;
		}
		else if (ContainsChar(playerName, unpermittedNameCharacters))
		{
			return ValidationResult.UnusableCharacters;
		}

		//Input is valid
		return ValidationResult.Validated;
	}

	public ValidationResult ValidatePlayerPassword(string playerPassword)
	{
		//Validate password
		if (playerPassword.Length < minPasswordSize)
		{
			return ValidationResult.ShortStringLength;
		}
		else if (playerPassword.Length > maxPasswordSize)
		{
			return ValidationResult.LargeStringLength;
		}

		return ValidationResult.Validated;
	}

	private bool ContainsChar(string value, char[] chars)
	{
		foreach (char _char in chars)
		{
			if (value.Contains(_char))
			{
				return true;
			}
		}
		return false;
	}
}
