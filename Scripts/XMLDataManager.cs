//2018 Jordan Black

using System.Collections;
using System.Collections.Generic;

using System.Xml.Serialization;
using System.IO;

// ================================================================================
// Saving and Loading
// ================================================================================

[System.Serializable]
[XmlRoot ( "DataCollection" )]
public class XMLDataManager<T>
{
	[XmlArray("Data")]
	[XmlArrayItem("DataItem")]
	public List<T> DataList = new List<T>();
	
	//default constructor
	public DataSerializer ( )
	{
		DataList = new List<T> ( );
	}

	public void SaveData ( string _path, T _dataObject )
	{
		DataList.Add ( _dataObject );
    
		var serializer = new XmlSerializer(typeof(DataSerializer<T>));
		using ( var stream = new FileStream ( _path, FileMode.Create ) )
		{
			serializer.Serialize ( stream, this );
		}
	}

	public T ReturnData ( int _dataIndex )
	{
		return DataList [ _dataIndex ];
	}

	public static DataSerializer<T> LoadData ( string _path )
	{
		if ( File.Exists ( _path ) == false ) return null;

		var serializer = new XmlSerializer(typeof(DataSerializer<T>));
		using ( var stream = new FileStream ( _path, FileMode.Open ) )
		{
			return serializer.Deserialize ( stream ) as DataSerializer<T>;
		}

	}

	//Loads the xml directly from the given string. Useful in combination with www.text.
	public static DataSerializer<T> LoadFromText ( string text )
	{
		var serializer = new XmlSerializer(typeof(DataSerializer<T>));
		return serializer.Deserialize ( new StringReader ( text ) ) as DataSerializer<T>;
	}

}

// ================================================================================
// Saving and Loading Usage
// ================================================================================

// ----- USE THIS TO SAVE ----- 
////create a local manager of the desired list type, and then use it to pass the save path and list through to save the data.
//public void SaveDataToFile ( )
//{
//	XMLDataManager<List<PlayerScore>> playerScores = new XMLDataManager<List<PlayerScore>>();
//	playerScores.SaveData ( Application.persistentDataPath + "/PlayerScores.xml", playerScoresList );
//}

// ----- USE THIS TO LOAD -----
////create a local data serializer of the desired list type, and then try to load the file and assign the data to your list.
//public bool LoadDataFromFile ( )
//{
//	XMLDataManager<List<PlayerScore>> playerScores = new XMLDataManager<List<PlayerScore>>();

//	try
//	{
//		dailyDataLoader = XMLDataManager<List<PlayerScore>>.LoadData ( Application.persistentDataPath + "/PlayerScores.xml" );
//		playerScoresList = playerScores.ReturnData ( 0 );
//	}
//	catch ( System.Exception _err )
//	{
//		Debug.LogWarning ( _err );
//	}

//	if ( playerScores != null && playerScores.DataList.Count > 0 )
//	{
//		Debug.Log ( "Loaded score data from xml file." );
//		return true;
//	}
//	else
//	{
//		Debug.LogWarning ( "Failed to load score data from xml file." );
//		return false;
//	}
//}


// ================================================================================
// Load from text usage.
// ================================================================================

//if ( DataList.Count == 0 )
//{
//	string xmlData = @"<DataCollection><Data><DataItem targetID=""1""><Position>(0, 0, 0)</Position></DataItem></Data></DataCollection>";
//	LoadFromText ( xmlData );
//}
