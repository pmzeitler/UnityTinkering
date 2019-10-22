using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Mono.Data.Sqlite;
using System.Data.Common;

public class SystemDataManager : ScriptableObject
{
    private static SystemDataManager _instance;
    public string Localization { get; private set; }

    private string AssetPrefix { get; set; }

    private Dictionary<SystemDatabaseFile, DbConnection> dbConnections;

    public static SystemDataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = ScriptableObject.CreateInstance<SystemDataManager>();
            }

            return _instance;
        }

        private set
        {
            _instance = value;
        }
    }





    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            Localization = "en-us";
            AssetPrefix = Application.dataPath + SystemDatabaseFileName.FILE_SEPARATOR + "Databases"; // + SystemDatabaseFileName.FILE_SEPARATOR;
            Debug.Log("SystemDataManager created");
            string GameDataFile = SystemDatabaseFileName.GetFilename(SystemDatabaseFile.GAME_DATA);
            string LocalizationFile = SystemDatabaseFileName.GetFilename(SystemDatabaseFile.LOCALIZATION_BASE, Localization);

            dbConnections = new Dictionary<SystemDatabaseFile, DbConnection>();

            //Debug.Log("GameData file: " + AssetPrefix + GameDataFile);
            //Debug.Log("Localization File: " + AssetPrefix + LocalizationFile);

            //TODO: fix this
     
            Debug.Log("Testing DB Connection....");
            SetUpDatabase(SystemDatabaseFile.LOCALIZATION_BASE, AssetPrefix + LocalizationFile);

            DBDialogue dialogue = new DBDialogue(GetDbConnection(SystemDatabaseFile.LOCALIZATION_BASE));
            dialogue.TextKey = "SAMPLE_TEXT";
            dialogue.Load();

            Debug.Assert(dialogue.LocalizedText == "This is some sample text for localization.", "Did not get correct dialogue fragment for method setting TextKey directly");

            DBDialogue dialogue2 = new DBDialogue(GetDbConnection(SystemDatabaseFile.LOCALIZATION_BASE));
            dialogue2.Load("SAMPLE_TEXT_ALT");

            Debug.Assert(dialogue2.LocalizedText == "This is different sample text for localization.", "Did not get correct dialogue fragment for method load with params");
          
        }
        else
        {
            if (_instance != this)
            {
                //do nothing;
            }
        }
    }

    private DbConnection GetDbConnection(SystemDatabaseFile dbType)
    {
        return dbConnections[dbType];
    }

    private void SetUpDatabase(SystemDatabaseFile dbType, string DatabaseLocation)
    {
        Debug.Log("Attempting to load DB for type " + dbType);
        if (!dbConnections.ContainsKey(dbType))
        {
            dbConnections[dbType] = null;
        }
        
        SqliteConnection connectionObj = (SqliteConnection) dbConnections[dbType];

        string sanitizedString = /* "file:" + */ SanitizeConnectionString(DatabaseLocation);

        if (connectionObj == null)
        {
            string connString;
            /*
            SqliteConnectionStringBuilder builder = new SqliteConnectionStringBuilder
            {
                Uri = new Uri(sanitizedString).AbsoluteUri,
                //DataSource = sanitizedString,
                //DateTimeFormat = SQLiteDateFormats.ISO8601,
                //SyncMode = SynchronizationModes.Off,
                //JournalMode = SQLiteJournalModeEnum.Off,
                //DefaultIsolationLevel = System.Data.IsolationLevel.ReadCommitted,
                //Pooling = true,
                ReadOnly = true,
                Version = 3,

            };

            connString = builder.ConnectionString;
            */

            connString = "Data Source=file:" + sanitizedString + ";Version=3;Read Only=True;";

            Debug.Log("connString: " + connString);
            connectionObj = new SqliteConnection(connString);
            connectionObj.Open();
            dbConnections[dbType] = connectionObj;

        } else
        {
            //do nothing; assume that this is okay
        }
    }


    public string SanitizeConnectionString(string connStringIn)
    {
        string retval = connStringIn;

        retval = retval.Replace(" ", "%20");

        //retval = retval.Replace("/", "\\");

        return retval;
    }


}

