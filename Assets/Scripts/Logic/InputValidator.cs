
using System;
using System.Collections;
using System.Threading.Tasks;
using TMPro;

public class InputValidator
{
	public readonly int minPlayerNameSize = 3;
	public readonly int maxPlayerNameSize = 18;

	public readonly int minPasswordSize = 6;
	public readonly int maxPasswordSize = 30;

	private readonly char[] unpermittedNameCharacters =
		{ ' ', '\\', '/', ';', ':', '\'', '\"', '|', ',', '_', };

	/// <summary>
	/// Validates the <see cref="string"/> <paramref name="playerName"/>. Checks for <see cref="maxPlayerNameSize"/> etc.
	/// </summary>
	/// <param name="playerPassword"></param>
	/// <returns>
	/// <para><see cref="ValidationResult.ShortStringLength"/> : Username too short.</para>
	/// <para><see cref="ValidationResult.LargeStringLength"/> : Username too large.</para>
	/// <para><see cref="ValidationResult.UnusableCharacters"/> : Username contains <see cref="char"/>'s from <see cref="unpermittedNameCharacters"/></para>
	/// <para><see cref="ValidationResult.Validated"/> : Default</para>
	/// </returns>
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

	/// <summary>
	/// Asyncronously returns a <see cref="ValidationResult"/> using <see cref="IDatabaseManager.PlayerExists(NewPlayer, Action{bool})"/>
	/// </summary>
	/// <param name="newPlayer"></param>
	/// <param name="databaseManager"></param>
	/// <param name="callback"></param>
	/// <returns>
	/// <para><see cref="ValidationResult.AlreadyExists"/> : <paramref name="newPlayer"/> already exists in database</para>
	/// <para><see cref="ValidationResult.Validated"/> : <paramref name="newPlayer"/> does not exists in the database yet</para>
	/// </returns>
	public async Task<ValidationResult> ValidatePlayerNameExists(Player newPlayer, IDatabaseManager databaseManager)
	{
		bool playerExists = await databaseManager.PlayerExists(newPlayer);

		return playerExists ? ValidationResult.AlreadyExists : ValidationResult.Validated;
	}

	/// <summary>
	/// Validates the <see cref="string"/> <paramref name="playerPassword"/>. Checks for <see cref="maxPasswordSize"/> etc.
	/// </summary>
	/// <param name="playerPassword"></param>
	/// <returns>
	/// <para><see cref="ValidationResult.ShortStringLength"/> : Password too short.</para>
	/// <para><see cref="ValidationResult.LargeStringLength"/> : Password too large.</para>
	/// <para><see cref="ValidationResult.Validated"/> : Default</para>
	/// </returns>
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

	/// <summary>
	/// Checks whether <paramref name="value"/> contains of the <see cref="char"/>'s in the array <paramref name="chars"/>
	/// </summary>
	/// <param name="value"></param>
	/// <param name="chars"></param>
	/// <returns><b>True:</b> if value contains a char in <paramref name="chars"/>. <b>False:</b> if nothing was found</returns>
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

	/// <summary>
	/// Returns a username error message (<see cref="string"/>) corresponding to the enum value <paramref name="validationResult"/>
	/// </summary>
	/// <param name="validationResult"></param>
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

	/// <summary>
	/// Returns a password error message (<see cref="string"/>) corresponding to the enum value <paramref name="validationResult"/>
	/// </summary>
	/// <param name="validationResult"></param>
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

	/// <summary>
	/// Show validation error for username and password fields using <paramref name="nameResult"/> and <paramref name="passwordResult"/>
	/// <para>
	/// Won't show an error message if <paramref name="nameResult"/> or <paramref name="passwordResult"/> = <see cref="ValidationResult.Validated"/>.
	/// </para>
	/// </summary>
	/// <param name="nameResult"></param>
	/// <param name="passwordResult"></param>
	public void ShowValidationError(ValidationResult nameResult, ValidationResult passwordResult, TMP_Text usernameErrorMessage, TMP_Text passwordErrorMessage)
	{
		string usernameString = GetUsernameErrorMessage(nameResult);
		if (usernameString != null)
		{
			usernameErrorMessage.gameObject.SetActive(true);
			usernameErrorMessage.text = usernameString;
		}

		string passwordString = GetPasswordErrorMessage(passwordResult);
		if (passwordString != null)
		{
			passwordErrorMessage.gameObject.SetActive(true);
			passwordErrorMessage.text = passwordString;
		}
	}
}
