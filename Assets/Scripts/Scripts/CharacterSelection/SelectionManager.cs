
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectChar : MonoBehaviour
{
	// The left marker out of visible scene
	public Transform markerLeft2;
	// The left marker of visible scene
	public Transform markerLeft;
	// The middle marker of visible scene
	public Transform markerMiddle;
	// The right marker of visible scene
	public Transform markerRight;
	// The right marker out of visible scene
	public Transform markerRight2;

	// The characters prefabs to pick
	public Transform[] charsPrefabs;
	// Textures for buttons


	// The game objects created to be showed on screen
	private GameObject[] chars;

	// The index of the current character
	public int currentChar = 0;
	public Button buyButton;
	public Transform ButtonHolder;
	public List<Button> Buybuttons;

	[SerializeField] CharacterSceneManager characterSceneManager;

	void Awake()
	{
		int index = 0;
		foreach (Transform t in charsPrefabs)
		{
			// characterSceneManager.iAPManager.AddToBuilder(charsPrefabs[index].GetComponent<CharacterSelector>().productID);
			Button btn = Instantiate(buyButton, ButtonHolder);
			btn.GetComponentsInChildren<TMP_Text>()[0].text = "$" + charsPrefabs[index].GetComponent<CharacterSelector>().Price;
			btn.GetComponentsInChildren<TMP_Text>()[1].text = charsPrefabs[index].GetComponent<CharacterSelector>().Name;
			int capturedIndex = index;
			btn.onClick.AddListener(() => AddlistnerToButton(capturedIndex));
			Buybuttons.Add(btn);
			btn.gameObject.SetActive(false);
			print("Product added to builder" + charsPrefabs[index].GetComponent<CharacterSelector>().productID);
			index++;
		}
		if (index > 1)
		{
			Buybuttons[0].gameObject.SetActive(true);
		}
	}

	void AddlistnerToButton(int num)
	{
		characterSceneManager.iAPManager.BuyCharacter(Buybuttons[num], charsPrefabs[num].GetComponent<CharacterSelector>().productID);
	}

	void Start()
	{

		// Initialize the chars array
		chars = new GameObject[charsPrefabs.Length];

		// Create game objects based on characters prefabs
		int index = 0;
		foreach (Transform t in charsPrefabs)
		{
			chars[index] = Instantiate(t.gameObject, markerRight2.position, Quaternion.identity);
			characterSceneManager.characters.Add(chars[index]);
			//Add products to builder

			index++;
		}


	}

	void OnGUI()
	{
		ManageCharacterPositions();

	}

	public void MoveToPreviousCharacter()
	{
		currentChar--;
		if (currentChar < 0) currentChar = chars.Length - 1;
		characterSceneManager.changeButton.Invoke();

	}

	public void MoveToNextCharacter()
	{
		currentChar++;
		if (currentChar >= chars.Length) currentChar = 0;
		characterSceneManager.changeButton.Invoke();

	}

	private void ManageCharacterPositions()
	{
		int middleIndex = currentChar;
		int leftIndex = currentChar - 1;
		int rightIndex = currentChar + 1;

		for (int index = 0; index < chars.Length; index++)
		{
			Transform transf = chars[index].transform;

			if (index < leftIndex)
			{
				transf.position = Vector3.Lerp(transf.position, markerLeft2.position, Time.deltaTime);
			}
			else if (index > rightIndex)
			{
				transf.position = Vector3.Lerp(transf.position, markerRight2.position, Time.deltaTime);
			}
			else if (index == leftIndex)
			{
				transf.position = Vector3.Lerp(transf.position, markerLeft.position, Time.deltaTime);
			}
			else if (index == middleIndex)
			{
				transf.position = Vector3.Lerp(transf.position, markerMiddle.position, Time.deltaTime);
			}
			else if (index == rightIndex)
			{
				transf.position = Vector3.Lerp(transf.position, markerRight.position, Time.deltaTime);
			}
		}
	}
}
