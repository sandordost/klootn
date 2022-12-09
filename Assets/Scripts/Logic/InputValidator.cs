
using System;
using System.Collections;

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

	public IEnumerator ValidatePlayerNameExists(NewPlayer newPlayer, IDatabaseManager databaseManager, Action<ValidationResult> callback)
	{
		yield return databaseManager.PlayerExists(newPlayer, (exists) =>
		{
			if (exists) callback.Invoke(ValidationResult.AlreadyExists);
			else callback.Invoke(ValidationResult.Validated);
		});
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

	public string GetUsernameErrorMessage(ValidationResult validationResult)
	{
		return validationResult switch
		{
			ValidationResult.ShortStringLength =>
				$"Username is shorter than {minPlayerNameSize} characters",
			ValidationResult.LargeStringLength =>
				$"Username is larger than {maxPlayerNameSize} characters",
			ValidationResult.UnusableCharacters =>
				$"Username contains unusable characters",
			ValidationResult.AlreadyExists =>
				$"Username already exists",
			ValidationResult.DoesNotExist =>
				$"Username does not exist",
			ValidationResult.Validated => null,
			_ => null,
		};
	}

	public string GetPasswordErrorMessage(ValidationResult validationResult)
	{
		return validationResult switch
		{
			ValidationResult.ShortStringLength =>
				$"Password is shorter than {minPasswordSize} characters",
			ValidationResult.LargeStringLength =>
				$"Password is larger than {maxPasswordSize} characters",
			ValidationResult.UnusableCharacters =>
				$"Password contains unusable characters",
			ValidationResult.PasswordIncorrect =>
				$"Password is incorrect",
			ValidationResult.Validated => null,
			_ => null
		};
	}
}
