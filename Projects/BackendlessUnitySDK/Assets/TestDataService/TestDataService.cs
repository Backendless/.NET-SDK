using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackendlessAPI;
using BackendlessAPI.Data;
using BackendlessAPI.Async;
using BackendlessAPI.Persistence;

public class TestDataService : MonoBehaviour {
	void Start ()
    {
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
}
