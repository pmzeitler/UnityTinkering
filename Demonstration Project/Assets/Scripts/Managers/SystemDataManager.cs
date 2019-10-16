using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Mono.Data.Sqlite;


public class SystemDataManager : ScriptableObject
{
    private static SystemDataManager _instance;
    public string Localization { get; private set; }

    private string AssetPrefix { get; set; }

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
            AssetPrefix = Application.dataPath + SystemDatabaseFileName.FILE_SEPARATOR + "Databases" + SystemDatabaseFileName.FILE_SEPARATOR;
            Debug.Log("SystemDataManager created");
            string GameDataFile = SystemDatabaseFileName.GetFilename(SystemDatabaseFile.GAME_DATA);
            string LocalizationFile = SystemDatabaseFileName.GetFilename(SystemDatabaseFile.LOCALIZATION_BASE, Localization);

            Debug.Log("GameData file: " + AssetPrefix + GameDataFile);
            Debug.Log("Localization File: " + AssetPrefix + LocalizationFile);


        }
        else
        {
            if (_instance != this)
            {
                //do nothing;
            }
        }
    }



}

