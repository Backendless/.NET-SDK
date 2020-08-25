using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackendlessAPI;
using BackendlessAPI.Data;
using BackendlessAPI.Async;
using BackendlessAPI.Persistence;
using UnityEditor;

public class TestDataService : MonoBehaviour {
    
	void Start ()
    {
        #if UNITY_EDITOR
        EditorApplication.playModeStateChanged += HandleOnPlayModeChanged;
        #endif
        SaveTestData();
    }

    void SaveTestData()
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("nameTimeColumn", "Bob Jones " + System.DateTime.UtcNow);

        Debug.Log("Saving test data...");

        Backendless.Data.Of("TestTable")
            .Save(dict, 
                new AsyncCallback<Dictionary<string, object>>(
                    response =>
                    {
                        Debug.Log("Test data saved with objectId " + response["objectId"]);

                        GetTestData();
                    },
                    error =>
                    {
                        Debug.LogError("Failed to save test data: Code = " + error.FaultCode 
                            + ", Message = " + error.Message 
                            + ", Details = " + error.Detail);
                    }
                )
           );
        
        Debug.Log("Adding RT listener...");
        Backendless.Data.Of( "TestTable" ).RT().AddCreateListener((obj) =>
        {
            Debug.Log( "RT: Object created " );
            Debug.Log( "RT object - " + obj );
        });
	}

    void GetTestData()
    {
        DataQueryBuilder query = DataQueryBuilder.Create();
        query.SetWhereClause("nameTimeColumn LIKE '%Bob%'");
        query.AddSortBy("created desc");
        Backendless.Data.Of("TestTable")
            .Find(query, 
                new AsyncCallback<IList<Dictionary<string, object>>>(
                    response =>
                    {
                        Debug.Log ("Found " + response.Count + " test data results. First object ID is " + response[0]["objectId"]);
                    },
                    error =>
                    {
                        Debug.Log("Failed to find test data: Code = " + error.FaultCode 
                                + ", Message = " + error.Message 
                                + ", Details = " + error.Detail);
                    }
                )
            );
    }
    
    #if UNITY_EDITOR
    void HandleOnPlayModeChanged( PlayModeStateChange stateChange )
    {
        // This method is run whenever the playmode state is changed.
        if ( stateChange == PlayModeStateChange.ExitingEditMode || stateChange == PlayModeStateChange.ExitingPlayMode )
        {
            Backendless.RT.Disconnect();
        }
    }
    #endif
}
